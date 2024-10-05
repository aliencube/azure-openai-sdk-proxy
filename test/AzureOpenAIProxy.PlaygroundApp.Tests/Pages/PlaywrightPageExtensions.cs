using Microsoft.Playwright;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

internal static class PlaywrightPageExtensions
{
    public static async Task DragToPoint(this IMouse mouse, ILocator source, float x, float y)
    {
        await source.HoverAsync();
        await mouse.DownAsync();
        await mouse.MoveAsync(x, y);
        await mouse.UpAsync();
    }
}
