using Microsoft.AspNetCore.Mvc;
using nhmatsumoto.exception.middleware.demo.ExceptionMiddleware;

namespace nhmatsumoto.exception.middleware.demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            throw new ApiException(500, "Ocorreu uma exceção não tratada");
        }
    }
}
