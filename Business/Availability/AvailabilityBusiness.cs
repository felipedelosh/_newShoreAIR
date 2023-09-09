using Microsoft.Extensions.Configuration;
using Models;
using Models.Contracts;
using System;
using System.Collections.Generic;

namespace Business.Availability
{
    public class AvailabilityBusiness : IAvailability
    {

        private readonly IConfiguration _iconfiguraion;

        public AvailabilityBusiness(IConfiguration iconfiguraion)
        {
            _iconfiguraion = iconfiguraion;
        }

        public List<Flight> ListFlights(ModelRequesFlights requestF)
        {
            List<Flight> list = new List<Flight>();

            try
            {
                var url = _iconfiguraion.GetSection("AppSettings").GetSection("urlAPIFlights").Value;

                Console.WriteLine(url);
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
