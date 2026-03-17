function capitalizeFirstLetter(string) {
    if (!string) return string;
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function fillForm(obj, containerId) {
    clearForm(containerId)
    if (!obj) return;

    let $container = containerId ? $("#" + containerId) : $(document);


    $.each(obj, function (key, value) {
        let keyCapitalized = capitalizeFirstLetter(key);
        let $element = $container.find("#" + keyCapitalized);

        if (!$element.length) {
            $element = $container.find("[name='" + keyCapitalized + "']");
        }

        if (!$element.length) return;


        if (key.endsWith("CityId")) {

            let prefixCapitalized = keyCapitalized.replace("CityId", "");
            let prefix = key.replace("CityId", "");

            waitUntilOptionsLoaded("#" + prefixCapitalized + "CityId", function () {

                let cityId = obj[prefix + "CityId"];
                $("#" + prefixCapitalized + "CityId").val(cityId).trigger("change");

                waitUntilOptionsLoaded("#" + prefixCapitalized + "TownId", function () {

                    let townId = obj[prefix + "TownId"];
                    $("#" + prefixCapitalized + "TownId").val(townId).trigger("change");

                });
            });

            return; 
        }

        else {
            $element.each(function () {
                let $el = $(this);
                let tag = $el.prop("tagName").toLowerCase();
                let type = $el.attr("type");

                if (tag === "select") {
                    if ($el.attr("bool") !== undefined) {
                        if (typeof value === "boolean") {
                            value = value ? "1" : "0";
                        }
                    }

                    if ($el.hasClass('selectpicker')) {
                        const values = Array.isArray(value)
                            ? value.map(v => v.toString())
                            : (value != null ? [value.toString()] : []);
                        $el.selectpicker('val', values).trigger('changed.bs.select');
                    }

                    else if ($el.prop('multiple')) {
                        const values = Array.isArray(value) ? value : (value != null ? [value] : []);
                        $el.val(values).trigger("change.select2");
                    } else {
                        $el.val(value).trigger("change.select2");
                    }
                }

                else if (tag === "textarea") {
                    if ($el.attr("id") && window.CKEDITOR && window.CKEDITOR.instances && CKEDITOR.instances[$el.attr("id")]) {
                        CKEDITOR.instances[$el.attr("id")].setData(value || '');
                    } else if ($el[0].ckeditorInstance) {
                        $el[0].ckeditorInstance.setData(value || '');
                    } else {
                        $el.val(value);
                    }
                }

                else if (tag === "input") {
                    if (type === "checkbox") {
                        $el.prop("checked", !!value);
                    } else if (type === "radio") {
                        if ($el.val() == value) {
                            $el.prop("checked", true);
                        }
                    } else if (type === "date") {
                        if (typeof value === "string" && /^\d{4}-\d{2}-\d{2}$/.test(value)) {
                            $el.val(value);
                        } else {
                            let formattedDate = formatDate(value);
                            $el.val(formattedDate);
                        }
                    } else {
                        $el.val(value);
                    }
                }

                else if (tag === "img") {
                    if (value) {
                        if (value.startsWith("data:")) {
                            $el.attr("src", value);
                        } else {
                            $el.attr("src", "data:image/png;base64," + value);
                        }
                    } else {
                        $el.attr("src", "/assets/images/avatars/blank-image.jpg");
                    }
                }
            });

        }
    });
}


function waitUntilOptionsLoaded(selector, callback) {
    let tryCount = 0;

    let interval = setInterval(() => {

        let el = $(selector)[0];

        if (el && el.options.length > 1) {
            clearInterval(interval);
            callback(); 
        }

        if (++tryCount > 30) { 
            clearInterval(interval);
            callback();
        }

    }, 100);
}

function serializeForm(formId) {
    var formData = {};
    var $form = $("#" + formId);

    $form.find("input, select, textarea").each(function () {
        var $element = $(this);
        var type = $element.attr("type");
        var name = $element.attr("name");
        var id = $element.attr("id");
        var tag = $element.prop("tagName").toLowerCase();
        var key = name || id;

        if (!key) return;

        var rawValue = $element.val();
        if (rawValue === "") {
            formData[key] = null;
            return;
        }

        var isBool = $element.is("[bool]");

        switch (type) {
            case "checkbox":
                formData[key] = $element.is(":checked");
                break;
            case "radio":
                if ($element.is(":checked")) {
                    formData[key] = rawValue;
                } else if (formData[key] === undefined) {
                    formData[key] = null;
                }
                break;
            case "number":
                var parsed = parseFloat(rawValue);
                formData[key] = isNaN(parsed) ? null : parsed;
                break;
            case "date":
                var date = new Date(rawValue);
                formData[key] = isNaN(date.getTime()) ? null : date.toISOString();
                break;
            default:
                if (tag === "select") {
                    if ($element.prop("multiple")) {
                        formData[key] = Array.isArray(rawValue) ? rawValue : [rawValue];
                    } else {
                        formData[key] = isBool ? rawValue === "1" : rawValue;
                    }
                } else {
                    formData[key] = isBool ? rawValue === "1" : rawValue;
                }
                break;
        }
    });
    $form.find(".dx-tagbox").each(function () {
        var $tagBoxContainer = $(this);
        var instance = $tagBoxContainer.dxTagBox("instance");
        if (instance) {
            var id = $tagBoxContainer.attr("id");
            if (id) {
                formData[id] = instance.option("value");
            }
        }
    });

    return formData;
}




async function serializeFormAsync(formId) {
    var formData = {};
    var $form = $("#" + formId);

    var filePromises = [];

    $form.find("input, select, textarea").each(function () {
        var $element = $(this);
        var type = $element.attr("type");
        var name = $element.attr("name");
        var id = $element.attr("id");
        var tag = $element.prop("tagName").toLowerCase();
        var key = name || id;

        if (!key) return;

        var rawValue = $element.val();
        if (rawValue === "" && type !== "file") {
            formData[key] = null;
            return;
        }

        var isBool = $element.is("[bool]");

        switch (type) {
            case "checkbox":
                formData[key] = $element.is(":checked");
                break;
            case "radio":
                if ($element.is(":checked")) {
                    formData[key] = rawValue;
                } else if (formData[key] === undefined) {
                    formData[key] = null;
                }
                break;
            case "number":
                var parsed = parseFloat(rawValue);
                formData[key] = isNaN(parsed) ? null : parsed;
                break;
            case "date":
                var date = new Date(rawValue);
                formData[key] = isNaN(date.getTime()) ? null : date.toISOString();
                break;
            case "file":
                var files = $element[0].files;
                if (files && files.length > 0) {
                    filePromises.push(new Promise((resolve) => {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            var base64 = e.target.result.split(",")[1];
                            formData[key] = base64;
                            resolve();
                        };
                        reader.readAsDataURL(files[0]);
                    }));
                } else {
                    formData[key] = null;
                }
                break;
            default:
                if (tag === "select") {
                    if ($element.prop("multiple")) {
                        formData[key] = Array.isArray(rawValue) ? rawValue : [rawValue];
                    } else {
                        formData[key] = isBool ? rawValue === "1" : rawValue;
                    }
                } else {
                    formData[key] = isBool ? rawValue === "1" : rawValue;
                }
                break;
        }
    });

    // DevExtreme TagBox
    $form.find(".dx-tagbox").each(function () {
        var $tagBoxContainer = $(this);
        var instance = $tagBoxContainer.dxTagBox("instance");
        if (instance) {
            var id = $tagBoxContainer.attr("id");
            if (id) {
                formData[id] = instance.option("value");
            }
        }
    });

    if (filePromises.length > 0) {
        await Promise.all(filePromises);
    }

    return formData;
}





