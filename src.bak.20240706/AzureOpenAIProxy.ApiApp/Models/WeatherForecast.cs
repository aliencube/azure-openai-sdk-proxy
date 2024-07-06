namespace AzureOpenAIProxy.ApiApp.Models;

/// <summary>
/// This represents the weather forecast record.
/// </summary>
/// <param name="Date"><see cref="DateOnly"/> value.</param>
/// <param name="TemperatureC">Temperature value in Celsius</param>
/// <param name="Summary">Weather summary value.</param>
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    /// <summary>
    /// Gets the temperature in Fahrenheit.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
