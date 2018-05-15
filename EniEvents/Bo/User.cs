using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bo;

namespace Bo
{
    public class User: IIdentifiable
    {
        public User()
        {
            this.Events = new List<Event>();
        }

        /* Attributes */
        public int Id
        {
            get; set;
        }

        public string Address
        {
            get; set;
        }

        public string City
        {
            get; set;
        }

        public string Zipcode
        {
            get; set;
        }

        public string Phone
        {
            get; set;
        }

        public virtual List<Event> Events
        {
            get; set;
        }

    }
}
