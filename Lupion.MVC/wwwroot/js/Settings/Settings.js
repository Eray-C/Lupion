$(document).ready(function () {

});

function openUsersModal() {
    showModal({
        url: '/settings/GetPartialView?name=User/frmUsers',
        title: 'Kullanıcılar',
        size: 'xl',
        successEvent: () => {
            createUserDatagrid();
            getUsers();
        }
    });
}

function createUserDatagrid() {
    var columns = [
        { dataField: "firstName", caption: "Adı" },
        { dataField: "lastName", caption: "Soyadı" },
        { dataField: "username", caption: "Kullanıcı Adı" },
        { dataField: "email", caption: "Mail" },
        { dataField: "isActive", caption: "Aktif mi" },

    ];
    var rowDbClick = function (rowData) {

    }
    var contextMenu = function (rowData, gridInstance) {
        return [
            {
                text: "Ekle",
                icon: "plus",
                onItemClick: function () {
                    showModal({
                        url: '/settings/users/add-edit',
                        title: 'Ekle',
                        size: 'lg',
                        successEvent: () => {
                            userFormValidation()
                        }
                    });
                }
            },
            {
                text: "Düzenle",
                icon: "edit",
                onItemClick: function () {
                    showModal({
                        url: '/settings/users/add-edit',
                        title: 'Düzenle',
                        size: 'lg',
                        successEvent: () => {
                            fillForm(rowData, "userAddEditForm");
                            $('#PasswordContainer').hide();
                        }
                    });
                }
            },
            {
                text: "Sil",
                icon: "trash",
                onItemClick: function (e) {
                    showDeleteAlert(`settings/users/${rowData.id}`, () => getUsers())
                }
            },

        ];
    };

    renderDatagrid("users_datagrid", [], columns, null, contextMenu, null, rowDbClick)
}

function getUsers() {
    datagridAjaxRequest(`settings/users`, null, null, "users_datagrid")
}

function openRolesModal() {
    showModal({
        url: '/settings/GetPartialView?name=Role/frmRoles',
        title: 'Roller',
        size: 'xl',
        successEvent: function () {
            createRolesDatagrid();
            getRoles();
        }
    });
}

function createRolesDatagrid() {
    var columns = [
        { dataField: "name", caption: "Rol Adı" },
        { dataField: "description", caption: "Açıklama" }
    ];
    var contextMenu = function (rowData, gridInstance) {
        return [
            {
                text: "Ekle",
                icon: "plus",
                onItemClick: function () {
                    showModal({
                        url: '/settings/roles/add-edit',
                        title: 'Rol Ekle',
                        size: 'lg',
                        successEvent: function () { roleFormValidation(); }
                    });
                }
            },
            {
                text: "Düzenle",
                icon: "edit",
                onItemClick: function () {
                    showModal({
                        url: '/settings/roles/add-edit',
                        title: 'Rol Düzenle',
                        size: 'lg',
                        successEvent: function () {
                            fillForm(rowData, "roleAddEditForm");
                        }
                    });
                }
            },
            {
                text: "Sil",
                icon: "trash",
                onItemClick: function () {
                    showDeleteAlert('settings/roles/' + rowData.id, function () { getRoles(); });
                }
            }
        ];
    };
    renderDatagrid("roles_datagrid", [], columns, null, contextMenu, null, null);
}

function getRoles() {
    datagridAjaxRequest('settings/roles', null, null, 'roles_datagrid');
}

function upsertRole() {
    if (!$("#roleAddEditForm").valid()) return;
    var formData = serializeForm("roleAddEditForm");
    var httpType = formData.Id ? "PUT" : "POST";
    var url = formData.Id ? 'settings/roles/' + formData.Id : 'settings/roles';
    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: function () { getRoles(); }
    });
}

function roleFormValidation() {
    applyFormValidation("roleAddEditForm",
        { Name: { required: true, minlength: 2 } },
        { Name: { required: "Rol adı zorunludur", minlength: "En az 2 karakter giriniz" } }
    );
}

function openRolePermissionsModal() {
    showModal({
        url: '/settings/role-permissions-view',
        title: 'Rol bazlı modül izinleri',
        size: 'xl',
        successEvent: function () {
            $('#rolePermissionsGrid').closest('.modal').find('#closeModalButton').remove();
            initRolePermissionsGrid();
            $('#RoleId').off('change').on('change', function () {
                var roleId = $(this).val();
                if (roleId) loadRolePermissionsGrid(parseInt(roleId));
                else setRolePermissionsGridData([]);
            });
            var roleId = $('#RoleId').val();
            if (roleId) loadRolePermissionsGrid(parseInt(roleId));
        }
    });
}

