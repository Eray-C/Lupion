$(document).ready(function () {
    loadDashboardSummary();
    loadTodaysPlannedShipments();
    loadExpiredVehicleDocuments();
    loadExpiredTrailerDocuments();
    loadMonthlyShipmentsChart();
    loadRecentShipments();
    loadShipmentsByStatesChart();
});

function formatDateTr(dateStr) {
    if (!dateStr) return '—';
    var d = new Date(dateStr);
    if (isNaN(d.getTime())) return '—';
    var day = String(d.getDate()).padStart(2, '0');
    var month = String(d.getMonth() + 1).padStart(2, '0');
    var year = d.getFullYear();
    return day + '.' + month + '.' + year;
}

function renderTodaysPlannedTable(data) {
    var tbody = $('#todaysPlannedBody');
    if (!tbody.length) return;
    tbody.empty();
    if (!data.length) return;
    data.forEach(function (item) {
        var lastExec = item.lastExecutedDate ? formatDateTr(item.lastExecutedDate) : 'Henüz gerçekleşmedi';
        var viewUrl = '/shipments?selectPlannedId=' + (item.id || '');
        var rowClass = (item.isDueTodayAndNotExecuted) ? 'table-danger' : '';
        var row = '<tr class="' + rowClass + '">' +
            '<td class="fw-semibold">' + (item.name ?? '—') + '</td>' +
            '<td>' + (item.customerName ?? '—') + '</td>' +
            '<td>' + lastExec + '</td>' +
            '<td>' + (item.materialTypeName ?? '—') + '</td>' +
            '<td class="text-end"><a href="' + viewUrl + '" target="_blank" class="btn btn-sm btn-outline-primary">İlgili kaydı gör</a></td>' +
            '</tr>';
        tbody.append(row);
    });
}

function loadExpiredVehicleDocuments() {
    genericAjax({
        url: 'vehicle-documents/expiring',
        isAuth: true,
        success: function (response) {
            var list = response?.data || [];
            var sub = document.getElementById('expiredVehicleDocsSubtitle');
            var empty = document.getElementById('expiredVehicleDocsEmpty');
            var gridWrap = document.getElementById('expiredVehicleDocsGridWrap');
            if (sub) sub.textContent = list.length > 0 ? list.length + ' belge süresi geçmiş veya 30 gün içinde bitiyor.' : '';
            if (empty) empty.classList.toggle('d-none', list.length > 0);
            if (gridWrap) gridWrap.classList.toggle('d-none', list.length === 0);
            renderExpiredVehicleDocsTable(Array.isArray(list) ? list : []);
        }
    });
}

function renderExpiredVehicleDocsTable(data) {
    var tbody = $('#expiredVehicleDocsBody');
    if (!tbody.length) return;
    tbody.empty();
    if (!data.length) return;
    data.forEach(function (item) {
        var days = item.remainingDays != null ? item.remainingDays : '-';
        var rowClass = days <= 0 ? 'table-danger' : (days <= 15 ? 'table-warning' : '');
        var expiryText = item.expiryDate ? formatDateTr(item.expiryDate) : '—';
        var row = '<tr class="' + rowClass + '">' +
            '<td class="fw-semibold">' + (item.vehiclePlate ?? '—') + '</td>' +
            '<td>' + (item.documentTypeName ?? '—') + '</td>' +
            '<td>' + (item.documentNumber ?? '—') + '</td>' +
            '<td>' + expiryText + '</td>' +
            '<td>' + (days !== '-' ? days + ' gün' : '—') + '</td>' +
            '</tr>';
        tbody.append(row);
    });
}

function loadExpiredTrailerDocuments() {
    genericAjax({
        url: 'trailer-documents/expiring',
        isAuth: true,
        success: function (response) {
            var list = response?.data || [];
            var sub = document.getElementById('expiredTrailerDocsSubtitle');
            var empty = document.getElementById('expiredTrailerDocsEmpty');
            var gridWrap = document.getElementById('expiredTrailerDocsGridWrap');
            if (sub) sub.textContent = list.length > 0 ? list.length + ' belge süresi geçmiş veya 30 gün içinde bitiyor.' : '';
            if (empty) empty.classList.toggle('d-none', list.length > 0);
            if (gridWrap) gridWrap.classList.toggle('d-none', list.length === 0);
            renderExpiredTrailerDocsTable(Array.isArray(list) ? list : []);
        }
    });
}

