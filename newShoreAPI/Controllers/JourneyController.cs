using Helper;
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
        public dynamic GetHealth()
        {
            _logger.LogInformation("the user PING the API status");
            return GetStandardJsonResponse("200", "v1.0", "the server is run :)");
        }

        [EnableCors("AllowOrigin")]
        [HttpGet()]
        [Route("Get")]
        public dynamic Get(string origin, string destination, string Authorization, string Currience_selector)
        {
            try
            {
                if (_authentication.IsTokenValid(Authorization))
                {
                    ModelRequesFlights request = new ModelRequesFlights() { Origin = origin, Destination = destination };
                    Journey fligtsData = _availabilityBusines.GetJourney(request, Currience_selector);

                    //Export data to Json
                    string endData = JsonConvert.SerializeObject(fligtsData);
                    return GetStandardJsonResponse("200", "v1.0", endData);
                }
                else
                {
                    _logger.LogInformation("the user insert invalid token");
                    return GetStandardJsonResponse("401", "v1.0", Authorization + ": is Invalid token");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server Error Get()");
                return GetStandardJsonResponse("500", "v1.0", ex.ToString());
            }

        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        [Route("getAllFlights")]
        public dynamic GetAllFlights()
        {
            try
            {
                return GetStandardJsonResponse("200", "OK", _availabilityBusines.GetAllFlights());
            }
            catch (Exception ex)
            {
                _logger.LogError("Internal Server Error getFlightsV0()");
                return GetStandardJsonResponse("500", "ERROR", ex.ToString());
            }
        }


        private dynamic GetStandardJsonResponse(string _statusCode, string _metadata, string _data)
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
