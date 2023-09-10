using Microsoft.Extensions.Configuration;
using Models;
using Helper;
using Models.Contracts;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Models.Third;
using System.Linq.Expressions;

namespace Business.Availability
{
    public class AvailabilityBusiness : IAvailability
    {

        private readonly IConfiguration _iconfiguraion;
        private readonly GetAPIData getAPIData;

        public AvailabilityBusiness(IConfiguration iconfiguraion)
        {
            _iconfiguraion = iconfiguraion;
            getAPIData = new GetAPIData();
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






    }
}
