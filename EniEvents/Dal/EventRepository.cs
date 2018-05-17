using Bo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class EventRepository : GenericRepository<Event>
    {
        public EventRepository(Context context) : base(context)
        {

        }

        public override void Update(Event ev)
        {
            Event e = GetById(ev.Id);
            e.Id = ev.Id;
            e.Address = ev.Address;
            e.Hour = ev.Hour;
            e.Lat = ev.Lat;
            e.Long = ev.Long;
            e.Pictures = ev.Pictures;
            e.Users = ev.Users;
            e.Title = ev.Title;
            e.Zipcode = ev.Zipcode;
            e.City = ev.City;
            e.Date = ev.Date;
            e.Duration = ev.Duration;
            e.Thema = ev.Thema;
            this.dbContext.Entry(e).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        public List<Event> getByPeriodAndThema(int period, int themaId)
        {
            var maxDate = DateTime.Now;
            var h = 23;
            var m = 59;
            var s = 59;

            switch (period)
            {
                case (int) PeriodEnum.Today:
                    maxDate = new DateTime(maxDate.Year, maxDate.Month, maxDate.Day, h, m, s);
                    break;
                case (int)PeriodEnum.NextSevenDays:
                    maxDate = maxDate.AddDays(7);
                    break;
                case (int)PeriodEnum.NextThirtyDays:
                    maxDate = maxDate.AddDays(30);
                    break;
                default:
                    maxDate = new DateTime(maxDate.Year, maxDate.Month, maxDate.Day, h, m, s);
                    break;
            }

            Expression<Func<Event, bool>> ex;
            if (themaId > 0)
            {
                ex = ev => ev.Thema.Id == themaId && ev.Date != null && DateTime.Compare((DateTime) ev.Date, maxDate) < 0;
            }
            else
            {
                ex = ev => (ev.Date != null && DateTime.Compare((DateTime) ev.Date, maxDate) < 0) || ev.Id > 0;
            }
            IQueryable<Event> eventsQuery = set.Where(ex);

            if (eventsQuery.Count() > 0)
            {
                return eventsQuery.ToList();
            }
            else
            {
                return new List<Event>();
            }
        }
    }
}
