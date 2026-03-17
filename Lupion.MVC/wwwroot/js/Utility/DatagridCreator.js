function renderDatagrid(gridId, dataSource, columns, ExtraSettings = {}, customContextMenuFn = null, rowClickFn = null, rowDoubleClickFn = null) {
    var defaultContextMenuItems = function (rowData, gridInstance) {
        return [
            { text: 'Datagrid İşlemleri', beginGroup: true, disabled: true },
            {
                text: "Filtreleri Temizle",
                icon: "filter",
                onItemClick: function () {
                    gridInstance.clearFilter();
                }
            },
            {
                text: "Grupları aç / kapa",
                icon: "indent",
                onItemClick: function () {
                    const current = gridInstance.option("grouping.autoExpandAll");
                    gridInstance.option("grouping.autoExpandAll", !current);
                }
            }
        ];
    };

    var settings = {
        dataSource: dataSource,
        columns: columns,
        showBorders: true,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        allowColumnReordering: true,
        allowColumnResizing: true,
        columnAutoWidth: true,
        selection: { mode: "single" },
        groupPanel: { visible: true, allowColumnDragging: true },
        grouping: { autoExpandAll: false },
        filterRow: { visible: true, applyFilter: "auto" },
        headerFilter: { visible: true },
        columnChooser: { enabled: true, mode: "select" },
        export: {
            enabled: true,
            fileName: "Rapor",
            allowExportSelectedData: false,
            autoExpandAll: false
        },
        paging: { pageSize: 15 }, // default
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [15, 25, 50, 100],
            showInfo: true,
            /*visible: true*/
            visible: "auto" 
        },
        onContentReady: function (e) {
            //setTimeout(() => {
            //    adjustPageSize(e.component, gridId);
            //}, 100);
        },
        onContextMenuPreparing: function (e) {
            const gridInstance = e.component;
            const defaultItems = defaultContextMenuItems(e.row ? e.row.data : null, gridInstance);
            const customItems = customContextMenuFn ? customContextMenuFn(e.row ? e.row.data : null, gridInstance) : [];
            e.items = [...customItems, ...defaultItems];
        },
        onExporting: function (e) {
            const workbook = new ExcelJS.Workbook();
            const worksheet = workbook.addWorksheet('Sheet1');
            DevExpress.excelExporter.exportDataGrid({
                component: e.component,
                worksheet: worksheet,
                autoFilterEnabled: true
            }).then(function () {
                workbook.xlsx.writeBuffer().then(function (buffer) {
                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), e.fileName + '.xlsx');
                });
            });
            e.cancel = true;
        },
        onRowClick: function (e) {
            if (rowClickFn) rowClickFn(e.data, e.component);
        },
        onRowDblClick: function (e) {
            if (rowDoubleClickFn) rowDoubleClickFn(e.data, e.component);
        },
        ...ExtraSettings
    };

    $(`#${gridId}`).on("contextmenu", function (e) {
        e.preventDefault();
    });

    return $(`#${gridId}`).dxDataGrid(settings).dxDataGrid('instance');
}
function renderDatagridDynamic(
    gridId,
    options
) {
    const {
        key,
        url,
        take = 50,
        getBaseParams,
        columns,
        extraSettings = {},
        customContextMenuFn = null,
        rowClickFn = null,
        rowDoubleClickFn = null
    } = options;

    const dataSource = new DevExpress.data.CustomStore({
        key: key,
        load: function (loadOptions) {

            const baseParams = getBaseParams ? getBaseParams() : null;

            if (!baseParams) {
                return Promise.resolve({ data: [], totalCount: 0 });
            }

            const sort = loadOptions.sort?.length ? loadOptions.sort[0] : null;

            const params = new URLSearchParams({
                ...baseParams,
                skip: loadOptions.skip ?? 0,
                take: loadOptions.take ?? take,
                search: loadOptions.searchValue ?? "",
                sortField: sort?.selector ?? "",
                sortOrder: sort?.desc ? "desc" : "asc"
            });

            return new Promise((resolve, reject) => {
                genericAjax({
                    url: `${url}?${params}`,
                    method: "GET",
                    success: function (response) {
                        const payload = response.data ?? response;
                        resolve({
                            data: payload.data ?? payload.Data ?? [],
                            totalCount: payload.totalCount ?? payload.TotalCount ?? 0
                        });
                    },
                    error: reject
                });
            });
        }
    });

    const settings = {
        dataSource: dataSource,
        columns: columns,
        showBorders: true,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        allowColumnReordering: true,
        allowColumnResizing: true,
        columnAutoWidth: true,
        selection: { mode: "single" },
        scrolling: { mode: "virtual" },
        remoteOperations: { paging: true, sorting: true, filtering: true, search: true },
        searchPanel: { visible: true },
        paging: { pageSize: take },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [25, 50, 100],
            showInfo: true,
            visible: "auto"
        },
        onContextMenuPreparing: function (e) {
            //if (!e.row || e.row.rowType !== "data") return;
            const grid = e.component;
            const customItems = customContextMenuFn
                ? customContextMenuFn(e.row.data, grid)
                : [];
            e.items = customItems;
        },
        onRowClick: function (e) {
            if (rowClickFn) rowClickFn(e.data, e.component);
        },
        onRowDblClick: function (e) {
            if (rowDoubleClickFn) rowDoubleClickFn(e.data, e.component);
        },
        ...extraSettings
    };

    $(`#${gridId}`).on("contextmenu", function (e) {
        e.preventDefault();
    });

    return $(`#${gridId}`).dxDataGrid(settings).dxDataGrid("instance");
}

