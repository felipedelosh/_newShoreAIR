using Autofac;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Models.Contracts;

namespace Helper.RoutesCalculator
{
    public class RouteCalculator : IRouteCalculator
    {
        private string _origin;
        private string _destination;
        private IEnumerable<Flight> _ArrFlights;
        private int LenOfArr;
        private List<string> ControlDestinations;
        private readonly ILogger<RouteCalculator> _logger;



        public RouteCalculator(ILogger<RouteCalculator> logger) {
            _logger = logger;
        }

        /// <summary>
        /// CALLED in auto by GetRoute
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="flights"></param>
        public void InitRouteCalculator(string origin, string destination, IEnumerable<Flight> flights) {
            _ArrFlights = flights;
            _origin = origin;
            _destination = destination;
            ControlDestinations = new List<string>();
            LenOfArr = 0;
            foreach (Flight flight in _ArrFlights)
            {
                if (!ControlDestinations.Contains(flight.Origin))
                {
                    ControlDestinations.Add(flight.Origin);
                }
                
                if (!ControlDestinations.Contains(flight.Destination))
                {
                    ControlDestinations.Add(flight.Destination);
                }
                LenOfArr++;
            }

        }

        /// <summary>
        /// Calculate a route Flight A to Flight B
        /// You send the params when instance the class(o, d, f[])
        /// </summary>
        /// <returns>Journey</returns>
        public Journey GetRoute(string origin, string destination, IEnumerable<Flight> flights) { 
            InitRouteCalculator(origin, destination, flights);
            Journey journey = new Journey();

            try
            {
                journey.Oigin = _origin;
                journey.Destination = _destination;

                //The user inser invalid site?
                if (!ControlDestinations.Contains(journey.Oigin) || !ControlDestinations.Contains(journey.Destination))
                {
                    if (!ControlDestinations.Contains(journey.Oigin)) {
                        journey.Message = $"Error in Origin NOT EXITST {journey.Oigin}";
                    }

                    if (!ControlDestinations.Contains(journey.Destination)) {
                        journey.Message += $"Error in Destination NOT EXITST {journey.Destination}";
                    }
                    return journey;
                }

                //The user insert equals route?
                if (journey.Oigin == journey.Destination) {
                    journey.Message = $"Error in Origin and destination be the same {journey.Oigin} = {journey.Destination}";
                    return journey;
                }


                List<Flight> route = new List<Flight>();
                FindRouteRecursive(journey.Oigin, journey.Destination, route);
                journey.Flights = route;
                journey.Message = $"Total Flights : {journey.Flights.Count}";
                _logger.LogInformation("The user get journey route information");

            }
            catch (Exception ex){
                _logger.LogError($"Error in route of Journey {ex}");
                journey.Message = "Fatal Error To calculate IT";
            }


            return journey;
        }

        private bool FindRouteRecursive(string currentStation, string destination, List<Flight> route)
        {
            if (currentStation == destination)
            {
                return true;
            }

            var nextFlight = _ArrFlights
                .Where(f => f.Origin == currentStation)
                .OrderBy(f => f.Price)
                .FirstOrDefault();

            if (nextFlight != null)
            {
                route.Add(nextFlight);
                if (FindRouteRecursive(nextFlight.Destination, destination, route))
                {
                    return true;
                }
                route.Remove(nextFlight);
            }

            return false;
        }

    }
}
