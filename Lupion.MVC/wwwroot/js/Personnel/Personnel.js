let personnelId;

$(document).ready(function () {
    createPersonnelsDatagrid();
    createPersonnelLicencesDatagrid();
    createPersonnelSalaryDatagrid();
    createPersonnelBonusDatagrid();
    createPersonnelDeductionDatagrid();
    createPersonnelAdvanceDatagrid();
    createPersonnelPaymentHistoryDatagrid();
    createPersonnelContactDatagrid();
    createPersonnelRelativeContactDatagrid();
    getPersonnels();
});

function createPersonnelsDatagrid() {
    var columns = [
        { dataField: "fullName", caption: "Adı Soyadı" },
        { dataField: "personnelTypeName", caption: "Görevi" },
        { dataField: "statusName", caption: "Durumu" }
    ];

    var rowClick = function (rowData) {
        personnelId = rowData.id;
        fillPersonnelDetails(personnelId)
    };

    var contextMenu = function (rowData, gridInstance) {
        var menuItems = [];

        menuItems.push(
            {
                text: "Ekle",
                icon: "plus",
                onItemClick: function () {
                    showModal({
                        url: '/personnel/add-edit/',
                        title: 'Personel Ekle',
                        size: 'xl',
                        successEvent: () => {

                        }
                    });
                },
            }
        );

        if (rowData) {
            menuItems.push(
                {
                    text: "Düzenle",
                    icon: "edit",
                    onItemClick: function () {
                        if (!rowData) return
                        showModal({
                            url: '/personnel/add-edit/',
                            title: 'Personel Düzenle',
                            size: 'xl',
                            successEvent: () => {
                                fillForm(rowData, "personnelForm")
                                loadPersonnelPhoto(rowData.id)
                            }
                        });
                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        if (!rowData) return
                        showDeleteAlert(`personnel/${rowData.id}`, () => getPersonnels())
                    }
                }
            );
        }

        return menuItems;
    };

    renderDatagrid("personnels_datagrid", [], columns, null, contextMenu, rowClick, null);
}
async function upsertPersonnel() {
    var formData = await serializeFormAsync("personnelForm")
    var httpType = formData.Id ? "PUT" : "POST";

    var url = formData.Id
        ? `personnel/${formData.Id}`
        : `personnel`;

    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: (response) => {
            getPersonnels();
        },
    });
}


function getPersonnels() {
    genericAjax({
        url: `personnel`,
        isAuth: true,
        success: function (response) {
            $("#personnels_datagrid").dxDataGrid("instance").option("dataSource", response.data)
        },
    });
}


function createPersonnelLicencesDatagrid() {
    var columns = [
        { dataField: "licenseTypeName", caption: "Tip" },
        { dataField: "category", caption: "Kategori" },
        { dataField: "issueDate", caption: "Başlangıç Tarihi", dataType: 'date' },
        { dataField: "expiryDate", caption: "Bitiş Tarihi", dataType: 'date' },
    ];

    var rowDbClick = function (rowData) {
    };

    var contextMenu = function (rowData, gridInstance) {
        var menuItems = [];
        menuItems.push({
            text: "Ekle",
            icon: "plus",
            onItemClick: function () {
                showModal({
                    url: '/personnel/license/add-edit/',
                    title: 'Ehliyet veya Sertifika Ekle',
                    size: 'xl',
                    variable: personnelId,

                    successEvent: () => {
                    }
                });
            }
        });


        if (rowData) {
            menuItems.push(
                {
                    text: "Düzenle",
                    icon: "edit",
                    onItemClick: function () {
                        if (!rowData) return

                        showModal({
                            url: '/personnel/license/add-edit/',
                            title: 'Düzenle',
                            size: 'xl',
                            successEvent: () => {
                                fillForm(rowData, "personnelLicenseForm")
                            }
                        });
                    }
                },

                {
                    text: "Dosyalar",
                    icon: "doc",
                    onItemClick: function () {
                        if (!rowData) return
                        openAttachmentModal("PersonnelLicenseDocuments", rowData.id, "PersonnelDocumentType")
                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        if (!rowData) return

                        showDeleteAlert(`personnel/license/${rowData.id}`, () => getPersonnelLicenses(rowData.personnelId))
                    }
                }
            );
        }

        return menuItems;
    };

    renderDatagrid("personnel_licenses_datagrid", [], columns, null, contextMenu, null, rowDbClick);
}


