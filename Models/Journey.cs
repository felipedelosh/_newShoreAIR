using System.Collections.Generic;

namespace Models
{
    public class Journey
    {
        public List<Flight> Flights { get; set; }
        public string Message { get; set; } // To say things to user
        public string Oigin { get; set; }
        public string Destination { get; set; }
        public double Price { get; set; }

        public string currienceISO { get; set; }

    }
}
