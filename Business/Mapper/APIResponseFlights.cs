using System;
using System.Collections.Generic;
using Models;
using Models.Contracts;
using Models.Third;

namespace Business.Mapper
{
    public class APIResponseFlights<P, T> : IMap<P, T>
    {
        private readonly IMap<Transport, GetJsonFlightResponse> _map;

        public APIResponseFlights(IMap<Transport, GetJsonFlightResponse> map) {
            _map = map;
        }

        P IMap<P, T>.Map(T origin)
        {
            List<Flight> flightResponse = new List<Flight>();

            List<GetJsonFlightResponse> flights = (List<GetJsonFlightResponse>)Convert.ChangeType(origin, typeof(List<GetJsonFlightResponse>));
            foreach (GetJsonFlightResponse flight in flights)
            {
                var flightAux = new Flight();
                flightAux.Origin = flight.DepartureStation;
                flightAux.Destination = flight.ArrivalStation;
                flightAux.Price = flight.Price;
                flightAux.Transport = _map.Map(flight);
                flightResponse.Add(flightAux);

            }

            return (P)(object)flightResponse;
        }
    }
}
