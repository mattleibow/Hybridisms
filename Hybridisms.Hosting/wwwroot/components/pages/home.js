
export function onLoad() {
    console.log('home.js Loaded');
}

export function onUpdate() {
    console.log('home.js Updated');
}

export function onDispose() {
    console.log('home.js Disposed');
}



//window.refreshNotesTable = async function() {
//    // fetch the notes data from the server
//    const response = await fetch('/api/notes');
//    if (!response.ok) return;

//    // parse the JSON response
//    const data = await response.json();

//    // find the table body
//    const tbody = document.querySelector('#notesTable tbody');
//    if (!tbody) return;
    
//    // clear the existing rows and add new data rows
//    tbody.innerHTML = '';
//    for (const note of data) {
//        const row = document.createElement('tr');
//        row.innerHTML = `
//            <td>${new Date(note.date).toLocaleDateString()}</td>
//            <td>${note.temperatureC}</td>
//            <td>${note.temperatureF}</td>
//            <td>${note.summary ?? ''}</td>
//        `;
//        tbody.appendChild(row);
//    }
//}
