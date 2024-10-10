using Microsoft.Playwright;

namespace AzureOpenAIProxy.PlaygroundApp.Tests.Pages;

internal static class PlaywrightPageExtensions
{
    public static async Task DragElementToPoint(this IMouse mouse, ILocator source, float x, float y)
    {
        await source.HoverAsync();
        await mouse.DownAsync();

        // Double execution for reliable mouse move https://playwright.dev/docs/input
        await mouse.MoveAsync(x, y);
        await mouse.MoveAsync(x, y);
        
        await mouse.UpAsync();
    }
}