function upsertPersonnelLicense() {
    var formData = serializeForm("personnelLicenseForm")
    var httpType = formData.Id ? "PUT" : "POST";

    var url = formData.Id
        ? `personnel/license/${formData.Id}`
        : `personnel/license`;
    formData.PersonnelId = personnelId

    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: (response) => {
            getPersonnelLicenses(formData.PersonnelId);
        },
    });
}

function getPersonnelLicenses(personnelId) {
    genericAjax({
        url: `personnel/license/${personnelId}`,
        isAuth: true,
        success: function (response) {
            $("#personnel_licenses_datagrid").dxDataGrid("instance").option("dataSource", response.data)
        },
    });
}


//SALARY
function createPersonnelSalaryDatagrid() {
    var columns = [
        { dataField: "netSalary", caption: "Net Maaş" },
        { dataField: "paymentType", caption: "Ödeme Tipi" },
        { dataField: "bankAccount", caption: "Banka Hesabı" },
        { dataField: "notes", caption: "Not" },
        { dataField: "currencyName", caption: "Para Birimi" },
    ];

    var rowDbClick = function (rowData) {
    };

    var contextMenu = function (rowData, gridInstance) {
        var menuItems = [];
        menuItems.push({
            text: "Ekle",
            icon: "plus",
            onItemClick: function () {
                showModal({
                    url: '/personnel/salary/add-edit/',
                    title: 'Maaş Ekle',
                    size: 'xl',
                    variable: personnelId,
                    successEvent: () => {

                    }
                });
            }
        });
        if (rowData) {
            menuItems.push(
                {
                    text: "Düzenle",
                    icon: "edit",
                    onItemClick: function () {
                        if (!rowData) return
                        showModal({
                            url: '/personnel/salary/add-edit/',
                            title: 'Maaş Düzenle',
                            size: 'xl',
                            successEvent: () => {
                                fillForm(rowData, "personnelSalaryForm")
                            }
                        });
                    }
                },
                {
                    text: "Dosyalar",
                    icon: "doc",
                    onItemClick: function () {
                        if (!rowData) return
                        openAttachmentModal("PersonnelSalaryDocuments", rowData.id, "PersonnelDocumentType")

                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        if (!rowData) return
                        showDeleteAlert(`personnel/salary/${rowData.id}`, () => getPersonnelSalary(rowData.personnelId))
                    }
                }
            );
        }

        return menuItems;
    };

    renderDatagrid("personnel_salaries_datagrid", [], columns, null, contextMenu, null, rowDbClick);
}

function createPersonnelBonusDatagrid() {
    var columns = [
        { dataField: "typeName", caption: "Tip" },
        { dataField: "currencyName", caption: "Para Birimi" },
        { dataField: "amount", caption: "Tutar", dataType: "number", format: "#,##0.##" },
        { dataField: "year", caption: "Yıl" },
        { dataField: "monthName", caption: "Ay" },
        { dataField: "notes", caption: "Not" }
    ];
    var contextMenu = function (rowData, gridInstance) {
        var menuItems = [{
            text: "Ekle",
            icon: "plus",
            onItemClick: function () {
                showModal({
                    url: '/personnel/bonus/add-edit/',
                    title: 'Bonus Ekle',
                    size: 'lg',
                    variable: personnelId,
                    successEvent: () => getPersonnelBonus(personnelId)
                });
            }
        }];
        if (rowData) {
            menuItems.push(
                {
                    text: "Düzenle",
                    icon: "edit",
                    onItemClick: function () {
                        showModal({
                            url: '/personnel/bonus/add-edit/',
                            title: 'Bonus Düzenle',
                            size: 'lg',
                            successEvent: () => fillForm(rowData, "personnelBonusForm")
                        });
                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        showDeleteAlert(`personnel/bonus/${rowData.id}`, () => getPersonnelBonus(rowData.personnelId));
                    }
                }
            );
        }
        return menuItems;
    };
    renderDatagrid("personnel_bonuses_datagrid", [], columns, null, contextMenu, null, null);
}

