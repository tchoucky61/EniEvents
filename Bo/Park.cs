using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bo
{
    public class Park : IComparable, IIdentifiable
    {
        public Park()
        {
            this.Costs = new List<Cost>();
        }

        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public bool Status
        {
            get; set;
        }
        public int Free
        {
            get; set;
        }
        public int Max
        {
            get; set;
        }
        public float Lat
        {
            get; set;
        }
        public float Long
        {
            get; set;
        }
        public double DistanceToStart
        {
            get; set;
        }
        public double DistanceToEvent
        {
            get; set;
        }
        public List<Cost> Costs
        {
            get; set;
        }
        public string Schedule
        {
            get; set;
        }
        public double Cost
        {
            get; set;
        }

        public int CompareTo(object p)
        {
            if (((Park)p).DistanceToStart + ((Park)p).DistanceToEvent == this.DistanceToStart + this.DistanceToEvent)
            {
                return 0;
            }
            else
            {
                return (((Park)p).DistanceToStart + ((Park)p).DistanceToEvent > this.DistanceToStart + this.DistanceToEvent) ? -1 : 1;
            }
        }
    }
}
