using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bo
{
    public class Cost: IComparable
    {
        public int duration
        {
            get; set;
        }
        public double price
        {
            get; set;
        }

        public int CompareTo(object c)
        {
            if (((Cost)c).duration == this.duration)
            {
                return 0;
            }
            else
            {
                return ((Cost)c).duration > this.duration ? -1 : 1;
            }
        }
    }
}
