using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bo;
using System.ComponentModel.DataAnnotations;

namespace Bo
{
    public class Utilisateur: IIdentifiable
    {
        public Utilisateur()
        {
            this.Events = new List<Event>();
        }

        /* Attributes */
        public int Id
        {
            get; set;
        }

        [Display(Name = "Adresse")]
        public string Address
        {
            get; set;
        }

        [Display(Name = "Ville")]
        public string City
        {
            get; set;
        }

        [Display(Name = "Code postal")]
        public string Zipcode
        {
            get; set;
        }

        [Display(Name = "Numéro de téléphone")]
        public string Phone
        {
            get; set;
        }

        public string ApplicationUserId
        {
            get; set;
        }

        public virtual List<Event> Events
        {
            get; set;
        }

    }
}
