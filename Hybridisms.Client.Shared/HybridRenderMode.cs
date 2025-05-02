using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Hybridisms.Client.Shared;

public static class HybridRenderMode
{
    public static IComponentRenderMode? InteractiveServer { get; } = IfSupported(RenderMode.InteractiveServer);

    public static IComponentRenderMode? InteractiveAuto { get; } = IfSupported(RenderMode.InteractiveAuto);

    public static IComponentRenderMode? InteractiveWebAssembly { get; } = IfSupported(RenderMode.InteractiveWebAssembly);

    private static IComponentRenderMode? IfSupported(IComponentRenderMode? mode) =>
        !AppContext.TryGetSwitch("Hybridisms.SupportsRenderMode", out var isEnabled) || isEnabled ? mode : null;
}
