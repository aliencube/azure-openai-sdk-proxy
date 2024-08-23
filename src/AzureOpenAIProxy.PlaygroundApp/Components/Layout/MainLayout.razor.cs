using Microsoft.AspNetCore.Components;

namespace AzureOpenAIProxy.PlaygroundApp.Components.Layout;

/// <summary>
/// This represents the layout component for the main layout.
/// </summary>
public partial class MainLayout : LayoutComponentBase
{
    private bool _isAdmin;

    /// <summary>
    /// Injects the <see cref="NavigationManager"/> instance.
    /// </summary>
    [Inject]
    private NavigationManager? NavMan { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        var path = new Uri(this.NavMan!.Uri).AbsolutePath;
        this._isAdmin = path.StartsWith("/admin", StringComparison.CurrentCultureIgnoreCase);

        await Task.CompletedTask;
    }
}