function renderExpiredTrailerDocsTable(data) {
    var tbody = $('#expiredTrailerDocsBody');
    if (!tbody.length) return;
    tbody.empty();
    if (!data.length) return;
    data.forEach(function (item) {
        var days = item.remainingDays != null ? item.remainingDays : '-';
        var rowClass = days <= 0 ? 'table-danger' : (days <= 15 ? 'table-warning' : '');
        var expiryText = item.expiryDate ? formatDateTr(item.expiryDate) : '—';
        var row = '<tr class="' + rowClass + '">' +
            '<td class="fw-semibold">' + (item.trailerPlate ?? '—') + '</td>' +
            '<td>' + (item.documentTypeName ?? '—') + '</td>' +
            '<td>' + (item.documentNumber ?? '—') + '</td>' +
            '<td>' + expiryText + '</td>' +
            '<td>' + (days !== '-' ? days + ' gün' : '—') + '</td>' +
            '</tr>';
        tbody.append(row);
    });
}

function loadTodaysPlannedShipments() {
    genericAjax({
        url: 'planned-shipments/todays',
        isAuth: true,
        success: function (response) {
            var list = response?.data || [];
            var sub = document.getElementById('todaysPlannedSubtitle');
            var empty = document.getElementById('todaysPlannedEmpty');
            var gridWrap = document.getElementById('todaysPlannedGridWrap');
            if (sub) sub.textContent = list.length > 0 ? 'Bugün ' + list.length + ' planlı sevkiyat var.' : '';
            if (empty) empty.classList.toggle('d-none', list.length > 0);
            if (gridWrap) gridWrap.classList.toggle('d-none', list.length === 0);
            renderTodaysPlannedTable(Array.isArray(list) ? list : []);
        }
    });
}

let materialTypeChartInstance = null;
let materialMonthGroups = [];

const numberFormatter = new Intl.NumberFormat('tr-TR');
const currencyFormatter = new Intl.NumberFormat('tr-TR', {
    style: 'currency',
    currency: 'TRY',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
});

function loadDashboardSummary() {
    genericAjax({
        url: 'dashboard/summary',
        isAuth: true,
        success: function (response) {
            const summary = response?.data;
            if (!summary) return;

            updateSummaryCard('#totalShipmentsValue', '#totalShipmentsChange', summary.totalShipments);
            updateSummaryCard('#totalRevenueValue', '#totalRevenueChange', summary.totalRevenue);
            updateSummaryCard('#totalPersonnelsValue', '#totalPersonnelChange', summary.totalPersonnels);
            updateSummaryCard('#totalCustomersValue', '#totalCustomersChange', summary.totalCustomers);
        }
    });
}

function updateSummaryCard(valueSelector, changeSelector, cardData) {
    if (!cardData) return;

    const valueElement = $(valueSelector);
    const changeElement = $(changeSelector);

    if (valueElement.length) {
        const formattedValue = cardData.valueFormatted ?? formatValue(cardData.value, cardData.format);
        valueElement.text(formattedValue);
    }

    if (!changeElement || !changeElement.length) return;

    changeElement.removeClass('text-success text-danger text-muted');

    const currentValue = cardData.value ?? 0;
    const difference = cardData.weeklyDifference ?? 0;
    const previousValue = currentValue - difference;

    let percentChange = 0;
    let text = "";

    if (previousValue === 0) {
        if (currentValue > 0) {
            percentChange = 100;
            changeElement.addClass('text-success');
            text = "Geçen haftaya göre %100 arttı";
        } else {
            changeElement.addClass('text-muted');
            text = "Geçen haftaya göre değişim yok";
        }
    } else {
        percentChange = (difference / previousValue) * 100;
        percentChange = Math.round(percentChange);

        if (percentChange > 0) {
            changeElement.addClass('text-success');
            text = `Geçen haftaya göre %${percentChange} arttı`;
        } else if (percentChange < 0) {
            changeElement.addClass('text-danger');
            text = `Geçen haftaya göre %${percentChange} azaldı`;
        } else {
            changeElement.addClass('text-muted');
            text = "Geçen haftaya göre değişim yok";
        }
    }

    changeElement.text(text);
}

