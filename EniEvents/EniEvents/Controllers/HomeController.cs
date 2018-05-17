using Bo;
using Dal;
using EniEvents.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EniEvents.Controllers
{

    public class HomeController : Controller
    {
        private const string PARKING_LIST_API_URL = "http://data.citedia.com/r1/parks?crs=EPSG:4326";
        private const string GMAP_DISTANCE_API_URL = "https://maps.googleapis.com/maps/api/distancematrix/json?units=metrics&origins={origins}&destinations={destinations}&key=AIzaSyBj842_CpEx66J2_YKNGpXF676QJpgo4z8";

        private EventRepository repoEvent;
        private IRepository<Thema> repoThema;

        Context dbContext = new Context();
        public HomeController()
        {
            repoEvent = new EventRepository(this.dbContext);
            repoThema = new GenericRepository<Thema>(this.dbContext);
        }

        public ActionResult Index()
        {
            var vm = new IndexVM();
            vm.Themas = repoThema.GetAll();

            return View(vm);
        }

        public string EventJson(int period = 0, int themaId = 0)
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

            // Get free parkings.
            List<Park> parks = new List<Park>();
            var parksJson = "";
            using (WebClient wc = new WebClient())
            {
                parksJson = wc.DownloadString(PARKING_LIST_API_URL);
            }

            JObject parksJsonObj = JObject.Parse(parksJson);
            JArray parksArray = (JArray)parksJsonObj.SelectToken("parks");
            JArray featuresArray = (JArray)parksJsonObj.SelectToken("features").SelectToken("features");
            foreach (JObject parkObj in parksArray)
            {
                Park park = new Park();
                park.Id = parkObj.GetValue("id").Value<string>();
                park.Name = ((JObject)parkObj.SelectToken("parkInformation")).GetValue("name").Value<string>();
                park.Status = ((JObject)parkObj.SelectToken("parkInformation")).GetValue("status").Value<string>() == "AVAILABLE";
                park.Free = ((JObject)parkObj.SelectToken("parkInformation")).GetValue("free").Value<int>();
                park.Max = ((JObject)parkObj.SelectToken("parkInformation")).GetValue("max").Value<int>();
                if (park.Free >= 10 && park.Status)
                {
                    parks.Add(park);
                }
            }
            foreach (JObject featureObj in featuresArray)
            {
                var parkId = featureObj.GetValue("id").Value<string>();
                var park = (Park)parks.Find(p => p.Id == parkId);
                if (parks.Contains(park))
                {
                    park.Long = ((JArray)((JObject)featureObj.SelectToken("geometry")).GetValue("coordinates")).First.Value<float>();
                    park.Lat = ((JArray)((JObject)featureObj.SelectToken("geometry")).GetValue("coordinates")).Last.Value<float>();
                }
            }

            // Get distance from event.
            var gmapJson = "";
            foreach (var park in parks)
            {
                using (WebClient wc = new WebClient())
                {
                    var url = GMAP_DISTANCE_API_URL.Replace("{origins}", park.Lat.ToString().Replace(",", ".") + "," + park.Long.ToString().Replace(",", "."))
                        .Replace("{destinations}", ev.Lat.ToString().Replace(",", ".") + "," + ev.Long.ToString().Replace(",", "."));
                    gmapJson = wc.DownloadString(url);
                }
                JObject gmapJsonObj = JObject.Parse(gmapJson);
                JToken gmapRowTkn = gmapJsonObj["rows"].First;
                JToken gmapElementTkn = gmapRowTkn["elements"].First;
                JToken gmapDistanceTkn = gmapElementTkn.SelectToken("distance");
                var distance =((JObject)gmapDistanceTkn).GetValue("value").Value<int>();

                park.DistanceToEvent = distance;
            }

            // Sort parkings.
            parks.Sort();


            // Keep only 3 parks.
            if (parks.Count > 3)
            {
                parks.RemoveRange(3, parks.Count - 3);
            }

            vm.Parks = parks;

            return PartialView(vm);
        }
    }
}