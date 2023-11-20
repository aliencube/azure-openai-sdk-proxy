using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Controllers;

[ApiController]
[Route("ping")]
public class PingController : ControllerBase
{
    private readonly ILogger<PingController> _logger;

    public PingController(ILogger<PingController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "Ping")]
    public async Task<IActionResult> GetAsync()
    {
        var result = new OkObjectResult("Pong");

        return await Task.FromResult(result);
    }
}