function loadMonthlyShipmentsChart() {
    genericAjax({
        url: 'dashboard/monthly-material-type',
        isAuth: true,
        success: function (response) {
            const chartData = response?.data;
            const months = Array.isArray(chartData?.months) ? chartData.months : [];

            $('#materialChartSubtitle').text(chartData?.periodDescription ?? '');

            if (!hasAnyShipment(months)) {
                materialMonthGroups = [];

                toggleMaterialChartVisibility(false);
                return;
            }

            materialMonthGroups = months;
            toggleMaterialChartVisibility(true);
            renderMaterialTypeChart(months);
        }
    });
}

function hasAnyShipment(months) {
    if (!Array.isArray(months) || months.length === 0) {
        return false;
    }

    return months.some(month => Array.isArray(month.items) && month.items.some(item => (item.shipmentCount ?? 0) > 0));
}


function toggleMaterialChartVisibility(hasData) {
    const emptyState = $('#materialChartEmpty');
    const container = $('#materialChartContainer');

    if (!hasData) {
        emptyState.removeClass('d-none');
        container.addClass('d-none');
    } else {
        emptyState.addClass('d-none');
        container.removeClass('d-none');
    }
}

function renderMaterialTypeChart(monthGroups) {
    const ctx = document.getElementById('materialTypeChart');
    if (!ctx) return;

    const labels = monthGroups.map(group => group.monthName ?? '-');
    const materialMap = new Map();

    const ensureEntry = (typeName, monthLength) => {
        if (!materialMap.has(typeName)) {
            materialMap.set(typeName, {
                label: typeName,
                counts: new Array(monthLength).fill(0),
                totals: new Array(monthLength).fill(0)
            });
        }

        return materialMap.get(typeName);
    };

    monthGroups.forEach(group => {
        (group.items ?? []).forEach(item => {
            const typeName = item.materialType ?? 'Belirtilmemiş';
            ensureEntry(typeName, monthGroups.length);
        });
    });

    monthGroups.forEach((group, monthIndex) => {
        (group.items ?? []).forEach(item => {
            const typeName = item.materialType ?? 'Belirtilmemiş';
            const entry = ensureEntry(typeName, monthGroups.length);
            entry.counts[monthIndex] = item.shipmentCount ?? 0;
            entry.totals[monthIndex] = item.totalFreightPrice ?? 0;
        });
    });

    const palette = getMaterialPalette();
    const datasets = Array.from(materialMap.values()).map((entry, index) => ({
        label: entry.label,
        data: entry.counts,
        backgroundColor: palette[index % palette.length],
        borderRadius: 2,
        stack: 'shipments',
        monthTotals: entry.totals
    }));


    if (materialTypeChartInstance) {
        materialTypeChartInstance.destroy();
    }

    materialTypeChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels,
            datasets

        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                x: {
                    stacked: true
                },
                y: {
                    stacked: true,

                    beginAtZero: true,
                    ticks: {
                        precision: 0
                    }
                }
            },
            plugins: {
                legend: {
                    position: 'bottom'

                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            const dataset = context.dataset;
                            const shipmentCount = context.raw ?? 0;
                            const monthIndex = context.dataIndex;
                            const amount = Array.isArray(dataset.monthTotals)
                                ? dataset.monthTotals[monthIndex] ?? 0
                                : 0;
                            const countText = `${dataset.label}: ${numberFormatter.format(shipmentCount)} sevkiyat`;
                            return [countText, `Toplam Tutar: ${formatCurrency(amount)}`];
                        },
                        footer: function (tooltipItems) {
                            if (!Array.isArray(tooltipItems) || tooltipItems.length === 0) {
                                return '';
                            }

                            const monthIndex = tooltipItems[0].dataIndex;
                            const monthGroup = materialMonthGroups[monthIndex];
                            if (!monthGroup || !Array.isArray(monthGroup.items)) {
                                return '';
                            }

                            const total = monthGroup.items.reduce((sum, item) => sum + (item.shipmentCount ?? 0), 0);
                            return `Ay Toplamı: ${numberFormatter.format(total)} sevkiyat`;
                        }
                    }
                }
            }
        }
    });
}

