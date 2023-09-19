using Microsoft.Extensions.Configuration;
using Models;
using Helper;
using Models.Contracts;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Models.Third;
using Helper.RoutesCalculator;
using Microsoft.Extensions.Logging;

namespace Business.Availability
{
    public class AvailabilityBusiness : IAvailability
    {

        private readonly ILogger<AvailabilityBusiness> _logger;
        private readonly IConfiguration _iconfiguraion;
        private readonly GetAPIData getAPIData;
        private Graph routes;
        private ShortestPathFinder shortestPathFinder;
        private CurriencesConverter curriencesConverter;

        public AvailabilityBusiness(ILogger<AvailabilityBusiness> logger, IConfiguration iconfiguraion)
        {
            _logger = logger;
            _iconfiguraion = iconfiguraion;
            getAPIData = new GetAPIData(); //Como le mando el logger?
            routes = new Graph();
            shortestPathFinder = new ShortestPathFinder(routes);
            //Update curriences
            string url = _iconfiguraion.GetSection("AppSettings").GetSection("applicationCurriences").Value;
            string updateCurriences = getAPIData.GetDicHTTPServiceCurriences(url);
            curriencesConverter = new CurriencesConverter(updateCurriences);
        }
        

        /// <summary>
        /// Return a simple list of all flights to show in a banner
        /// </summary>
        /// <returns></returns>
        public string getAllFlights()
        {
            string response = "";
            try
            {
                string data = getFlightsV0();
                List<GetJsonFlightResponse> flights = JsonConvert.DeserializeObject<List<GetJsonFlightResponse>>(data);

                foreach (var i in flights)
                {
                    response += $"Number:{i.FlightNumber}-(Origin:{i.DepartureStation}-Destination:{i.ArrivalStation})-Price:{i.Price}\n";
                }

            }
            catch (Exception ex) {
                _logger.LogError($"Error to get ALL flights routes {ex}");
            }
            return response;
        }

        public string getFlightsV0()
        {
            return GetResponseAPIvrX(0);
        }

        public string getFlightsV1()
        {
            return GetResponseAPIvrX(1);
        }

        public string getFlightsV2()
        {
            return GetResponseAPIvrX(2);
        }

        private string GetResponseAPIvrX(int v)
        {
            string response = "";
            try {
                string url = _iconfiguraion.GetSection("AppSettings").GetSection("urlAPIFlights").Value;
                var result = "";

                switch (v)
                {
                    case 0:
                        result = getAPIData.GetHTTPServiceVr0(url);
                        break;
                    case 1:
                        result = getAPIData.GetHTTPServiceVr1(url);
                        break;
                    case 2:
                        result = getAPIData.GetHTTPServiceVr2(url);
                        break;
                    default:
                        return "NO VALID OPTION";
                }

                List<GetJsonFlightResponse> flightsResponse = JsonConvert.DeserializeObject<List<GetJsonFlightResponse>>(result);
                //Contruct Graph
                //Reset routes
                routes = new Graph();
                foreach (var n in flightsResponse) {
                    string nodeA = n.DepartureStation;
                    string nodeB = n.ArrivalStation;
                    double price = n.Price;
                    routes.AddNode(nodeA);
                    routes.AddNode(nodeB);
                    //Save conection
                    routes.addEdge(nodeA, nodeB, price);
                }

                //Save the Graph in route calculator
                shortestPathFinder = new ShortestPathFinder(routes);


                //Save the response
                _logger.LogInformation("The user get DATA");
                response = JsonConvert.SerializeObject(flightsResponse);

            }
            catch (Exception ex) {
                _logger.LogError($"Error to call External API {ex.Message}");
                response = ex.Message;
            }

            return response;
        }

        public Journey GetJourney(ModelRequesFlights requestF, string Currience_selector)
        {
            Journey response = new Journey();
            try
            {
                // Calculates a route
                string _origin = requestF.Origin;
                string _destination = requestF.Destination;
              
                List<string> shortestPath = shortestPathFinder.FindShortestPath(_origin, _destination);

                //Contruct a response
                int sizeOfResponse = shortestPath.Count;
                response.Oigin = _origin;
                response.Destination = _destination;
                _logger.LogInformation("The user get journey information");
                formatJournetData(response, shortestPathFinder.isValidRoute, shortestPath, Currience_selector);
            }
            catch (Exception ex){
                _logger.LogError($"Error in GetJourney {ex}");
            }


            return response;
        }

        private void formatJournetData(Journey j, bool isValidRoute, List<string> data, string Currience_selector) {
            int sizeData = data.Count;

            if (sizeData == 0)
            {
                j.Message = "It´s imposible to calculate route";
            }
            else 
            {
                if (!isValidRoute)
                {
                    j.Message = $"Erros {sizeData} data:\n";
                    foreach (var i in data) {
                        j.Message += $"{i}\n";
                    }
                }
                else 
                {
                    j.Message = $"Find {sizeData} data:\n";
                    double totalPrice = 0;
                    List<Flight> flights = new List<Flight>();
                    for (int i = 0; i < sizeData-1; i++) {
                        //Search Fligths in cache
                        Flight f = searchFlightInCache(data[i], data[i+1]);
                        totalPrice += f.Price;
                        flights.Add(f);
                    }
                    j.Flights = flights;

                    double tempPrice = curriencesConverter.GetInConvertion(Currience_selector, totalPrice);

                    if (tempPrice >= 0)
                    {
                        j.Price = tempPrice;
                        j.Message += $"convertion price.\nReturn value in {Currience_selector}\n";
                    }
                    else {
                        j.Price = totalPrice;
                        j.Message += "Error in convertion price.\nReturn value in USD\n";
                    }
                   
                }
                
            }
        }

        /// <summary>
        /// Search a fly in cache and return Flight
        /// </summary>
        /// <param name="DepartureStation"></param>
        /// <param name="ArrivalStation"></param>
        /// <returns></returns>
        private Flight searchFlightInCache(string DepartureStation, string ArrivalStation) {
            Flight tempFlight = new Flight();

            try
            {
               
                string lastUrl = getAPIData.GetLastAPIUrl();
                string result = getAPIData.GetCacheDataByUrl(lastUrl);
                List<GetJsonFlightResponse> flightsResponse = JsonConvert.DeserializeObject<List<GetJsonFlightResponse>>(result);

                foreach (GetJsonFlightResponse i in flightsResponse) {
                    if (i.DepartureStation == DepartureStation && i.ArrivalStation == ArrivalStation) { 
                        //How to invoke a Transpor mapper?
                        //How to invoke a Journal mapper?
                        Transport tempTransport = new Transport();
                        // mappert.trasport(t.F, t.F)
                        // mapper.Journal(m.t, ...)
                        tempTransport.FlightCarrier = i.FlightCarrier;
                        tempTransport.FlightNumber  = i.FlightNumber;
                        tempFlight.Transport = tempTransport;
                        tempFlight.Origin = i.DepartureStation;
                        tempFlight.Destination = i.ArrivalStation;
                        tempFlight.Price = i.Price;
                    }   
                }

                _logger.LogInformation("The user get information via cache.");
            }
            catch (Exception ex) {
                _logger.LogError($"Error while the user search information in cache. {ex}");
      
            }
            return tempFlight;
        }

    }
}
