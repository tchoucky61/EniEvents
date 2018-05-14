using Bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bo
{
    public class Event : IIdentifiable
    {
        public Event()
        {
            this.Users = new List<User>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public int Lat { get; set; }

        public int Long { get; set; }

        public virtual Thema Thema { get; set; }

        public virtual List<Picture> Pictures { get; set; }

        public virtual List<User> Users { get; set; }
    }
}
