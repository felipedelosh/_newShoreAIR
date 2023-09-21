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
using AutoMapper;
using System.Linq;

namespace Business.Availability
{
    public class AvailabilityBusiness : IAvailability
    {

        //Mapper
        private readonly IMapper _mapper;
        private readonly IMap<List<Flight>, List<GetJsonFlightResponse>> _map;

        private readonly ILogger<AvailabilityBusiness> _logger;
        private readonly IConfiguration _iconfiguraion;
        private readonly GetAPIData getAPIData;
        private RouteCalculator routeCalculator;
        private CurriencesConverter curriencesConverter;

        public AvailabilityBusiness(
            ILogger<AvailabilityBusiness> logger,
            IConfiguration iconfiguraion,
            IMapper mapper,
            IMap<List<Flight>, List<GetJsonFlightResponse>> map
            )
        {
            _mapper = mapper;
            _map = map;


            _logger = logger;
            _iconfiguraion = iconfiguraion;
            getAPIData = new GetAPIData(); //Como le mando el logger?
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
                //Get the API data
                string data = getFlightsV0();
                //Parse
                List<GetJsonFlightResponse> flightsResponse = JsonConvert.DeserializeObject<List<GetJsonFlightResponse>>(data);


                // Calculates a route via LINQ
                string _origin = requestF.Origin;
                string _destination = requestF.Destination;

                if (!(flightsResponse is null))
                {
                    IEnumerable<Flight> flights = new List<Flight>();
                    flights = _map.Map(flightsResponse);

                    //flights = flights.Where(x => x.Origin.Equals(_origin) && x.Destination.Equals(_destination));
                    routeCalculator = new RouteCalculator(_origin, _destination, flights);
                    response = routeCalculator.getRoute();

                    //Put PRICES in all Fligths
                    double totalPrice = 0;
                    double tempCurrienceConverter = 0;
                    foreach (Flight flight in response.Flights) {
                        tempCurrienceConverter = curriencesConverter.GetInConvertion(Currience_selector, flight.Price);
                        if (tempCurrienceConverter >= 0) {
                            flight.Price = tempCurrienceConverter;
                            totalPrice += flight.Price;
                        }
                    }


                    //Put total Prices
                    tempCurrienceConverter = curriencesConverter.GetInConvertion(Currience_selector, totalPrice);
                    if (tempCurrienceConverter >= 0)
                    {
                        response.Price = tempCurrienceConverter;
                        response.currienceISO = Currience_selector;
                    }
                    else {
                        response.Message += $"Error to GET price in {Currience_selector}";
                    }

                }
                else {
                    response.Message = $"Imposible to calculate External API returns NULL";
                }

          
                _logger.LogInformation("The user get journey information");
            }
            catch (Exception ex){
                _logger.LogError($"Error in GetJourney {ex}");
            }


            return response;
        }

    }
}