function adjustPageSize(gridInstance, gridId) {
    const gridElement = document.getElementById(gridId);
    if (!gridElement) return;

    const gridHeight = gridElement.clientHeight;
    const headerHeight = gridElement.querySelector(".dx-datagrid-headers").offsetHeight || 50;
    const pagerHeight = gridElement.querySelector(".dx-datagrid-pager")?.offsetHeight || 50;
    const rowHeight = 30; 

    const availableHeight = gridHeight - headerHeight - pagerHeight;
    const possibleRows = Math.floor(availableHeight / rowHeight);

    if (possibleRows > 0) {
        gridInstance.option("paging.pageSize", possibleRows);
    }
}


function renderSmallDatagrid(gridId, dataSource, columns, ExtraSettings = {}, customContextMenuFn = null, rowClickFn = null, rowDoubleClickFn = null) {
    var defaultContextMenuItems = function (rowData, gridInstance) {
        return [
            {
                text: 'Datagrid İşlemleri',
                beginGroup: true,
                disabled: true
            },
            {
                text: "Filtreleri Temizle",
                icon: "filter",
                onItemClick: function () {
                    gridInstance.clearFilter();
                }
            }
        ];
    };

    var settings = {
        dataSource: dataSource,
        columns: columns,
        showBorders: true,
        showColumnLines: true,
        showRowLines: true,
        rowAlternationEnabled: true,
        selection: { mode: "single" },

        groupPanel: { visible: false },
        grouping: { autoExpandAll: false },
        filterRow: { visible: false },
        headerFilter: { visible: false },
        columnChooser: { enabled: false },
        export: {
            enabled: false,
            allowExportSelectedData: false,
            autoExpandAll: false
        },

        // 🔻 paging ve pager tamamen kapalı
        paging: {
            enabled: false
        },
        pager: {
            visible: false
        },

        onContextMenuPreparing: function (e) {
            const gridInstance = e.component;
            const defaultItems = defaultContextMenuItems(e.row ? e.row.data : null, gridInstance);
            const customItems = customContextMenuFn ? customContextMenuFn(e.row ? e.row.data : null, gridInstance) : [];
            e.items = [...customItems, ...defaultItems];
        },

        onExporting: function (e) {
            const workbook = new ExcelJS.Workbook();
            const worksheet = workbook.addWorksheet('Sheet1');
            DevExpress.excelExporter.exportDataGrid({
                component: e.component,
                worksheet: worksheet,
                autoFilterEnabled: true
            }).then(function () {
                workbook.xlsx.writeBuffer().then(function (buffer) {
                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), e.fileName + '.xlsx');
                });
            });
            e.cancel = true;
        },

        onRowClick: function (e) {
            if (rowClickFn) rowClickFn(e.data, e.component);
        },
        onRowDblClick: function (e) {
            if (rowDoubleClickFn) rowDoubleClickFn(e.data, e.component);
        },

        ...ExtraSettings
    };

    return $(`#${gridId}`).dxDataGrid(settings).dxDataGrid('instance');
}
