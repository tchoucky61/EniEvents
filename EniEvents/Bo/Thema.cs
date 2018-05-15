using Bo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bo
{
    public class Thema : IIdentifiable
    {
        public int Id { get; set; }

        [Display(Name = "Titre")]
        public string Title { get; set; }
       
    }
    

    
}
