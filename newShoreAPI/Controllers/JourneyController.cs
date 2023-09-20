﻿using Helper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Contracts;
using Newtonsoft.Json;

namespace newShoreAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JourneyController : ControllerBase
    {

        private readonly ILogger<JourneyController> _logger;
        private readonly IAvailability _availabilityBusines;
        private readonly Authentication _authentication;



        public JourneyController(ILogger<JourneyController> logger, IAvailability availabilityBusines)
        {
            _logger = logger;
            _availabilityBusines = availabilityBusines;
            _authentication = new Authentication();
        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("Health")]
        public dynamic getHealth()
        {
            _logger.LogInformation("the user PING the API status");
            return getStandardJsonResponse("200", "v1.0", "the server is run :)");
        }

        [EnableCors("AllowOrigin")]
        [HttpGet()]
        [Route("Get")]
        public dynamic Get(string origin, string destination, string Authorization, string Currience_selector)
        {
            try
            {
                if (_authentication.isTokenValid(Authorization))
                {
                    ModelRequesFlights request = new ModelRequesFlights() { Origin = origin, Destination = destination };
                    Journey fligtsData = _availabilityBusines.GetJourney(request, Currience_selector);

                    //Export data to Json
                    string endData = JsonConvert.SerializeObject(fligtsData);
                    return getStandardJsonResponse("200", "v1.0", endData);
                }
                else
                {
                    _logger.LogInformation("the user insert invalid token");
                    return getStandardJsonResponse("401", "v1.0", Authorization + ": is Invalid token");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server Error Get()");
                return getStandardJsonResponse("500", "v1.0", ex.ToString());
            }

        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("getAllFlights")]
        public dynamic getAllFlights()
        {
            try
            {
                return getStandardJsonResponse("200", "OK", _availabilityBusines.getAllFlights());
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server Error getFlightsV0()");
                return getStandardJsonResponse("500", "ERROR", ex.ToString());
            }
        }


        private dynamic getStandardJsonResponse(string _statusCode, string _metadata, string _data)
        {
            return new
            {
                status = _statusCode,
                metadata = _metadata,
                data = _data
            };
        }

    }
}
