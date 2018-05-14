using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bo;

namespace Bo
{
    class Role : IIdentifiable
    {
        /* Attributes */
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
    }
}
