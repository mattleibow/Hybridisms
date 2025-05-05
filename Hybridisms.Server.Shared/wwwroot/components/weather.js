
window.refreshWeatherTable = async function() {
    // fetch the weather data from the server
    const response = await fetch('/api/weather/forecasts');
    if (!response.ok) return;

    // parse the JSON response
    const data = await response.json();

    // find the table body
    const tbody = document.querySelector('#weatherTable tbody');
    if (!tbody) return;
    
    // clear the existing rows and add new data rows
    tbody.innerHTML = '';
    for (const forecast of data) {
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>${new Date(forecast.date).toLocaleDateString()}</td>
            <td>${forecast.temperatureC}</td>
            <td>${forecast.temperatureF}</td>
            <td>${forecast.summary ?? ''}</td>
        `;
        tbody.appendChild(row);
    }
}