function serializeEntity(containerSelector, subEntities = {}) {
    const $container = $(containerSelector);

    function getValue($el) {
        const tag = ($el.prop("tagName") || "").toLowerCase();
        const type = ($el.attr("type") || "").toLowerCase();
        let val = $el.val();

        if (tag === "input") {
            if (type === "checkbox") return $el.is(":checked");
            if (type === "radio") return $el.is(":checked") ? val : null;
        }
        if (type === "number") {
            return val ? parseFloat(val) : null;
        }
        return val && val.trim() !== "" ? val : null;
    }

    function serializeContainer($c) {
        const obj = {};
        $c.find("input,select,textarea").each(function () {
            const $el = $(this);
            let name = $el.attr("name") || $el.attr("id");
            if (!name) return;
            const val = getValue($el);
            obj[name.charAt(0).toLowerCase() + name.slice(1)] = val;
        });
        return obj;
    }

    // Ana entity alanları
    const mainData = serializeContainer($container);

    // Alt entity alanları
    for (const [propName, subSel] of Object.entries(subEntities)) {
        const $sub = $(subSel);
        if ($sub.length && !$sub.hasClass("d-none")) {
            const subObj = serializeContainer($sub);
            const hasValue = Object.values(subObj).some(v => v !== null && v !== "");
            mainData[propName] = hasValue ? subObj : null;
        } else {
            mainData[propName] = null;
        }
    }

    return mainData;
}

