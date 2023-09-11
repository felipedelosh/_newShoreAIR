using Microsoft.Extensions.Configuration;
using Models;
using Helper;
using Models.Contracts;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Models.Third;
using System.Linq.Expressions;
using Helper.RoutesCalculator;
using System.Xml.Linq;

namespace Business.Availability
{
    public class AvailabilityBusiness : IAvailability
    {

        private readonly IConfiguration _iconfiguraion;
        private readonly GetAPIData getAPIData;
        private Graph routes;
        private ShortestPathFinder shortestPathFinder;

        public AvailabilityBusiness(IConfiguration iconfiguraion)
        {
            _iconfiguraion = iconfiguraion;
            getAPIData = new GetAPIData();
            routes = new Graph();
            shortestPathFinder = new ShortestPathFinder(routes);
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
                var url = _iconfiguraion.GetSection("AppSettings").GetSection("urlAPIFlights").Value;
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

                var flightsResponse = JsonConvert.DeserializeObject<List<GetJsonFlightResponse>>(result);
                //Contruct Graph
                Console.WriteLine("==========Make Graph==========");
                //Reset routes
                routes = new Graph();
                foreach (var n in flightsResponse) {
                    var nodeA = n.DepartureStation;
                    var nodeB = n.ArrivalStation;
                    var price = n.Price;
                    routes.AddNode(nodeA);
                    routes.AddNode(nodeB);
                    //Save conection
                    routes.addEdge(nodeA, nodeB, price);
                }

                //Save the Graph in route calculator
                shortestPathFinder = new ShortestPathFinder(routes);


                //Save the response
                response = JsonConvert.SerializeObject(flightsResponse);

            }
            catch (Exception ex) {
                response = ex.Message;
            }

            return response;
        }


        public string getGraph()
        {
            return routes.getRoutes();
        }

        public Journey GetJourney(ModelRequesFlights requestF)
        {
            Journey response = new Journey();
            try
            {
                // Calculates a route
                string _origin = requestF.Origin;
                string _destination = requestF.Destination;
              
                List<string> shortestPath = shortestPathFinder.FindShortestPath(_origin, _destination);

                //Contruct a response
                var sizeOfResponse = shortestPath.Count;
                response.Oigin = _origin;
                response.Destination = _destination;
                formatJournetData(response, shortestPathFinder.isValidRoute, shortestPath);
            }
            catch {
                throw;
            }


            return response;
        }

        private void formatJournetData(Journey j, bool isValidRoute, List<string> data) {
            Console.WriteLine("=============CALCULATES A ROUTE=================");
            var sizeData = data.Count;

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
                    for (int i = 0; i < sizeData-1; i++) {
                        Console.WriteLine($"Estamos en: {data[i]} : vamos para {data[i+1]}");
                    }
                    j.Price = totalPrice;
                }
                
            }

            Console.WriteLine("=============END CALCULATES A ROUTE=================");
        }

    }
}
