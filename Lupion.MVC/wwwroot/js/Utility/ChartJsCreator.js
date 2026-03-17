function createSimpleChart({
    canvasId,
    type = 'bar',
    labels = [],
    data = [],
    label = '',
    color = null,
    showLegend = true
}) {
    const ctx = document.getElementById(canvasId).getContext('2d');

    if (window[canvasId + "_instance"]) {
        window[canvasId + "_instance"].destroy();
    }

    const defaultColors = [
        'rgb(255, 99, 132)',   // kırmızı
        'rgb(54, 162, 235)',   // mavi
        'rgb(255, 206, 86)',   // sarı
        'rgb(75, 192, 192)',   // turkuaz
        'rgb(153, 102, 255)',  // mor
        'rgb(255, 159, 64)',   // turuncu
        'rgb(0, 200, 83)',     // yeşil
        'rgb(255, 87, 34)',    // canlı turuncu
        'rgb(63, 81, 181)',    // lacivert
        'rgb(233, 30, 99)'     // pembe
    ];

    const backgroundColors = data.map((_, i) => color
        ? Array.isArray(color) ? color[i % color.length] : color
        : defaultColors[i % defaultColors.length]);

    const borderColors = backgroundColors;

    window[canvasId + "_instance"] = new Chart(ctx, {
        type: type,
        data: {
            labels: labels,
            datasets: [{
                label: label,
                data: data,
                backgroundColor: backgroundColors,
                borderColor: borderColors,
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: showLegend,
                    position: 'bottom',
                    labels: {
                        boxHeight: 10,
                        boxWidth: 20,
                        generateLabels: function (chart) {
                            const original = Chart.overrides[chart.config.type].plugins.legend.labels.generateLabels;
                            let labels = original.call(this, chart);

                            const data = chart.data.datasets[0].data;

                            labels.sort((a, b) => {
                                return data[b.index] - data[a.index];
                            });

                            labels = labels.slice(0, 3);

                            return labels;
                        }
                    }
                }
            },
            scales: type === 'bar' || type === 'line' ? {
                y: {
                    beginAtZero: true
                }
            } : {}
        }
    });
}
