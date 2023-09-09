using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace newShoreAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class JourneyController : ControllerBase
    {

        private readonly ILogger<JourneyController> _logger;

        public JourneyController(ILogger<JourneyController> logger) {
            _logger = logger;
        }

        [HttpGet]
        [Route("Health")]
        public dynamic getHealth() {
            _logger.LogInformation("the user PING the API status");
            return getStandarJsonResponse("200", "v1.0", "[]");
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
