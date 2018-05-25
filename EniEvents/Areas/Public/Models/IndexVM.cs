using Bo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EniEvents.Areas.Public.Models
{
    public class IndexVM
    {
        [Display(Name = "Thème")]
        public IEnumerable<Thema> Themas { get; set; }

        [Display(Name = "Période")]
        public PeriodEnum Periods { get; set; }

        public int? fooSelectedId { get; set; }
    }
}