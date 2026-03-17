function datagridAjaxRequest(url, method, data, gridId) {
    method = method || "GET";
    data = data || {}

    $.ajax({
        url: baseAPIURL + url,
        method: method,
        contentType: "application/json",
        dataType: "json",
        //header:
        data: data,
        success: function (response) {
            if (response) {
                $(`#${gridId}`).dxDataGrid("instance").option("dataSource", response.data);;
            }
        },
        error: function (xhr) {
            if (xhr.status === 401) { window.location.href = '/Login/Unauthorized'; return; }
        }
    });
}
function AjaxRequestWithoutAction(url, method, data, success) {
    method = method || "GET";
    data = data || {}

    $.ajax({
        url: baseAPIURL + url,
        method: method,
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(data),
        success: success,
        error: function (xhr) {
            if (xhr.status === 401) { window.location.href = '/Login/Unauthorized'; return; }
        }
    });
}

function ajaxRequest({
    url,
    type = 'GET',
    data = {},
    dataType = 'json',
    beforeSend = null,
    success = null,
    error = null,
    complete = null
}) {
    $.ajax({
        url: baseAPIURL + url,
        type: type,
        data: data,
        contentType: "application/json",
        dataType: dataType,
        beforeSend: beforeSend,
        success: success,
        error: function (xhr) {
            if (xhr.status === 401) { window.location.href = '/Login/Unauthorized'; return; }
            if (error) error(xhr);
        },
        complete: complete
    });
}



function genericAjax({
    url,
    method = 'GET',
    data = null,
    contentType = 'application/json',
    dataType = 'json',
    headers = {},
    success = function (res) { },
    error = function (xhr) { },
    beforeSend = function () { },
    complete = function () { },
    isAuth = false,
    showLoader = true,
    processData = false,
    loaderSelector = "#global-loader"
}) {
    
    if (showLoader) $(loaderSelector).show();
    let ajaxData = data;

    if (data && method.toUpperCase() !== 'GET' && contentType === 'application/json') {
        ajaxData = JSON.stringify(data);
    }
    $.ajax({
        url: baseAPIURL+ url,
        type: method,
        contentType: contentType,
        processData: processData,
        data: ajaxData,
        headers: headers,

        beforeSend: function () {
            beforeSend();
        },
        success: function (response) {
            success(response);
        },
        error: function (xhr) {
            if (xhr.status === 401) {
                window.location.href = '/Login/Unauthorized';
                return;
            }
            error(xhr);
        },
        complete: function () {
            if (showLoader) $(loaderSelector).hide();
            complete();
        }
    });
}




async function genericFetch({
    url,
    method = 'GET',
    data = null,
    isAuth = false,
    headers = {},
    contentType = 'application/json',
    showLoader = true,
    loaderSelector = "#global-loader",
    onSuccess = function (res) { },
    onError = function (err) { }
}) {
    try {
        if (showLoader) document.querySelector(loaderSelector)?.classList.remove('d-none');

        if (isAuth) {
            const token = localStorage.getItem("token");
            if (token) headers['Authorization'] = `Bearer ${token}`;
        }

        if (contentType && !(data instanceof FormData)) {
            headers['Content-Type'] = contentType;
        }

        const options = {
            method,
            headers,
        };

        if (data) {
            options.body = contentType === 'application/json' && !(data instanceof FormData)
                ? JSON.stringify(data)
                : data;
        }

        const response = await fetch(baseAPIURL+url, options);

        if (response.status === 401) {
            window.location.href = '/Login/Unauthorized';
            return;
        }

        const result = await response.json().catch(function () { return null; });

        if (response.ok) {
            onSuccess(result);
        } else {
            onError(result);
        }
    } catch (err) {
        onError(err);
    } finally {
        if (showLoader) document.querySelector(loaderSelector)?.classList.add('d-none');
    }
}
