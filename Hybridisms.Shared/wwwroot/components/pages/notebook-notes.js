
// Helper to show a Bootstrap modal confirm dialog
function showConfirmDialog(message) {
    const modalHtml =
`<div class="modal fade" id="hyb-confirm-modal" tabindex="-1" aria-labelledby="hybConfirmModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="hybConfirmModalLabel">Confirm</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <span id="hyb-confirm-message"></span>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" id="hyb-confirm-cancel">Cancel</button>
        <button type="button" class="btn btn-danger" id="hyb-confirm-ok">Delete</button>
      </div>
    </div>
  </div>
</div>`;

    return new Promise((resolve) => {

        // Create modal HTML if it doesn't exist
        let modal = document.getElementById('hyb-confirm-modal');
        if (!modal) {
            modal = document.createElement('div');
            modal.innerHTML = modalHtml;
            document.body.appendChild(modal);
        }

        // Set message
        document.getElementById('hyb-confirm-message').textContent = message;

        // Show modal
        const bsModal = new bootstrap.Modal(document.getElementById('hyb-confirm-modal'));
        bsModal.show();

        // Button handlers
        const okBtn = document.getElementById('hyb-confirm-ok');
        const cancelBtn = document.getElementById('hyb-confirm-cancel');

        function cleanup(result) {
            // Remove event listeners
            okBtn.removeEventListener('click', okHandler);
            cancelBtn.removeEventListener('click', cancelHandler);

            // Hide the modal
            bsModal.hide();

            // Resolve the promise with the result
            resolve(result);
        }

        function okHandler() {
            cleanup(true);
        }
        function cancelHandler() {
            cleanup(false);
        }

        // Add event listeners for buttons
        okBtn.addEventListener('click', okHandler);
        cancelBtn.addEventListener('click', cancelHandler);

        // Also handle modal close (X or backdrop)
        document.getElementById('hyb-confirm-modal')
            .addEventListener('hidden.bs.modal', cancelHandler, { once: true });
    });
}

async function deleteNote(noteId, notebookId) {
    // Check if the user really wants to delete the note
    const confirmed = await showConfirmDialog('Are you sure you want to delete this note?');
    if (!confirmed) {
        return;
    }

    var deleted = false;
    if (Hybridisms?.services?.notes) {
        // Directly call the .NET MAUI method to delete the note
        const result = await Hybridisms.services.notes.invokeMethodAsync('deleteNoteAsync', noteId);
        deleted = result;
    } else {
        // Send a DELETE request to the server to delete the note
        const response = await fetch(`/api/notes/${noteId}`, { method: 'DELETE' });
        deleted = response.ok;
    }

    if (deleted) {
        // Remove the note card from the DOM
        const card = document.getElementById(`note-card-${noteId}`);
        const container = card?.closest('.note-card-container');
        container?.remove();
    } else {
        console.error('Failed to delete note.');
    }
}

export function onLoad() {
    console.info('notebook-notes.js: onLoad');

    window.Hybridisms = window.Hybridisms || {};

    window.Hybridisms.deleteNote = deleteNote;
}

export function onUpdate() {
    console.info('notebook-notes.js: onUpdated');
}

export function onDispose() {
    console.info('notebook-notes.js: onDispose');

    delete window.Hybridisms.deleteNote;
}