// Verileriniz array olarak geliyor, ilk elemanı alın
const data = [
    {
        "id": 1,
        "vehicleId": 1,
        "documentTypeId": 35,
        "currencyId": null,
        "documentNumber": "1231231",
        "issueDate": "2025-08-15T00:00:00",
        "expiryDate": "2025-08-29T00:00:00",
        "issuer": "23123",
        "comment": "123",
        "documentType": {
            "id": 35,
            "name": "Muayene"
        },
        "currency": null,
        "inspection": {
            "id": 2,
            "inspectionDate": null,
            "nextInspectionDate": null,
            "result": "46"
        },
        "insurance": null,
        "licence": null
    }
];

function fillEntity(containerSelector, data, subEntities = {}) {
    if (!data) return;
    const $container = $(containerSelector);

    function toFormName(key) {
        return key.charAt(0).toUpperCase() + key.slice(1);
    }



    function setValue($el, value) {
        const tag = ($el.prop("tagName") || "").toLowerCase();
        const type = ($el.attr("type") || "").toLowerCase();

        if (tag === "select") {
            $el.val(value).trigger("change.select2");
        } else if (tag === "textarea") {
            const id = $el.attr("id");
            if (id && window.CKEDITOR && CKEDITOR.instances && CKEDITOR.instances[id]) {
                CKEDITOR.instances[id].setData(value || "");
            } else if ($el[0] && $el[0].ckeditorInstance) {
                $el[0].ckeditorInstance.setData(value || "");
            } else {
                $el.val(value ?? "");
            }
        } else if (tag === "input") {
            if (type === "checkbox") {
                $el.prop("checked", !!value);
            } else if (type === "radio") {
                if ($el.val() == (value ?? "")) $el.prop("checked", true);
            } else if (type === "date") {
                $el.val(formatDate(value));
            } else {
                $el.val(value ?? "");
            }
        } else {
            $el.val(value ?? "");
        }
    }

    // Ana entity alanları
    Object.keys(data).forEach(key => {
        if (subEntities[key]) return; // alt entity'ler sonra işlenecek

        const formKey = toFormName(key);
        let $els = $container.find("#" + formKey);
        if (!$els.length) $els = $container.find("[name='" + formKey + "']");
        if (!$els.length) return;

        $els.each(function () {
            setValue($(this), data[key]);
        });
    });

    // Alt entity alanları
    Object.entries(subEntities).forEach(([propName, subSelector]) => {
        const $sub = $(subSelector);
        if (!$sub.length) return;

        const subData = data[propName];
        if (!subData) return;

        Object.keys(subData).forEach(sKey => {
            const formKey = toFormName(sKey);
            let $els = $sub.find("#" + formKey);
            if (!$els.length) $els = $sub.find("[name='" + formKey + "']");
            if (!$els.length) return;

            $els.each(function () {
                setValue($(this), subData[sKey]);
            });
        });
    });
}

function formatDate(value) {
    if (!value) return "";
    if (typeof value === "string" && /^\d{4}-\d{2}-\d{2}$/.test(value)) return value;
    const d = new Date(value);
    if (isNaN(d)) return "";
    const yyyy = d.getFullYear();
    const mm = String(d.getMonth() + 1).padStart(2, "0");
    const dd = String(d.getDate()).padStart(2, "0");
    return `${yyyy}-${mm}-${dd}`;
}
function clearForm(formId) {
    var $form = $("#" + formId);

    $form.find("input, select, textarea").each(function () {
        var type = $(this).attr("type") || "text"; // boşsa text kabul et
        var tag = this.tagName.toLowerCase();

        if (tag === "input") {
            switch (type) {
                case "text":
                case "password":
                case "email":
                case "number":
                case "date":
                case "hidden":
                    $(this).val("");
                    break;
                case "checkbox":
                case "radio":
                    $(this).prop("checked", false);
                    break;
                case "file":
                    $(this).val(null);
                    break;
            }
        } else if (tag === "select") {
            $(this).prop("selectedIndex", 0);
        } else if (tag === "textarea") {
            $(this).val("");
        }
    });
}

