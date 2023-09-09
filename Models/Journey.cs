using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    internal class Journey
    {
        public List<Flight> Flights { get; set; }   
        public string origin { get; set; }
        public string destination { get; set; }
        public double price { get; set; }

    }
}
