using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace SampleService.Controllers
{
    [Route("api/sample")]
    [ApiController]

    public class SampleController : ControllerBase
    {
        private readonly ILogger logger;

        public SampleController(ILogger<SampleController> logger)
        {
            this.logger = logger;
        }

        [HttpPost("echo")]
        public IActionResult Echo()
        {
            logger.LogTrace($"Just echoing some stuff (trace)");
            logger.LogDebug($"Just echoing some stuff (debug)");
            logger.LogInformation($"Just echoing some stuff (information)");
            logger.LogWarning($"Just echoing some stuff (warning)");
            logger.LogError(new NullReferenceException(), $"Just echoing some stuff (err)");
            logger.LogCritical(new NullReferenceException("dummy"), $"Just echoing some stuff (critical)");

            return StatusCode(StatusCodes.Status200OK);  
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