function createPersonnelDeductionDatagrid() {
    var columns = [
        { dataField: "typeName", caption: "Tip" },
        { dataField: "currencyName", caption: "Para Birimi" },
        { dataField: "amount", caption: "Tutar", dataType: "number", format: "#,##0.##" },
        { dataField: "year", caption: "Yıl" },
        { dataField: "monthName", caption: "Ay" },
        { dataField: "notes", caption: "Not" }
    ];
    var contextMenu = function (rowData, gridInstance) {
        var menuItems = [{
            text: "Ekle",
            icon: "plus",
            onItemClick: function () {
                showModal({
                    url: '/personnel/deduction/add-edit/',
                    title: 'Kesinti Ekle',
                    size: 'lg',
                    variable: personnelId,
                    successEvent: () => getPersonnelDeduction(personnelId)
                });
            }
        }];
        if (rowData) {
            menuItems.push(
                {
                    text: "Düzenle",
                    icon: "edit",
                    onItemClick: function () {
                        showModal({
                            url: '/personnel/deduction/add-edit/',
                            title: 'Kesinti Düzenle',
                            size: 'lg',
                            successEvent: () => fillForm(rowData, "personnelDeductionForm")
                        });
                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        showDeleteAlert(`personnel/deduction/${rowData.id}`, () => getPersonnelDeduction(rowData.personnelId));
                    }
                }
            );
        }
        return menuItems;
    };
    renderDatagrid("personnel_deductions_datagrid", [], columns, null, contextMenu, null, null);
}

function createPersonnelAdvanceDatagrid() {
    var columns = [
        { dataField: "advanceDate", caption: "Avans Tarihi", dataType: "date", format: "dd.MM.yyyy" },
        { dataField: "startDeductionDate", caption: "Kesinti Başlangıç", dataType: "date", format: "dd.MM.yyyy" },
        { dataField: "currencyName", caption: "Kur" },
        { dataField: "givenAmount", caption: "Verilen Tutar", dataType: "number", format: "#,##0.##" },
        { dataField: "remainingAmount", caption: "Kalan Tutar", dataType: "number", format: "#,##0.##" },
        { dataField: "deductionMonths", caption: "Kaç Ay Kesinti" },
        { dataField: "deductionAmountPerMonth", caption: "Aylık Kesinti", dataType: "number", format: "#,##0.##" },
        { dataField: "isCompleted", caption: "Kapalı", dataType: "boolean" },
        { dataField: "advanceClosedDate", caption: "Kapanış Tarihi", dataType: "date", format: "dd.MM.yyyy" },
        { dataField: "notes", caption: "Not" }
    ];
    var contextMenu = function (rowData, gridInstance) {
        var menuItems = [{
            text: "Ekle",
            icon: "plus",
            onItemClick: function () {
                showModal({
                    url: '/personnel/advance/add-edit/',
                    title: 'Avans Ekle',
                    size: 'lg',
                    variable: personnelId,
                    successEvent: () => getPersonnelAdvance(personnelId)
                });
            }
        }];
        if (rowData) {
            menuItems.push(
                {
                    text: "Düzenle",
                    icon: "edit",
                    onItemClick: function () {
                        showModal({
                            url: '/personnel/advance/add-edit/',
                            title: 'Avans Düzenle',
                            size: 'lg',
                            successEvent: () => fillForm(rowData, "personnelAdvanceForm")
                        });
                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        showDeleteAlert(`personnel/advance/${rowData.id}`, () => getPersonnelAdvance(rowData.personnelId));
                    }
                }
            );
            if (!rowData.isCompleted) {
                menuItems.push({
                    text: "Avansı Kapat",
                    icon: "check",
                    onItemClick: function () {
                        genericAjax({
                            url: `personnel/advance/${rowData.id}/close`,
                            method: "PUT",
                            isAuth: true,
                            success: function () {
                                getPersonnelAdvance(rowData.personnelId);
                            }
                        });
                    }
                });
            }
        }
        return menuItems;
    };
    renderDatagrid("personnel_advances_datagrid", [], columns, null, contextMenu, null, null);
}

function createPersonnelPaymentHistoryDatagrid() {
    var columns = [
        { dataField: "periodYear", caption: "Yıl" },
        { dataField: "monthName", caption: "Ay" },
        { dataField: "realizedDate", caption: "Gerçekleşme Tarihi", dataType: "date", format: "dd.MM.yyyy" },
        { dataField: "netSalary", caption: "Net Maaş", dataType: "number", format: "#,##0.##" },
        { dataField: "travelExpense", caption: "Harcırah", dataType: "number", format: "#,##0.##" },
        { dataField: "bonusTotal", caption: "Bonus", dataType: "number", format: "#,##0.##" },
        { dataField: "deductionTotal", caption: "Kesinti", dataType: "number", format: "#,##0.##" },
        { dataField: "advanceDeductionTotal", caption: "Avans Kesintisi", dataType: "number", format: "#,##0.##" },
        { dataField: "totalPayable", caption: "Ödenen Toplam", dataType: "number", format: "#,##0.##" },
        { dataField: "notes", caption: "Not", allowWordWrap: true }
    ];
    renderDatagrid("personnel_payment_history_datagrid", [], columns, null, null, null, null);
}

