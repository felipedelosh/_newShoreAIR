using Models.Contracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Third;

namespace Business.Mapper
{
    public class Flight_Transport<P, T> : IMap<P, T>
    {
        P IMap<P, T>.Map(T origin)
        {
            var transport = new Transport();

            GetJsonFlightResponse flight = (GetJsonFlightResponse)Convert.ChangeType(origin, typeof(GetJsonFlightResponse));

            transport.FlightCarrier = flight.FlightCarrier;
            transport.FlightNumber = flight.FlightNumber;
            return (P)(object)transport;
        }
    }
}
