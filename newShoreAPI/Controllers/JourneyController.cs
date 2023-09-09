using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Models.Contracts;
using Models;

namespace newShoreAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JourneyController : ControllerBase
    {

        private readonly ILogger<JourneyController> _logger;
        private readonly IAvailability _availabilityBusines;
        


        public JourneyController(ILogger<JourneyController> logger, IAvailability availabilityBusines) {
            _logger = logger;
            _availabilityBusines = availabilityBusines;
        }

        [HttpGet]
        [Route("Health")]
        public dynamic getHealth() {
            _logger.LogInformation("the user PING the API status");
            return getStandarJsonResponse("200", "v1.0", "[]");
        }

        [HttpGet]
        [Route("flightsv0")]
        public dynamic getflightsV0() {
            //Token auto ...
            var  request = new ModelRequesFlights() { Origin="x", Destination="z" };
            return _availabilityBusines.ListFlights(request);
        }



        private dynamic getStandarJsonResponse(string _statusCode, string _metadata, string _data) {
            return new
            {
                status = _statusCode,
                metadata = _metadata,
                data = _data
            };
        }

    }
}