function upsertPersonnelAdvance() {
    var formData = serializeForm("personnelAdvanceForm");
    formData.PersonnelId = personnelId;
    var httpType = formData.Id ? "PUT" : "POST";
    var url = formData.Id ? `personnel/advance/${formData.Id}` : `personnel/advance`;
    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: function (response) {
            getPersonnelAdvance(personnelId);
        }
    });
}

function getPersonnelAdvance(personnelId) {
    genericAjax({
        url: `personnel/advance/${personnelId}`,
        isAuth: true,
        success: function (response) {
            $("#personnel_advances_datagrid").dxDataGrid("instance").option("dataSource", response.data);
        }
    });
}

function upsertPersonnelBonus() {
    var formData = serializeForm("personnelBonusForm");
    formData.PersonnelId = personnelId;
    var httpType = formData.Id ? "PUT" : "POST";
    var url = formData.Id ? `personnel/bonus/${formData.Id}` : `personnel/bonus`;
    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: function (response) {
            getPersonnelBonus(personnelId);
        }
    });
}

function getPersonnelBonus(personnelId) {
    genericAjax({
        url: `personnel/bonus/${personnelId}`,
        isAuth: true,
        success: function (response) {
            $("#personnel_bonuses_datagrid").dxDataGrid("instance").option("dataSource", response.data);
        }
    });
}

function upsertPersonnelDeduction() {
    var formData = serializeForm("personnelDeductionForm");
    formData.PersonnelId = personnelId;
    var httpType = formData.Id ? "PUT" : "POST";
    var url = formData.Id ? `personnel/deduction/${formData.Id}` : `personnel/deduction`;
    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: function (response) {
            getPersonnelDeduction(personnelId);
        }
    });
}

function getPersonnelDeduction(personnelId) {
    genericAjax({
        url: `personnel/deduction/${personnelId}`,
        isAuth: true,
        success: function (response) {
            $("#personnel_deductions_datagrid").dxDataGrid("instance").option("dataSource", response.data);
        }
    });
}

function upsertPersonnelSalary() {
    var formData = serializeForm("personnelSalaryForm")
    var httpType = formData.Id ? "PUT" : "POST";

    var url = formData.Id
        ? `personnel/salary/${formData.Id}`
        : `personnel/salary`;

    formData.PersonnelId = personnelId

    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: (response) => {
            getPersonnelSalary(formData.PersonnelId);
        },
    });
}

function getPersonnelSalary(personnelId) {
    genericAjax({
        url: `personnel/salary/${personnelId}`,
        isAuth: true,
        success: function (response) {
            $("#personnel_salaries_datagrid").dxDataGrid("instance").option("dataSource", response.data)
        },
    });
}



function createPersonnelContactDatagrid() {
    var columns = [
        { dataField: "personalEmail", caption: "Kişisel Email" },
        { dataField: "workEmail", caption: "İş Email" },
        { dataField: "mobilePhone", caption: "Mobil Telefon" },
        { dataField: "homePhone", caption: "Ev Telefonu" },
        { dataField: "addressLine1", caption: "Açık Adres" },
        { dataField: "cityId", caption: "İl" },
        { dataField: "stateId", caption: "İlçe" },
    ];

    var contextMenu = function (rowData, gridInstance) {
        var menuItems = [];
        menuItems.push({
            text: "Ekle",
            icon: "plus",
            onItemClick: function () {
                showModal({
                    url: '/personnel/contact/add-edit/',
                    title: 'İletişim Ekle',
                    size: 'xl',
                    variable: personnelId,
                    successEvent: () => getPersonnelContacts(personnelId)
                });
            }
        });
        if (rowData) {
            menuItems.push(
                {
                    text: "Düzenle",
                    icon: "edit",
                    onItemClick: function () {
                        showModal({
                            url: '/personnel/contact/add-edit/',
                            title: 'İletişim Düzenle',
                            size: 'xl',
                            successEvent: function () {
                                fillForm(rowData, "personnelContactForm");
                                if (rowData.cityId) {
                                    $("#ContactCityId").val(rowData.cityId).trigger("change");
                                    setTimeout(function () {
                                        if (rowData.stateId) $("#ContactTownId").val(rowData.stateId).trigger("change");
                                        $("#personnelContactForm #CityId").val(rowData.cityId || "");
                                        $("#personnelContactForm #StateId").val(rowData.stateId || "");
                                    }, 400);
                                }
                            }
                        });
                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        showDeleteAlert(`personnel/contact/${rowData.id}`, () => getPersonnelContacts(personnelId));
                    }
                }
            );
        }
        return menuItems;
    };

    renderDatagrid("personnel_contacts_datagrid", [], columns, null, contextMenu, null, null);
}

