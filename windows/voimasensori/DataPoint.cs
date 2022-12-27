using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace voimasensori
{
    public class DataPoint
    {
        public double Value { get; set; }
        public long Seconds { get; set; }

        public DataPoint(double v)
        {
            this.Value = v;
            this.Seconds = DateTime.Now.Ticks;
        }

        public override string ToString()
        {
            return Seconds.ToString() + ";" + Value.ToString();
        }
    }
}
