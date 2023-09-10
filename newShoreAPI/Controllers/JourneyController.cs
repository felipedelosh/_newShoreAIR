using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Models.Contracts;
using Models;
using Helper;

namespace newShoreAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JourneyController : ControllerBase
    {

        private readonly ILogger<JourneyController> _logger;
        private readonly IAvailability _availabilityBusines;
        private readonly Authentication _authentication;



        public JourneyController(ILogger<JourneyController> logger, IAvailability availabilityBusines) {
            _logger = logger;
            _availabilityBusines = availabilityBusines;
            _authentication = new Authentication();
        }

        [HttpGet]
        [Route("Health")]
        public dynamic getHealth() {
            _logger.LogInformation("the user PING the API status");
            return getStandardJsonResponse("200", "v1.0", "[]");
        }

        [HttpGet()]
        [Route("Get")]
        public dynamic Get(string origin, string destination, string Authorization)
        {
            try {
                if (_authentication.isTokenValid(Authorization))
                {
                    return getStandardJsonResponse("200", "v1.0", Authorization + ": Token OK");
                }
                else {
                    _logger.LogInformation("the user insert invalid token");
                    return getStandardJsonResponse("401", "v1.0", Authorization + ": is Invalid token");
                }
            }
            catch (Exception ex) {
                _logger.LogError("Internal Server Error Get()");
                return getStandardJsonResponse("500", "v1.0", ex.ToString());
            }
            
        }


        [HttpGet]
        [Route("getFlightsV0")]
        public dynamic getFlightsV0() {
            try {
                var request = new ModelRequesFlights() { Origin = "x", Destination = "z" };
                return getStandardJsonResponse("200", "OK", _availabilityBusines.getFlightsV0().ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server Error getFlightsV0()");
                return getStandardJsonResponse("500", "ERROR", ex.ToString());
            }
        }




        [HttpGet]
        [Route("getFlightsV1")]
        public dynamic getFlightsV1()
        {
            try
            {
                var request = new ModelRequesFlights() { Origin = "x", Destination = "z" };
                return getStandardJsonResponse("200", "OK", _availabilityBusines.getFlightsV1().ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server Error getFlightsV0()");
                return getStandardJsonResponse("500", "ERROR", ex.ToString());
            }
        }


        [HttpGet]
        [Route("getFlightsV2")]
        public dynamic getFlightsV2()
        {
            try
            {
                var request = new ModelRequesFlights() { Origin = "x", Destination = "z" };
                return getStandardJsonResponse("200", "OK", _availabilityBusines.getFlightsV2().ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server Error getFlightsV0()");
                return getStandardJsonResponse("500", "ERROR", ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetGraph")]
        public dynamic GetGraph(string Authorization)
        {
            try
            {
                if (_authentication.isTokenValid(Authorization))
                {
                    string data = _availabilityBusines.getGraph();
                    return getStandardJsonResponse("200", "v1.0", data);
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


        private dynamic getStandardJsonResponse(string _statusCode, string _metadata, string _data) {
            return new
            {
                status = _statusCode,
                metadata = _metadata,
                data = _data
            };
        }

    }
}
