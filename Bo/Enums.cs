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
        [Display(Name = "7 prochains jours")]
        NextSevenDays = 0,
        [Display(Name ="Aujourd'hui")]
        Today = 1,
        [Display(Name = "30 prochains jours")]
        NextThirtyDays = 2,
    }
}
