using Bo;
using Dal;
using EniEvents.Areas.Public.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EniEvents.Areas.Public.Controllers
{

    public class HomeController : Controller
    {
        private const string PARKING_LIST_API_URL = "https://data.rennesmetropole.fr/api/records/1.0/search/?dataset=export-api-parking-citedia&rows=20";
        private const string GMAP_DISTANCE_API_URL = "https://maps.googleapis.com/maps/api/distancematrix/json?units=metrics&origins={origins}&destinations={destinations}&key=AIzaSyB_7BsCOJKiwRPrRWoPMCm8C7QqLO_4Y-4";
        private const string GMAP_API_URL = "https://maps.googleapis.com/maps/api/geocode/json?address={address}CA&key=AIzaSyB_7BsCOJKiwRPrRWoPMCm8C7QqLO_4Y-4";

        private EventRepository repoEvent;
        private IRepository<Thema> repoThema;
        private UtilisateurRepository repoUtilisateur;

        Context dbContext = new Context();

        public HomeController()
        {
            repoEvent = new EventRepository(this.dbContext);
            repoUtilisateur = new UtilisateurRepository(this.dbContext);
            repoThema = new GenericRepository<Thema>(this.dbContext);
        }
        
        public ActionResult Index()
        {
            var vm = new IndexVM();
            vm.Themas = repoThema.GetAll();

            return View(vm);
        }

        public string EventJson(int period = (int) PeriodEnum.NextSevenDays, int themaId = 0)
        {
            List<Event> events = repoEvent.getByPeriodAndThema(period, themaId);
            var json = new JavaScriptSerializer().Serialize(events);

            return json;
        }

        public ActionResult EventDetails(int id = 0)
        {
            EventDetailsVM vm = new EventDetailsVM();
            Event ev = repoEvent.GetById((int)id);
            if (ev == null)
            {
                return HttpNotFound();
            }

            vm.Event = ev;

            // Get Utilisateur.
            var currentUserId = User.Identity.GetUserId();
            vm.Utilisateur = repoUtilisateur.getByUserId(currentUserId);

            return PartialView(vm);
        }

        public string ParkList(int id = 0, string startAddress = "")
        {
            Event ev = repoEvent.GetById((int)id);
            if (ev == null)
            {
                return "<div class=\"no-result\">Aucun parking trouvé</div>";
            }

            // Get free parks.
            List<Park> parks = new List<Park>();
            var parksJson = "";
            var gmapJson = "";
            var wsStatus = true;
            string[,] durations = new string[,] { { "15", "tarif_15" },
                {"30", "tarif_30" },
                {"60", "tarif_1h" },
                {"90", "tarif_1h30" },
                {"120", "tarif_2h" },
                {"180", "tarif_3h" },
                {"240", "tarif_4h" } };


            // Geocode startAddress.
            double startAddressLat = 0.00;
            double startAddressLong = 0.00;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    var url = GMAP_API_URL.Replace("{address}", startAddress.Replace(",", "").Replace("  ", " ").Replace(" ", "+"));
                    gmapJson = wc.DownloadString(url);
                }
                catch (WebException e)
                {
                    wsStatus = false;
                }
            }

            if (wsStatus)
            {
                JObject gmapJsonObj = JObject.Parse(gmapJson);
                JToken gmapRowTkn = gmapJsonObj["results"].First;
                JToken gmapElementTkn = gmapRowTkn["geometry"].SelectToken("location");
                startAddressLat = ((JObject)gmapElementTkn).GetValue("lat").Value<double>();
                startAddressLong = ((JObject)gmapElementTkn).GetValue("lng").Value<double>();
            }


            // Get park list.
            wsStatus = true;
            using (WebClient wc = new WebClient())
            {
                try
                {
                    parksJson = wc.DownloadString(PARKING_LIST_API_URL);
                }
                catch (WebException e)
                {
                    wsStatus = false;
                }
            }

            if (wsStatus)
            {
                JObject parksJsonObj = JObject.Parse(parksJson);
                JArray parksArray = (JArray)parksJsonObj.SelectToken("records");
                foreach (JObject parkJsonObj in parksArray)
                {
                    Park park = new Park();
                    JObject parkDataObj = (JObject)parkJsonObj.SelectToken("fields");
                    park.Free = parkDataObj.GetValue("free").Value<int>();
                    park.Status = parkDataObj.GetValue("status").Value<string>() == "OUVERT";
                    if (park.Free >= 10 && park.Status)
                    {
                        park.Id = parkDataObj.GetValue("id").Value<int>();
                        park.Name = parkDataObj.GetValue("key").Value<string>();
                        byte[] bytes = System.Text.Encoding.Default.GetBytes(park.Name);
                        park.Name = System.Text.Encoding.UTF8.GetString(bytes);
                        park.Max = parkDataObj.GetValue("max").Value<int>();
                        park.Schedule = parkDataObj.GetValue("orgahoraires").Value<string>();
                        for (var i = 0; i < durations.Length / 2; i++)
                        {
                            park.Costs.Add(new Cost()
                            {
                                duration = int.Parse(durations[i, 0]),
                                price = parkDataObj.GetValue(durations[i, 1]).Value<double>(),

                            });
                        }

                        park.Costs.Sort();
                        double cost = 0.00;

                        foreach (Cost c in park.Costs)
                        {
                            cost = c.price;
                            if (c.duration > ev.Duration * 60)
                            {
                                break;
                            }
                        }

                        if (ev.Duration * 60 > park.Costs.Last().duration)
                        {
                            cost += ((park.Costs.Last().duration - ev.Duration - 1) / 15 + 1) * park.Costs.Where(c => c.duration == 15).First().price;
                        }

                        park.Cost = cost;
                        park.Long = ((JArray)((JObject)parkJsonObj.SelectToken("geometry")).GetValue("coordinates")).First.Value<float>();
                        park.Lat = ((JArray)((JObject)parkJsonObj.SelectToken("geometry")).GetValue("coordinates")).Last.Value<float>();

                        parks.Add(park);
                    }
                }
            }

            // Get distance from event.
            gmapJson = "";
            foreach (var park in parks)
            {
                wsStatus = true;
                using (WebClient wc = new WebClient())
                {
                    try
                    {
                        var url = GMAP_DISTANCE_API_URL.Replace("{origins}", park.Lat.ToString().Replace(",", ".") + "," + park.Long.ToString().Replace(",", "."))
                        .Replace("{destinations}", ev.Lat.ToString().Replace(",", ".") + "," + ev.Long.ToString().Replace(",", ".")) + "&mode=walking";
                        gmapJson = wc.DownloadString(url);
                    }
                    catch (WebException e)
                    {
                        wsStatus = false;
                    }

                }

                if (wsStatus)
                {
                    JObject gmapJsonObj = JObject.Parse(gmapJson);
                    JToken gmapRowTkn = gmapJsonObj["rows"].First;
                    JToken gmapElementTkn = gmapRowTkn["elements"].First;
                    JToken gmapDistanceTkn = gmapElementTkn.SelectToken("distance");
                    var distance = ((JObject)gmapDistanceTkn).GetValue("value").Value<int>();

                    park.DistanceToEvent = distance;
                }
            }

            // Get distance from startAddress.
            gmapJson = "";
            foreach (var park in parks)
            {
                wsStatus = true;
                using (WebClient wc = new WebClient())
                {
                    try
                    {
                        var url = GMAP_DISTANCE_API_URL.Replace("{origins}", park.Lat.ToString().Replace(",", ".") + "," + park.Long.ToString().Replace(",", "."))
                        .Replace("{destinations}", startAddressLat.ToString().Replace(",", ".") + "," + startAddressLong.ToString().Replace(",", ".")) + "&mode=driving";
                        gmapJson = wc.DownloadString(url);
                    }
                    catch (WebException e)
                    {
                        wsStatus = false;
                    }

                }

                if (wsStatus)
                {
                    JObject gmapJsonObj = JObject.Parse(gmapJson);
                    JToken gmapRowTkn = gmapJsonObj["rows"].First;
                    JToken gmapElementTkn = gmapRowTkn["elements"].First;
                    JToken gmapDistanceTkn = gmapElementTkn.SelectToken("distance");
                    var distance = ((JObject)gmapDistanceTkn).GetValue("value").Value<int>();

                    park.DistanceToStart = distance;
                }
            }

            // Sort parkings.
            parks.Sort();


            // Keep only 3 parks.
            if (parks.Count > 3)
            {
                parks.RemoveRange(3, parks.Count - 3);
            }

            ViewEngineResult  viewEngineResult = ViewEngines.Engines.FindPartialView(ControllerContext, "~/Areas/Public/Views/Home/ParkList.cshtml");
            var view = viewEngineResult.View;
            ControllerContext.Controller.ViewData.Model = parks;

            string result = null;
            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(ControllerContext, view, ControllerContext.Controller.ViewData, ControllerContext.Controller.TempData, sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }
            return result;
        }
    }
}