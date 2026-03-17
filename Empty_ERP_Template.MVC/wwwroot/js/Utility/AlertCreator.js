function showSuccessMessage(jqXHR, ajaxSettings) {
    if (ajaxSettings.type == "GET") return

    var response = jqXHR.responseJSON;

    if (jqXHR.status == 200 && response.success) {
        toastr.success(response.message || 'İşlem başarılı');
    }
    else {
        toastr.error(response.message);
    }
}
function showErrorMessage(jqXHR, ajaxSettings) {
    if (jqXHR.status === 401) return;
    if (jqXHR.status === 403) {
        var method = (ajaxSettings && ajaxSettings.type) ? ajaxSettings.type.toUpperCase() : '';
        if (method !== 'GET') toastr.error('İznin yok.');
        return;
    }
    var response = jqXHR.responseJSON;

    if (jqXHR.status === 400 && response && response.validationErrors) {
        response.validationErrors.forEach((error) => {
            toastr.error(error.errorMessage);
        });
    }
    else {
        toastr.error(response.message);
    }
}
function showDeleteAlert(url, func) {
    Swal.fire({
        title: "Silmek istediğinize emin misiniz?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Evet, sil",
        cancelButtonText: "İptal"
    }).then((result) => {
        if (result.isConfirmed) {
            ajaxRequest({
                url: url,
                type: "DELETE",
                success: function (response) {
                    func();
                },
                error: function (x) {
                },
            });
        }
    });
}
function showAlert(type, message, title) {
    title = title || "";

    switch (type) {
        case "success":
            toastr.success(message, title, {
                timeOut: 3000,
                closeButton: true,
                progressBar: true
            });
            break;

        case "error":
            toastr.error(message, title, {
                timeOut: 5000,
                closeButton: true,
                progressBar: true
            });
            break;

        case "warning":
            toastr.warning(message, title, {
                timeOut: 4000,
                closeButton: true,
                progressBar: true
            });
            break;

        case "info":
            toastr.info(message, title, {
                timeOut: 3000,
                closeButton: true,
                progressBar: true
            });
            break;

        default:
            alert(message); 
            break;
    }
}
