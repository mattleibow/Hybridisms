using Hybridisms.Client.NativeApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Hybridisms.Client.NativeApp;

public class JavaScriptServiceProvider : ComponentBase, IDisposable
{
    private readonly ICollection<IDisposable> dotnetReferences = [];

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private JavaScriptNotesService NotesService { get; set; } = default!;


    protected override async Task OnInitializedAsync()
    {
        await RegisterService("notes", NotesService);
    }

    private async Task RegisterService(string name, object service)
    {
        var reference = DotNetObjectReference.Create(service);
        dotnetReferences.Add(reference);
        await JSRuntime.InvokeVoidAsync("Hybridisms.setService", [name, reference]);
    }

    public void Dispose()
    {
        foreach (var reference in dotnetReferences)
        {
            reference.Dispose();
        }
        dotnetReferences.Clear();
    }
}
