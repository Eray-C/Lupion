var payrollDetailGridCreated = false;

$(document).ready(function () {
    createPayrollPeriodsGrid();
    loadPayrollPeriods();
});

function loadPayrollPeriods() {
    genericAjax({
        url: "personnel/payroll/periods",
        isAuth: true,
        success: function (response) {
            var data = Array.isArray(response) ? response : (response.data != null ? response.data : []);
            var grid = $("#personnel_payroll_periods_grid").dxDataGrid("instance");
            if (grid) grid.option("dataSource", data);
        }
    });
}

function createPayrollPeriodsGrid() {
    var columns = [
        { dataField: "year", caption: "Yıl", dataType: "number", width: "7%" },
        { dataField: "month", caption: "Ay", dataType: "number", width: "7%", calculateCellValue: function (row) { return getMonthName(row.month); } },
        { dataField: "note", caption: "Not", width: "18%" },
        { dataField: "paymentNote", caption: "Ödeme Notu", allowEditing: false, allowWordWrap: true, width: "18%" },
        { dataField: "totalPaidAmount", caption: "Toplam Ödenen Tutar", dataType: "number", format: "#,##0.##", width: "12%" },
        { dataField: "isPaid", caption: "Ödendi mi", dataType: "boolean", width: "8%" },
        { dataField: "updatedByName", caption: "Son Güncelleyen", allowEditing: false, width: "15%" },
        { dataField: "updatedDate", caption: "Son Güncelleme Tarihi", dataType: "datetime", format: "dd.MM.yyyy HH:mm", width: "15%" }
    ];

    var contextMenu = function (rowData, gridInstance) {
        return [
            { text: "Ekle", icon: "plus", onItemClick: function () { openPayrollPeriodModal(null); } },
            { text: "Düzenle", icon: "edit", onItemClick: function () { openPayrollPeriodModal(rowData); } },
            {
                text: "Sil", icon: "trash", onItemClick: function () {
                    if (!rowData) return;
                    showDeleteAlert("personnel/payroll/period/" + rowData.year + "/" + rowData.month, function () { loadPayrollPeriods(); });
                }
            },
            { text: "Ödendi olarak işaretle", icon: "check", onItemClick: function () { payrollModalRealizeAll(rowData); } }
        ];
    };

    renderDatagrid(
        "personnel_payroll_periods_grid",
        [],
        columns,
        { showBorders: true, allowColumnResizing: true, columnAutoWidth: false },
        contextMenu,
        null,
        function (rowData) { openPayrollPeriodModal(rowData); }
    );
}

