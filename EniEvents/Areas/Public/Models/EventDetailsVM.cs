using Bo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EniEvents.Areas.Public.Models
{
    public class EventDetailsVM
    {
        public Event Event { get; set; }

        public List<Park> Parks { get; set; }

        public Utilisateur Utilisateur { get; set; }
    }
}