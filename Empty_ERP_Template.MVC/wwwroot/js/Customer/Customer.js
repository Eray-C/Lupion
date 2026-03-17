let customerId;

$(document).ready(function () {
    createCustomersDatagrid();
    createCustomerPricesDatagrid();
    getCustomers();
});

function createCustomersDatagrid() {
    var columns = [
        { dataField: "name", caption: "Firma" },
        { dataField: "taxNumber", caption: "Vergi No" }
    ];

    var rowClick = function (rowData) {
        customerId = rowData.id;
        fillCustomerDetails(customerId);
    };

    var contextMenu = function (rowData) {
        var menuItems = [];

        menuItems.push({
            text: "Ekle",
            icon: "plus",
            onItemClick: function () {
                showModal({
                    url: '/customer/add-edit/',
                    title: 'Müşteri Ekle',
                    size: 'xl',
                    successEvent: () => { }
                });
            }
        });

        if (rowData) {
            menuItems.push({
                text: "Düzenle",
                icon: "edit",
                onItemClick: function () {
                    if (!rowData) return
                    showModal({
                        url: '/customer/add-edit/',
                        title: 'Müşteri Düzenle',
                        size: 'xl',
                        successEvent: () => {
                            fillForm(rowData, "customerForm");
                        }
                    });
                }
            },
                {
                    text: "Dosyalar",
                    icon: "doc",
                    onItemClick: function () {
                        if (!rowData) return
                        openAttachmentModal("CustomerGeneralDocuments", rowData.id)
                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        if (!rowData) return
                        showDeleteAlert(`customer/${rowData.id}`, () => getCustomers());
                    }
                }
            );
        }

        return menuItems;
    };

    renderDatagrid("customers_datagrid", [], columns, null, contextMenu, rowClick, null);
}

function getCustomers() {
    genericAjax({
        url: `customer`,
        isAuth: true,
        success: function (response) {
            $("#customers_datagrid").dxDataGrid("instance").option("dataSource", response.data);
        }
    });
}

function upsertCustomer() {
    serializeFormAsync("customerForm").then(function (formData) {
        var httpType = formData.Id ? "PUT" : "POST";
        var url = formData.Id ? `customer/${formData.Id}` : `customer`;

        genericAjax({
            url: url,
            method: httpType,
            data: formData,
            isAuth: true,
            success: () => getCustomers()
        });
    });
}

function fillCustomerDetails(id) {
    genericAjax({
        url: `customer/all-data/${id}`,
        isAuth: true,
        success: function (response) {
            if (response) {
                fillForm(response.data.customer, "previewCustomerForm");
                $("#customer_prices_datagrid").dxDataGrid("instance").option("dataSource", response.data.prices)
            }
        }
    });
}





function createCustomerPricesDatagrid() {
    var columns = [
        { dataField: "name", caption: "Fiyat Adı" },
        { dataField: "price", caption: "Fiyat" },
        { dataField: "departureRegion", caption: "Çıkış Adresi" },
        { dataField: "arrivalRegion", caption: "Varış Adresi" },
        { dataField: "departureCompany", caption: "Yükleme Firması" },
        { dataField: "arrivalCompany", caption: "Teslimat Firması" },
        { dataField: "note", caption: "Not" },
    ];

    var rowClick = function (rowData) {
    };

    var contextMenu = function (rowData) {
        var menuItems = [];

        menuItems.push({
            text: "Ekle",
            icon: "plus",
            onItemClick: function () {
                showModal({
                    url: '/customer/price/add-edit/',
                    title: 'Fiyat Ekle',
                    size: 'xl',
                    variable: customerId,
                    successEvent: () => {

                    }
                });
            }
        });

        if (rowData) {
            menuItems.push({
                text: "Düzenle",
                icon: "edit",
                onItemClick: function () {
                    if (!rowData) return
                    showModal({
                        url: '/customer/price/add-edit/',
                        title: 'Fiyatı Düzenle',
                        size: 'xl',
                        successEvent: () => {
                            fillForm(rowData, "customerPriceForm");
                        }
                    });
                }
            },
                {
                    text: "Dosyalar",
                    icon: "doc",
                    onItemClick: function () {
                        if (!rowData) return
                        openAttachmentModal("CustomerPriceDocuments", rowData.id)
                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        if (!rowData) return
                        showDeleteAlert(`customer/price/${rowData.id}`, () => getCustomers());
                    }
                }
            );
        }

        return menuItems;
    };

    renderDatagrid("customer_prices_datagrid", [], columns, null, contextMenu, null, null);
}
function upsertCustomerPrice() {
    var formData = serializeForm("customerPriceForm")
    var httpType = formData.Id ? "PUT" : "POST";

    var url = formData.Id
        ? `customer/price/${formData.Id}`
        : `customer/price`;
    formData.CustomerId = customerId

    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: (response) => {
            fillCustomerDetails(formData.CustomerId)
        },
    });
}