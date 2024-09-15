const apiKey = 'DHOML0QzBDaqhbhTBZa3wafUlN0M4781';

async function fetchData() {
    const symbol = document.getElementById('symbolInput').value;
    if (!symbol) {
        alert('Please enter a stock symbol');
        return;
    }

    try {
        const priceData = await fetchStockData(symbol);
        const smaData = await fetchIndicatorData(symbol, 'sma');
        const emaData = await fetchIndicatorData(symbol, 'ema');
        const wmaData = await fetchIndicatorData(symbol, 'wma');

        renderChart('priceChart', 'Stock Price', priceData);
        renderChart('smaChart', 'SMA', smaData);
        renderChart('emaChart', 'EMA', emaData);
        renderChart('wmaChart', 'WMA', wmaData);
    } catch (error) {
        console.error('Error fetching data:', error);
    }
}

async function fetchStockData(symbol) {
    const response = await axios.get(`https://financialmodelingprep.com/api/v3/historical-price-full/${symbol}?apikey=${apiKey}`);
    const data = response.data.historical.slice(0, 365);
    return data.map(entry => ({
        x: entry.date,
        y: entry.close
    }));
}

async function fetchIndicatorData(symbol, indicator) {
    const response = await axios.get(`https://financialmodelingprep.com/api/v3/technical_indicator/1day/${symbol}?type=${indicator}&period=10&apikey=${apiKey}`);
    const data = response.data.slice(0, 365);
    return data.map(entry => ({
        x: entry.date,
        y: entry[indicator]
    }));
}

function renderChart(canvasId, label, data) {
    const ctx = document.getElementById(canvasId).getContext('2d');
    const labels = data.map(entry => entry.x);
    const values = data.map(entry => entry.y);

    new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: label,
                data: values,
                borderColor: 'rgba(75, 192, 192, 1)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                fill: false,
            }]
        },
        options: {
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'Day'
                    },
                    title: {
                        display: true,
                        text: 'Full Date'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: 'Value'
                    }
                }
            }
        }
    });
}