using Bo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EniEvents.Areas.Admin.Models
{
    public class CreateEditEventVM
    {
        public Event Event { get; set; }

        [Display(Name = "Thème")]
        public IEnumerable<Thema> Themas { get; set; }

        public int? IdSelectedThema { get; set; }
    }
}