function initRolePermissionsGrid() {
    var $grid = $('#rolePermissionsGrid');
    if ($grid.data('dxDataGrid')) return;
    var columns = [
        { dataField: 'moduleName', caption: 'Yetki Adı', groupIndex: 0, allowEditing: false },
        { dataField: 'operationName', caption: 'İşlem', allowEditing: false },
        { dataField: 'description', caption: 'Açıklama', allowEditing: false },
        { dataField: 'checked', caption: 'İzin', dataType: 'boolean', allowEditing: true }
    ];
    var extraSettings = {
        height: '100%',
        editing: { mode: 'cell', allowUpdating: true },
        grouping: { autoExpandAll: true },
        paging: { pageSize: 20 },
        pager: { allowedPageSizes: [20, 50, 100] }
    };
    renderDatagrid('rolePermissionsGrid', [], columns, extraSettings);
}

function setRolePermissionsGridData(data) {
    var grid = $('#rolePermissionsGrid').dxDataGrid('instance');
    if (grid) grid.option('dataSource', data || []);
}

function loadRolePermissionsGrid(roleId) {
    genericAjax({
        url: 'settings/role-permissions/' + roleId,
        method: 'GET',
        isAuth: true,
        showLoader: false,
        success: function (res) {
            var data = res && res.data != null ? res.data : (Array.isArray(res) ? res : []);
            setRolePermissionsGridData(Array.isArray(data) ? data : []);
        }
    });
}

function saveRolePermissions() {
    var roleId = $('#RoleId').val();
    if (!roleId) { toastr.warning('Lütfen rol seçiniz'); return; }
    var data = grid.option('dataSource') || [];
    var gridItems = data.map(function (row) {
        return {
            moduleId: row.moduleId != null ? row.moduleId : row.ModuleId,
            operationId: row.operationId != null ? row.operationId : row.OperationId,
            checked: row.checked === true || row.Checked === true
        };
    }).filter(function (x) { return x.moduleId != null && x.operationId != null; });

    genericAjax({
        url: 'settings/role-permissions',
        method: 'PUT',
        data: { roleId: parseInt(roleId, 10), gridItems: gridItems },
        isAuth: true,
        showLoader: false,
        success: function () {
        }
    });
}

function upsertUser() {
    if (!$("#userAddEditForm").valid()) return;

    var formData = serializeForm("userAddEditForm");
    var httpType = formData.Id ? "PUT" : "POST";
    var url = formData.Id ? `auth/update/${formData.Id}` : "auth/register";

    genericAjax({
        url: url,
        method: httpType,
        data: formData,
        isAuth: true,
        success: function () {
            getUsers()
        }
    });
}








function userFormValidation() {
    applyFormValidation("userAddEditForm",
        {
            FirstName: {
                required: true,
                minlength: 2
            },
            LastName: {
                required: true,
                minlength: 2
            },
            Username: {
                required: true,
                minlength: 3
            },
            Password: {
                required: true,
                minlength: 6
            },
            Email: {
                required: true,
                email: true
            },
            'RoleIds': {
                required: true,
                minlength: 1
            },
            IsActive: {
                required: true
            }
        },
        {


            FirstName: {
                required: "Adı zorunludur",
                minlength: "En az 2 karakter giriniz"
            },
            LastName: {
                required: "Soyadı zorunludur",
                minlength: "En az 2 karakter giriniz"
            },
            Username: {
                required: "Kullanıcı adı zorunludur",
                minlength: "En az 3 karakter giriniz"
            },
            Password: {
                required: "Şifre zorunludur",
                minlength: "En az 6 karakter giriniz"
            },
            Email: {
                required: "Email zorunludur",
                email: "Geçerli bir email giriniz"
            },
            'RoleIds': {
                required: "En az bir rol seçiniz",
                minlength: "En az bir rol seçiniz"
            },
            IsActive: {
                required: "Durum seçiniz"
            }
        });
}
function Logout() {
    $.ajax({
        url: "/Login/Logout",
        type: "POST",
        success: function (response) {
            if (response.success) {
                localStorage.removeItem("access_token");
                window.location.href = "/Login";
            }
        }
    });
}
