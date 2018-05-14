using Bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bo
{
    public class Picture : IIdentifiable
    {
        public int Id { get; set; }

        public string Uri { get; set; }

        public virtual Event Event { get; set; }

    }
}