function getMaterialPalette() {
    return [
        '#14abef',
        '#ff6a6a',
        '#6f42c1',
        '#2ecc71',
        '#f39c12',
        '#1abc9c',
        '#e67e22',
        '#3498db'
    ];
}


function loadRecentShipments() {
    genericAjax({
        url: 'dashboard/recent-shipments?count=6',
        isAuth: true,
        success: function (response) {
            const list = response?.data;
            renderRecentShipmentsTable(Array.isArray(list) ? list : []);
        }
    });
}

function renderRecentShipmentsTable(data) {
    const tbody = $('#recentShipmentsBody');
    if (!tbody.length) return;

    tbody.empty();

    if (!data.length) {
        tbody.append(`<tr><td colspan="6" class="text-center text-muted py-4">Son sevkiyat bulunamadı.</td></tr>`);
        return;
    }

    data.forEach(item => {
        const dateText = formatDate(item.shipmentDate);
        const statusBadge = createStatusBadge(item.statusName);
        const viewUrl = '/shipments?selectShipmentId=' + (item.id || '');
        const row = `
            <tr>
                <td class="fw-semibold">${item.shipmentNumber ?? '-'}</td>
                <td>${item.customerName ?? '-'}</td>
                <td>${statusBadge}</td>
                <td>${dateText}</td>
                <td>${item.vehiclePlate ?? '-'}</td>
                <td class="text-end"><a href="${viewUrl}" target="_blank" class="btn btn-sm btn-outline-primary">İlgili kaydı gör</a></td>
            </tr>`;

        tbody.append(row);
    });
}

function createStatusBadge(status) {
    if (!status) {
        return '<span class="badge bg-secondary">Belirsiz</span>';
    }

    const mappings = {
        'Planlandı': 'bg-warning text-dark',
        'Yolda': 'bg-info text-dark',
        'Teslim Edildi': 'bg-success',
        'Tamamlandı': 'bg-success',
        'İptal Edildi': 'bg-danger'
    };

    const classes = mappings[status] || 'bg-primary';
    return `<span class="badge ${classes}">${status}</span>`;
}

function formatDate(value) {
    if (!value) return '-';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '-';
    return date.toLocaleDateString('tr-TR');
}

function formatCurrency(value) {
    return currencyFormatter.format(value ?? 0);
}

function formatValue(value, format) {
    if (format === 'currency') {
        return formatCurrency(value);
    }

    return numberFormatter.format(value ?? 0);
}



function loadShipmentsByStatesChart() {
    genericAjax({
        url: 'dashboard/shipments-by-city',
        isAuth: true,
        success: function (response) {
            createShipmentsByStatesChart(response?.data || []);
            renderCityProgress(response?.data || []);
        }
    });
}

function createShipmentsByStatesChart(items) {
    (async () => {
        const topology = await fetch('/maps/tr-all.topo.json')
            .then(response => response.json());

        const chartData = items.map(x => [x.cityCode, x.shipmentCount]);

        Highcharts.mapChart('turkeyMap', {
            chart: { map: topology },
            title: { text: 'Şehirler' },
            mapNavigation: { enabled: false },
            colorAxis: { min: 0 },
            series: [{
                data: chartData,
                name: 'Sevkiyat sayısı',
                states: { hover: { color: '#BADA55' } },
                dataLabels: { enabled: true, format: '{point.name}' }
            }]
        });
    })();
}
function renderCityProgress(data) {
    let container = $("#cityProgressList");
    container.empty();

    const total = data.reduce((sum, x) => sum + x.shipmentCount, 0);

    const top5 = data.sort((a, b) => b.shipmentCount - a.shipmentCount).slice(0, 6);

    top5.forEach((item, index) => {
        const percent = total > 0 ? Math.round((item.shipmentCount / total) * 100) : 0;

        const colors = ["primary", "danger", "success", "warning", "info"];
        const color = colors[index % colors.length];

        const html = `
            <div class="mb-3">
                <div class="d-flex justify-content-between">
                    <span>${item.cityName}</span><span>${percent}%</span>
                </div>
                <div class="progress">
                    <div class="progress-bar bg-${color}" role="progressbar" 
                         style="width:${percent}%">
                    </div>
                </div>
            </div>
        `;
        container.append(html);
    });
}
