
function createGeneralTypesDatagrid() {
    var columns = [
        { dataField: "name", caption: "Tip" },
        { dataField: "description", caption: "Açıklama" },
    ];

    var contextMenu = function (rowData, gridInstance) {
        return [
            {
                text: "Ekle",
                icon: "plus",
                onItemClick: function () {
                    showModal({
                        url: '/Shared/frmAddGeneralTypes',
                        title: 'Tip Ekle',
                        successEvent: () => {
                            var category = $("#GeneralType #Category").val();
                            $("#generalTypeForm #Category").val(category);
                        }
                    });
                }
            },
            {
                text: "Düzenle",
                icon: "edit",
                onItemClick: function () {
                    showModal({
                        url: '/Shared/frmAddGeneralTypes',
                        title: 'Tip Ekle',
                        successEvent: () => {
                            fillForm(rowData, "generalTypeForm")
                        }
                    });
                }
            },

            {
                text: "Sil",
                icon: "trash",
                onItemClick: function (e) {
                    showDeleteAlert(`shared/types/${rowData.id}`, () => getGeneralTypes())
                }
            }
        ];
    };

    renderDatagrid("general_types_datagrid", [], columns, null, contextMenu)
}




function upsertGeneralType() {
    var formData = serializeForm("generalTypeForm");
    var httpType = formData.Id ? "PUT" : "POST";
    var url = formData.Id ? `shared/types/${formData.Id}` : `shared/types`;
    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: () => getGeneralTypes()
    });
}

function getGeneralTypes() {
    var category = $("#GeneralType #Category").val() || null;
    var parentId = $("#GeneralType #ParentId").val() || null;

    if (!category && !parentId) {
        return;
    }

    var url = `shared/types/${category}/`;
    if (parentId) {
        url += `${parentId}/`;
    }

    genericAjax({
        url: url,
        isAuth: true,
        success: function (response) {
            $("#general_types_datagrid").dxDataGrid("instance").option("dataSource", response.data);
        }
    });
}
