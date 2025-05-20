using Hybridisms.Shared.Services;
using Microsoft.JSInterop;

namespace Hybridisms.Client.NativeApp.Services;

public class JavaScriptNotesService(INotesService notesService)
{
    [JSInvokable("deleteNoteAsync")]
    public async Task DeleteNoteAsync(Guid noteId)
    {
        await notesService.DeleteNoteAsync(noteId);
    }
}
