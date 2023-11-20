using Microsoft.AspNetCore.Mvc;

namespace AzureOpenAIProxy.ApiApp.Controllers;

[ApiController]
[Route("management")]
public class ManagementController : ControllerBase
{
    private readonly ILogger<ManagementController> _logger;

    public ManagementController(ILogger<ManagementController> logger)
    {
        _logger = logger;
    }

    [HttpGet("events", Name = "GetListOfEvents")]
    public async Task<IActionResult> GetEventsAsync()
    {
        var result = new OkObjectResult("Pong");

        return await Task.FromResult(result);
    }

    [HttpGet("events/{eventId}", Name = "GetEventById")]
    public async Task<IActionResult> GetEventByEventIdAsync([FromRoute] string eventId)
    {
        var result = new OkObjectResult("Pong");

        return await Task.FromResult(result);
    }

    [HttpGet("events/{eventId}/access-codes", Name = "GetListOfAccessCodesOfEvent")]
    public async Task<IActionResult> GetAccessCodesByEventIdAsync([FromRoute] string eventId)
    {
        var result = new OkObjectResult("Pong");

        return await Task.FromResult(result);
    }
}
