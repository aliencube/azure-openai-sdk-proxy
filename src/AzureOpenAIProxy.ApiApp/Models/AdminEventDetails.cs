namespace AzureOpenAIProxy.ApiApp.Models;

public class AdminEventDetails
{
    public string? EventId { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string? TimeZone { get; set; }
    public bool IsActive { get; set; }
    public string? OrganizerName { get; set; }
    public string? OrganizerEmail { get; set; }
    public string? CoorganizerName { get; set; }
    public string? CoorganizerEmail { get; set; }
    public int MaxTokenCap { get; set; }
    public int DailyRequestCap { get; set; }
}