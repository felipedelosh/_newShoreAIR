using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Models.Contracts;
using Models.Third;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Business.Availability
{
    public class AvailabilityBusiness : IAvailability
    {

        private readonly IMapper _mapper;
        private readonly IMap<List<Flight>, List<GetJsonFlightResponse>> _map;
        private readonly ILogger<AvailabilityBusiness> _logger;
        private readonly IConfiguration _iconfiguraion;
        private readonly ICurriencesConverter _curriencesConverter;
        private readonly IRouteCalculator _routeCalculator;
        private readonly IGetAPIData _getAPIData;

        public AvailabilityBusiness(
            ILogger<AvailabilityBusiness> logger,
            IConfiguration iconfiguraion,
            ICurriencesConverter curriencesConverter,
            IMapper mapper,
            IMap<List<Flight>, List<GetJsonFlightResponse>> map,
            IRouteCalculator routeCalculator,
            IGetAPIData getAPIData
            )
        {
            _mapper = mapper;
            _map = map;

            _logger = logger;
            _iconfiguraion = iconfiguraion;

            _curriencesConverter = curriencesConverter;

            _getAPIData = getAPIData;

            //Update curriences
            string url = _iconfiguraion.GetSection("AppSettings").GetSection("applicationCurriences").Value;
            string updateCurriencesData = getAPIData.GetDicHTTPServiceCurriences(url);
            _curriencesConverter.UpdateCurriences(updateCurriencesData);

            _routeCalculator = routeCalculator;
        }


        /// <summary>
        /// Return a simple list of all flights to show in a banner
        /// </summary>
        /// <returns></returns>
        public string GetAllFlights()
        {
            string response = "";
            try
            {
                string data = GetFlightsV0();
                List<GetJsonFlightResponse> flights = JsonConvert.DeserializeObject<List<GetJsonFlightResponse>>(data);

                foreach (var i in flights)
                {
                    response += $"Number:{i.FlightNumber}-(Origin:{i.DepartureStation}-Destination:{i.ArrivalStation})-Price:{i.Price}\n";
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error to get ALL flights routes {ex}");
            }
            return response;
        }

        public string GetFlightsV0()
        {
            return GetResponseAPIvrX(0);
        }

        public string GetFlightsV1()
        {
            return GetResponseAPIvrX(1);
        }

        public string GetFlightsV2()
        {
            return GetResponseAPIvrX(2);
        }

        private string GetResponseAPIvrX(int v)
        {
            string response = "";
            try
            {
                string url = _iconfiguraion.GetSection("AppSettings").GetSection("urlAPIFlights").Value;
                var result = "";

                switch (v)
                {
                    case 0:
                        result = _getAPIData.GetHTTPServiceVr0(url);
                        break;
                    case 1:
                        result = _getAPIData.GetHTTPServiceVr1(url);
                        break;
                    case 2:
                        result = _getAPIData.GetHTTPServiceVr2(url);
                        break;
                    default:
                        return "NO VALID OPTION";
                }

                List<GetJsonFlightResponse> flightsResponse = JsonConvert.DeserializeObject<List<GetJsonFlightResponse>>(result);

                //Save the response
                _logger.LogInformation("The user get DATA");
                response = JsonConvert.SerializeObject(flightsResponse);

            }
            catch (Exception ex)
            {
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
                string data = GetFlightsV0();
                //Parse
                List<GetJsonFlightResponse> flightsResponse = JsonConvert.DeserializeObject<List<GetJsonFlightResponse>>(data);


                // Calculates a route via LINQ
                string _origin = requestF.Origin;
                string _destination = requestF.Destination;

                if (!(flightsResponse is null))
                {
                    IEnumerable<Flight> flights = new List<Flight>();
                    flights = _map.Map(flightsResponse);

                    response = _routeCalculator.GetRoute(_origin, _destination, flights);

                    //Put PRICES in all Fligths
                    if (!(response.Flights is null))
                    {
                        double totalConvertPrice = 0; // Save all prices when the conversion is done
                        double totalOriginalPrice = 0; // Save all prices original API Json
                        double tempCurrienceConverter = 0; // temporal to convertion Flights prices
                        foreach (Flight flight in response.Flights)
                        {
                            totalOriginalPrice += flight.Price;
                            tempCurrienceConverter = _curriencesConverter.GetInConvertion(Currience_selector, flight.Price);
                            if (tempCurrienceConverter >= 0)
                            {
                                flight.Price = tempCurrienceConverter;
                                totalConvertPrice += flight.Price;
                            }
                        }


                        //Put total Prices
                        tempCurrienceConverter = _curriencesConverter.GetInConvertion(Currience_selector, totalOriginalPrice);
                        if (tempCurrienceConverter >= 0)
                        {
                            response.Price = tempCurrienceConverter;
                            response.CurrienceISO = Currience_selector;
                            response.Message += $".The price is a convertion: {totalConvertPrice}{Currience_selector}";
                        }
                        else
                        {
                            response.Message += $".Error to GET price in {Currience_selector}";
                            response.Price = totalOriginalPrice;
                            response.CurrienceISO = "USD";

                        }
                    }
                }
                else
                {
                    response.Message = $"Imposible to calculate External API returns NULL";
                }


                _logger.LogInformation("The user get journey information");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetJourney {ex}");
            }


            return response;
        }

    }
}
