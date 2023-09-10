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

        public AvailabilityBusiness(IConfiguration iconfiguraion)
        {
            _iconfiguraion = iconfiguraion;
            getAPIData = new GetAPIData();
            routes = new Graph();
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

                
                response = JsonConvert.SerializeObject(flightsResponse);

            }
            catch (Exception ex) {
                response = ex.Message;
            }

            return response;
        }

        public List<Flight> ListFlights(ModelRequesFlights requestF)
        {
            List<Flight> list = new List<Flight>();

            try
            {
                var url = _iconfiguraion.GetSection("AppSettings").GetSection("urlAPIFlights").Value;

                
            }
            catch (Exception ex)
            {
                //logger
                throw;
            }

            return list;
        }

        public string getGraph()
        {
            return routes.getRoutes();
        }
    }
}
