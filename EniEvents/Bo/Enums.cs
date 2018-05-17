using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bo
{
    public enum PeriodEnum
    {
        [Display(Name ="Aujourd'hui")]
        Today = 0,
        [Display(Name = "7 prochains jours")]
        NextSevenDays = 1,
        [Display(Name = "30 prochains jours")]
        NextThirtyDays = 2,
    }
}
