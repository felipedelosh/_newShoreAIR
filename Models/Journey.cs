using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Journey
    {
        public List<Flight> Flights { get; set; }   
        public string Oigin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }

    }
}