function getMonthName(month) {
    var names = ["", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"];
    return names[month] || month;
}

function exportPayrollPeriodsGrid(gridInstance) {
    var fileName = "Bordro_Donem_Listesi.xlsx";
    var workbook = new ExcelJS.Workbook();
    var worksheet = workbook.addWorksheet("Dönemler");
    DevExpress.excelExporter.exportDataGrid({
        component: gridInstance,
        worksheet: worksheet,
        autoFilterEnabled: true
    }).then(function () {
        workbook.xlsx.writeBuffer().then(function (buffer) {
            saveAs(new Blob([buffer], { type: "application/octet-stream" }), fileName);
        });
    });
}

function openPayrollPeriodModal(rowData) {
    showModal({
        url: "/personnel/payroll/period-modal",
        title: rowData ? "Bordro Düzenle" : "Bordro Ekle",
        size: "full",
        successEvent: function () {
            if (rowData) {
                $("#payrollPeriodForm #Year").val(rowData.year);
                $("#payrollPeriodForm #Month").val(rowData.month);
                $("#payrollPeriodForm #Note").val(rowData.note || "");
            } else {
                var now = new Date();
                $("#payrollPeriodForm #Year").val(now.getFullYear());
                $("#payrollPeriodForm #Month").val(now.getMonth() + 1);
            }
            window._payrollPeriodIsPaid = rowData ? (rowData.isPaid === true) : false;
            initPayrollDetailGridInModal();
            if (rowData) {
                loadPayrollDetailInModal(rowData.year, rowData.month);
            }
            var $modal = $(".modal.show").last();
            if ($modal.length) $modal.addClass("payroll-period-modal");
        }
    });
}

function initPayrollDetailGridInModal() {
    var $container = $("#payroll_detail_grid_container");
    $container.empty().append('<div id="payroll_detail_grid" style="height:280px"></div>');
    payrollDetailGridCreated = false;
    var columns = [
        { dataField: "personnelName", caption: "Personel", allowEditing: false, width: "12%" },
        { dataField: "netSalary", caption: "Net Maaş", dataType: "number", format: "#,##0.##", width: "10%" },
        { dataField: "mealCardFee", caption: "Yemek Kartı", dataType: "number", format: "#,##0.##", width: "9%" },
        { dataField: "travelExpense", caption: "Harcırah", dataType: "number", format: "#,##0.##", width: "9%" },
        { dataField: "totalBonus", caption: "Bonus", dataType: "number", format: "#,##0.##", allowEditing: false, width: "8%" },
        { dataField: "bonusDetails", caption: "Bonus Detay", allowEditing: false, allowWordWrap: true, width: "12%" },
        { dataField: "totalDeduction", caption: "Kesinti", dataType: "number", format: "#,##0.##", allowEditing: false, width: "8%" },
        { dataField: "deductionDetails", caption: "Kesinti Detay", allowEditing: false, allowWordWrap: true, width: "12%" },
        { dataField: "advanceDeduction", caption: "Avans Kesinti", dataType: "number", format: "#,##0.##", allowEditing: false, width: "8%" },
        { dataField: "advanceDetails", caption: "Avans Detay", allowEditing: false, allowWordWrap: true, width: "12%" },
        { dataField: "totalPayableAmount", caption: "Ödenecek Toplam", dataType: "number", format: "#,##0.##", allowEditing: false, width: "10%" }
    ];
    var extraSettings = {
        showBorders: true,
        allowColumnResizing: true,
        columnAutoWidth: false,
        editing: { allowUpdating: false },
        summary: {
            totalItems: [
                { column: "netSalary", summaryType: "sum", valueFormat: "#,##0.##", displayFormat: "Net Maaş: {0}" },
                { column: "mealCardFee", summaryType: "sum", valueFormat: "#,##0.##", displayFormat: "Yemek Kartı: {0}" },
                { column: "travelExpense", summaryType: "sum", valueFormat: "#,##0.##", displayFormat: "Harcırah: {0}" },
                { column: "totalBonus", summaryType: "sum", valueFormat: "#,##0.##", displayFormat: "Bonus: {0}" },
                { column: "totalDeduction", summaryType: "sum", valueFormat: "#,##0.##", displayFormat: "Kesinti: {0}" },
                { column: "advanceDeduction", summaryType: "sum", valueFormat: "#,##0.##", displayFormat: "Avans: {0}" },
                { column: "totalPayableAmount", summaryType: "sum", valueFormat: "#,##0.##", displayFormat: "TOPLAM: {0}" }
            ]
        },
        onRowUpdating: function (e) {
            var d = Object.assign({}, e.oldData, e.newData);
            var net = parseFloat(d.netSalary) || 0, mealCard = parseFloat(d.mealCardFee) || 0, travel = parseFloat(d.travelExpense) || 0;
            var bonus = parseFloat(d.totalBonus) || 0, deduction = parseFloat(d.totalDeduction) || 0, advance = parseFloat(d.advanceDeduction) || 0;
            e.newData.totalPayableAmount = net + mealCard + travel + bonus - deduction - advance;
        }
    };
    renderDatagrid("payroll_detail_grid", [], columns, extraSettings, null, null, null);
    payrollDetailGridCreated = true;
}

function loadPayrollDetailInModal(year, month) {
    genericAjax({
        url: "personnel/payroll?year=" + year + "&month=" + month,
        isAuth: true,
        success: function (response) {
            var data = Array.isArray(response) ? response : (response.data != null ? response.data : []);
            var grid = $("#payroll_detail_grid").dxDataGrid("instance");
            if (grid) grid.option("dataSource", data);
            setPayrollModalButtonState();
        }
    });
}

function payrollModalListele() {
    var year = parseInt($("#payrollPeriodForm #Year").val(), 10);
    var month = parseInt($("#payrollPeriodForm #Month").val(), 10);
    if (!year || !month) {
        toastr.warning("Yıl ve ay seçin.");
        return;
    }
    loadPayrollDetailInModal(year, month);
}

function setPayrollModalButtonState() {
    var grid = $("#payroll_detail_grid").dxDataGrid("instance");
    if (!grid) return;
    var data = grid.option("dataSource") || [];
    var hasApproved = data.length > 0 && data.some(function (r) { return r.id != null && r.id !== ""; });
    $("#btn_modal_soft_delete").prop("disabled", !hasApproved);
    $("#btn_modal_realize").prop("disabled", !hasApproved);
}

function payrollModalOnayla() {
    var year = parseInt($("#payrollPeriodForm #Year").val(), 10);
    var month = parseInt($("#payrollPeriodForm #Month").val(), 10);
    var note = $("#payrollPeriodForm #Note").val() || null;
    var grid = $("#payroll_detail_grid").dxDataGrid("instance");
    if (!grid) return;
    var dataSource = grid.option("dataSource") || [];
    if (dataSource.length === 0) {
        toastr.warning("Önce Listele ile veri yükleyin.");
        return;
    }
    var periodDate = new Date(year, month - 1, 1);
    var periodDateIso = periodDate.getFullYear() + "-" + String(periodDate.getMonth() + 1).padStart(2, "0") + "-" + String(periodDate.getDate()).padStart(2, "0") + "T00:00:00.000Z";
    var payload = {
        year: year,
        month: month,
        note: note,
        payrolls: dataSource.map(function (item) {
            var effDate = item.effectiveDate;
            var effectiveDateStr = periodDateIso;
            if (effDate != null && effDate !== "") {
                var d = new Date(effDate);
                if (!isNaN(d.getTime())) effectiveDateStr = d.getFullYear() + "-" + String(d.getMonth() + 1).padStart(2, "0") + "-" + String(d.getDate()).padStart(2, "0") + "T00:00:00.000Z";
            }
            return {
                id: item.id,
                personnelId: item.personnelId,
                personnelName: item.personnelName || "",
                effectiveDate: effectiveDateStr,
                baseSalary: toDecimal(item.baseSalary),
                calculatedNetSalary: toDecimal(item.calculatedNetSalary),
                netSalary: toDecimal(item.netSalary),
                mealCardFee: toDecimal(item.mealCardFee),
                travelExpense: toDecimal(item.travelExpense),
                totalPayableAmount: toDecimal(item.totalPayableAmount),
                totalBonus: toDecimal(item.totalBonus),
                bonusDetails: item.bonusDetails || "",
                totalDeduction: toDecimal(item.totalDeduction),
                deductionDetails: item.deductionDetails || "",
                advanceDeduction: toDecimal(item.advanceDeduction),
                advanceDetails: item.advanceDetails || "",
                provider: item.provider || "Kaydedilmiş Çalışma"
            };
        })
    };
    genericAjax({
        url: "personnel/payroll",
        method: "POST",
        data: payload,
        isAuth: true,
        success: function () {
            loadPayrollDetailInModal(year, month);
            setPayrollModalButtonState();
            loadPayrollPeriods();
        }
    });
}

function payrollModalSoftDeleteAndRelist() {
    var year = parseInt($("#payrollPeriodForm #Year").val(), 10);
    var month = parseInt($("#payrollPeriodForm #Month").val(), 10);
    if (!year || !month) return;
    genericAjax({
        url: "personnel/payroll/soft-delete-period?year=" + year + "&month=" + month,
        method: "POST",
        isAuth: true,
        success: function () {
            loadPayrollDetailInModal(year, month);
            loadPayrollPeriods();
            setPayrollModalButtonState();
        }
    });
}

function payrollModalRealizeAll(rowData) {
    if (rowData && rowData.isPaid === true) {
        toastr.warning("Bu dönem bordrosu zaten ödenmiş.");
        return;
    }
    var year, month;
    if (rowData && rowData.year != null && rowData.month != null) {
        year = parseInt(rowData.year, 10);
        month = parseInt(rowData.month, 10);
        window._payrollRealizePeriod = { year: year, month: month };
    } else {
        if (window._payrollPeriodIsPaid === true) {
            toastr.warning("Bu dönem bordrosu zaten ödenmiş.");
            return;
        }
        year = parseInt($("#payrollPeriodForm #Year").val(), 10);
        month = parseInt($("#payrollPeriodForm #Month").val(), 10);
        var grid = $("#payroll_detail_grid").dxDataGrid("instance");
        var data = grid ? (grid.option("dataSource") || []) : [];
        var hasApproved = data.length > 0 && data.some(function (r) { return r.id != null && r.id !== ""; });
        if (!hasApproved) {
            toastr.warning("Önce bordroyu onaylamanız gerekir.");
            return;
        }
        window._payrollRealizePeriod = { year: year, month: month };
    }
    showModal({
        url: "/personnel/payroll/realize-modal",
        title: "Ödendi Olarak İşaretle",
        size: "md",
        successEvent: function () {
            $("#RealizeYear").val(window._payrollRealizePeriod.year);
            $("#RealizeMonth").val(window._payrollRealizePeriod.month);
            var y = window._payrollRealizePeriod.year, m = window._payrollRealizePeriod.month;
            var lastDay = new Date(y, m, 0);
            var ys = lastDay.getFullYear(), ms = String(lastDay.getMonth() + 1).padStart(2, "0"), ds = String(lastDay.getDate()).padStart(2, "0");
            $("#RealizedDate").val(ys + "-" + ms + "-" + ds);
        }
    });
}

function submitRealizePayroll() {
    var year = parseInt($("#RealizeYear").val(), 10) || parseInt($("#payrollPeriodForm #Year").val(), 10);
    var month = parseInt($("#RealizeMonth").val(), 10) || parseInt($("#payrollPeriodForm #Month").val(), 10);
    var realizedDateStr = $("#RealizedDate").val();
    if (!realizedDateStr) {
        toastr.warning("Gerçekleşme tarihi girin.");
        return;
    }
    var payload = {
        year: year,
        month: month,
        realizedDate: realizedDateStr + "T00:00:00",
        notes: $("#Notes").val() || null
    };
    genericAjax({
        url: "personnel/payroll/realize-all",
        method: "POST",
        data: payload,
        isAuth: true,
        success: function () {
            var $modal = $(".modal.show").last();
            if ($modal.length && bootstrap.Modal.getInstance($modal[0])) bootstrap.Modal.getInstance($modal[0]).hide();
            loadPayrollPeriods();
            if ($("#payroll_detail_grid").length) {
                loadPayrollDetailInModal(year, month);
                setPayrollModalButtonState();
            }
        }
    });
}



function toDecimal(value) {
    var parsed = parseFloat(value);
    return isNaN(parsed) ? null : parsed;
}
