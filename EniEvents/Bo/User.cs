using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bo
{
    class User: IIdentifiable
    {
        /* Attributes */
        public int Id
        {
            get; set;
        }

        public string Username
        {
            get; set;
        }

        public string Password
        {
            get; set;
        }

        public string Email
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


        public virtual List<Role> roles
        {
            get; set;
        }

    }
}
