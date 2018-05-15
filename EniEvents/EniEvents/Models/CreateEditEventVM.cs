using Bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EniEvents.Models
{
    public class CreateEditEventVM
    {
        public Event Event { get; set; }

        public IEnumerable<Thema> Themas { get; set; }

        public int? IdSelectedThema { get; set; }
    }
}