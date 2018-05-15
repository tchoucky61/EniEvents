using Bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bo
{
    public class Event : IIdentifiable
    {
        public Event()
        {
            this.Users = new List<Utilisateur>();
        }

        public int Id { get; set; }

        [Display(Name = "Titre")]
        public string Title { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [Display(Name = "Heure de début")]
        public string Hour { get; set; }

        [Display(Name = "Durée en heure(s)")]
        public int Duration { get; set; }

        [Display(Name = "Adresse")]
        public string Address { get; set; }

        [Display(Name = "Ville")]
        public string City { get; set; }

        [Display(Name = "Code postal")]
        [DataType(DataType.PostalCode)]
        public string Zipcode { get; set; }

        public string Description { get; set; }

        [Display(Name = "Latitude")]
        public string Lat { get; set; }

        [Display(Name = "Longitude")]
        public string Long { get; set; }

        [Display(Name = "Thème associé")]
        public virtual Thema Thema { get; set; }

        [Display(Name = "Image(s)")]
        public virtual List<Picture> Pictures { get; set; }

        [Display(Name = "Liste des convives")]
        public virtual List<Utilisateur> Users { get; set; }
    }
}