function upsertPersonnelContact() {
    var $form = $("#personnelContactForm");
    if ($form.find("#ContactCityId").length) {
        $form.find("#CityId").val($form.find("#ContactCityId").val() || "");
        $form.find("#StateId").val($form.find("#ContactTownId").val() || "");
    }
    var formData = serializeForm("personnelContactForm");
    var httpType = formData.Id ? "PUT" : "POST";
    var url = formData.Id ? `personnel/contact/${formData.Id}` : `personnel/contact`;
    formData.PersonnelId = personnelId;

    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: () => getPersonnelContacts(personnelId),
    });
}

function getPersonnelContacts(personnelId) {
    genericAjax({
        url: `personnel/contact/${personnelId}`,
        isAuth: true,
        success: function (response) {
            $("#personnel_contacts_datagrid").dxDataGrid("instance").option("dataSource", response.data);
        },
    });
}




//RELATIVE CONTACT
function createPersonnelRelativeContactDatagrid() {
    var columns = [
        { dataField: "fullName", caption: "Adı Soyadı" },
        { dataField: "relationshipTypeName", caption: "Yakınlık Derecesi" },
        { dataField: "genderTypeName", caption: "Cinsiyeti" },
        { dataField: "contactPhone", caption: "Telefonu" },
        { dataField: "contactEmail", caption: "Maili" },
        { dataField: "address", caption: "Adres" },
        { dataField: "notes", caption: "Not" },
        { dataField: "isEmergencyContact", caption: "Acil durum kişisi" },
    ];

    var rowDbClick = function (rowData) {
    };

    var contextMenu = function (rowData, gridInstance) {
        var menuItems = [];
        menuItems.push({
            text: "Ekle",
            icon: "plus",
            onItemClick: function () {
                showModal({
                    url: '/personnel/relative-contact/add-edit/',
                    title: 'İletişim Ekle',
                    size: 'xl',
                    variable: personnelId,
                    successEvent: () => {

                    }
                });
            }
        });
        if (rowData) {
            menuItems.push(
                {
                    text: "Düzenle",
                    icon: "edit",
                    onItemClick: function () {
                        if (!rowData) return
                        showModal({
                            url: '/personnel/relative-contact/add-edit/',
                            title: 'İletişim Düzenle',
                            size: 'xl',
                            successEvent: () => {
                                fillForm(rowData, "personnelRelativeContactForm")
                            }
                        });
                    }
                },
                {
                    text: "Dosyalar",
                    icon: "doc",
                    onItemClick: function () {
                        if (!rowData) return
                        openAttachmentModal("PersonnelRelativeContactDocuments", rowData.id, "PersonnelDocumentType")

                    }
                },
                {
                    text: "Sil",
                    icon: "trash",
                    onItemClick: function () {
                        if (!rowData) return
                        showDeleteAlert(`personnel/relative-contact/${rowData.id}`, () => getPersonnelRelativeContact(rowData.personnelId))
                    }
                }
            );
        }

        return menuItems;
    };

    renderDatagrid("personnel_relative_contacts_datagrid", [], columns, null, contextMenu, null, rowDbClick);
}


function upsertPersonnelRelativeContact() {
    var formData = serializeForm("personnelRelativeContactForm")
    var httpType = formData.Id ? "PUT" : "POST";

    var url = formData.Id
        ? `personnel/relative-contact/${formData.Id}`
        : `personnel/relative-contact`;

    formData.PersonnelId = personnelId
    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: (response) => {
            getPersonnelRelativeContact(formData.PersonnelId);
        },
    });
}

function getPersonnelRelativeContact(personnelId) {
    genericAjax({
        url: `personnel/relative-contact/${personnelId}`,
        isAuth: true,
        success: function (response) {
            $("#personnel_relative_contacts_datagrid").dxDataGrid("instance").option("dataSource", response.data)
        },
    });
}


