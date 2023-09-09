using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    internal class Flight
    {
        public Transport transport { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public double price { get; set; }

    }
}