function fillPersonnelDetails(personnelId) {
    genericAjax({
        url: `personnel/all-data/${personnelId}`,
        isAuth: true,
        success: function (response) {
            fillForm(response.data.personnel, "previewPersonnelForm")
            $("#personnel_licenses_datagrid").dxDataGrid("instance").option("dataSource", response.data.licences);
            $("#personnel_salaries_datagrid").dxDataGrid("instance").option("dataSource", response.data.salaries);
            $("#personnel_bonuses_datagrid").dxDataGrid("instance").option("dataSource", response.data.bonuses || []);
            $("#personnel_deductions_datagrid").dxDataGrid("instance").option("dataSource", response.data.deductions || []);
            $("#personnel_advances_datagrid").dxDataGrid("instance").option("dataSource", response.data.advances || []);
            $("#personnel_payment_history_datagrid").dxDataGrid("instance").option("dataSource", response.data.paymentHistory || []);
            $("#personnel_contacts_datagrid").dxDataGrid("instance").option("dataSource", response.data.contacts);
            $("#personnel_relative_contacts_datagrid").dxDataGrid("instance").option("dataSource", response.data.relatives);
        },
    });
    createFileField("GeneralPersonnel", personnelId)
}


function previewPersonnelPhoto(event) {
    var file = event.target.files[0];
    var img = document.getElementById("personnelPPpreview");

    if (file && file.type.startsWith("image/")) {
        img.src = URL.createObjectURL(file);
    } else {
        img.src = "/assets/images/avatars/blank-image.jpg";
        event.target.value = "";
    }
}
function loadPersonnelPhoto(personnelId) {
    genericAjax({
        url: `personnel/${personnelId}/photo`,
        isAuth: true,
        success: function (response) {
            if (response && response.data) {
                $("#personnelPPpreview").attr("src", "data:image/png;base64," + response.data);
                $("#ExistingPhoto").val(response.data);
            } else {
                $("#personnelPPpreview").attr("src", "/assets/images/avatars/blank-image.jpg");
                $("#ExistingPhoto").val("");
            }
        }
    });
}

function openTypeForm() {
    showModal({
        url: '/generalType/GetPartialView?name=frmGeneralTypes',
        title: 'Tipler',
        successEvent: () => {
            createGeneralTypesDatagrid()
            fillGeneralTypesDatagrid()
        }
    });
}
function openPersonnelTypeForm() {
    showModal({
        url: '/Shared/GeneralTypes',
        title: 'Tipler',
        size: 'lg',
        successEvent: () => {
            $("#GeneralType #Category").val("PersonnelType");
            createGeneralTypesDatagrid()
            getGeneralTypes()
        }
    });
}
function openLicenseTypeForm() {
    showModal({
        url: '/Shared/GeneralTypes',
        title: 'Tipler',
        size: 'lg',
        successEvent: () => {
            $("#GeneralType #Category").val("LicenseType");
            createGeneralTypesDatagrid()
            getGeneralTypes()
        }
    });
}
function openRelationshipTypeForm() {
    showModal({
        url: '/Shared/GeneralTypes',
        title: 'Tipler',
        size: 'lg',
        successEvent: () => {
            $("#GeneralType #Category").val("RelationshipType");
            createGeneralTypesDatagrid()
            getGeneralTypes()
        }
    });
}

function openDocumentTypeForm() {
    showModal({
        url: '/Shared/GeneralTypes',
        title: 'Tipler',
        size: 'lg',
        successEvent: () => {
            $("#GeneralType #Category").val("PersonnelDocumentType");
            createGeneralTypesDatagrid();
            getGeneralTypes();
        }
    });
}

function openBonusTypeForm() {
    showModal({
        url: '/Shared/GeneralTypes',
        title: 'Bonus Tipleri',
        size: 'lg',
        successEvent: () => {
            $("#GeneralType #Category").val("PersonnelBonusType");
            createGeneralTypesDatagrid();
            getGeneralTypes();
        }
    });
}

function openDeductionTypeForm() {
    showModal({
        url: '/Shared/GeneralTypes',
        title: 'Kesinti Tipleri',
        size: 'lg',
        successEvent: () => {
            $("#GeneralType #Category").val("PersonnelDeductionType");
            createGeneralTypesDatagrid();
            getGeneralTypes();
        }
    });
}
