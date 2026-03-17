function createFilesDatagrid() {
    var columns = [
        { dataField: "fileName", caption: "Dosya Adı" },
        { dataField: "fileType", caption: "Dosya Tipi" },
        { dataField: "documentTypeName", caption: "Belge Tipi", customizeText: function (e) { return e.value || "—"; } },
        { dataField: "comment", caption: "Not" },
        {
            caption: "Ön İzleme",
            type: "buttons",
            width: 120,
            buttons: [
                {
                    hint: "Görüntüle",
                    icon: "fa fa-eye",
                    onClick: function (e) {
                        previewAttachment(e.row.data);
                    }
                }
            ]
        }
    ];

    var contextMenu = function (rowData, gridInstance) {
        return [
            {
                text: "Ekle",
                icon: "plus",
                onItemClick: function (e) {
                    var qs = (typeof window.attachmentDocumentTypeCategory === 'string' && window.attachmentDocumentTypeCategory)
                        ? '?documentTypeCategory=' + encodeURIComponent(window.attachmentDocumentTypeCategory)
                        : '';
                    showModal({
                        url: '/Attachment/LayoutAttachmentUploader' + qs,
                        title: 'Dosya Ekle',
                        size: 'lg',
                        isFile: false
                    });
                }
            },
            {
                text: "İndir",
                icon: "download",
                onItemClick: function (x) {
                    downloadFileDirect(rowData.id);
                }
            },
            {
                text: "Sil",
                icon: "trash",
                onItemClick: function (e) {
                    showDeleteAlert(`attachments/${rowData.id}`, () => getDocuments(rowData.fieldName, rowData.fieldId))
                }
            },
            {
                text: "Paylaş",
                icon: "fa fa-share-alt",
                items: [
                    {
                        text: "WhatsApp ile Paylaş",
                        icon: "fa-brands fa-whatsapp",
                        onItemClick: function () {
                            $.get(baseAPIURL + "attachments/" + rowData.id + "/url", function (res) {
                                shareOnWhatsApp(res.data.url, res.data.fileName);
                            });
                        }
                    },
                    {
                        text: "Telegram ile Paylaş",
                        icon: "fa-brands fa-telegram",
                        onItemClick: function () {
                            $.get(baseAPIURL + "attachments/" + rowData.id + "/url", function (res) {
                                    shareOnTelegram(res.data.url, res.data.fileName);
                            });
                        }
                    }
                ]
            }
        ];
    };

    renderDatagrid("files_grid", [], columns, null, contextMenu)
}

function uploadAttachments(fieldName, fieldId) {
    const newFiles = [];

    $('#fileGrid .grid-item').each(function () {
        const $el = $(this);
        newFiles.push({
            id: $el.data('id') || null,
            file: $el.data('file'),
            comment: $el.find('.note-input').val(),
            documentTypeId: $el.find('.document-type-select').val() || null
        });
    });

    const formData = new FormData();
    const toUpload = newFiles.filter(f => !f.id && f.file);

    var hasDocType = $('#fileGrid .document-type-select').length > 0;
    toUpload.forEach(function (item, index) {
        formData.append("Files", item.file);
        formData.append("FileNames", item.file.name);
        formData.append("FileTypes", item.file.type);
        formData.append("OriginalSizes", item.file.size);
        formData.append("Comments", item.comment || "");
        if (hasDocType) {
            var typeId = (item.documentTypeId !== "" && item.documentTypeId != null) ? item.documentTypeId : "";
            formData.append("DocumentTypeIds[" + index + "]", typeId);
        }
    });

    formData.append("FieldId", String(fieldId));
    formData.append("FieldName", fieldName);

    $.ajax({
        url: baseAPIURL + "attachments",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.success) {
                getDocuments(fieldName, fieldId)
            }
        },
    });
}


// documentTypeCategory: opsiyonel; verilirse bu alan için dosya eklerken tip seçimi gösterilir (örn. "PersonnelDocumentType")
function createFileField(fieldName, fieldId, documentTypeCategory) {
    window.attachmentDocumentTypeCategory = documentTypeCategory || null;
    $("#DocumentFieldName").val(fieldName);
    $("#DocumentFieldId").val(fieldId);
    getDocuments(fieldName, fieldId);

    $("#Files .files-wrapper").remove();

    $("#Files").append(`
        <div class="files-wrapper mt-3">
            <div id="files_grid"></div>
        </div>
    `);

    createFilesDatagrid();
}
function getDocuments(fieldName, fieldId) {
    genericAjax({
        url: `attachments/${fieldName}/${fieldId}`,
        isAuth: true,
        success: (response) => {
            var grid = $("#files_grid").dxDataGrid("instance");

            grid.option("dataSource", response.data);
            grid.refresh();
        },
    });
}


function downloadFileDirect(id) {
    $.get(baseAPIURL + "attachments/" + id + "/url", function (res) {
        if (res.data.success && res.data.url) {
            fetch(res.data.url, { method: "GET" })
                .then(resp => resp.blob())
                .then(blob => {
                    const url = window.URL.createObjectURL(blob);
                    const a = document.createElement("a");
                    a.href = url;

                    a.download = res.data.fileName || "indirilen_dosya";

                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                    window.URL.revokeObjectURL(url);
                });
        } else {
            alert("URL alınamadı!");
        }
    });
}
function previewAttachment(rowData) {
    $.get(baseAPIURL + "attachments/" + rowData.id + "/url", function (res) {
        if (!(res.success && res.data && res.data.url)) {
            alert("URL alınamadı!");
            return;
        }

        const url = res.data.url;
        const fileName = (res.data.fileName || "").toLowerCase();

        let viewerUrl = "";

        if (fileName.endsWith(".pdf")) {
            // PDF dosyaları → direkt iframe
            viewerUrl = url;
        }
        else if (fileName.match(/\.(jpg|jpeg|png|gif|bmp|webp|svg)$/)) {
            // Görseller → direkt iframe
            viewerUrl = url;
        }
        else if (fileName.match(/\.(xls|xlsx)$/)) {
            // Excel dosyaları → Microsoft Office Online Viewer
            viewerUrl = "https://view.officeapps.live.com/op/view.aspx?src=" + encodeURIComponent(url);
        }
        else if (fileName.match(/\.(doc|docx|ppt|pptx)$/)) {
            // Word ve PowerPoint → Google Viewer (veya aynı şekilde Office viewer da kullanılabilir)
            viewerUrl = "https://view.officeapps.live.com/op/view.aspx?src=" + encodeURIComponent(url);
        }
        else {
            // Desteklenmeyen dosya → direkt indir
            window.open(url, "_blank");
            return;
        }

        showModal({
            url: '/Attachment/LayoutAttachmentViewer',
            title: rowData.fileName,
            size: 'xl',
            successEvent: () => {
                $("#previewFrame").attr("src", viewerUrl);
            }
        });
    });
}

// documentTypeCategory: dışarıdan verilirse (örn. "PersonnelDocumentType") dosya eklerken tip seçimi gösterilir; verilmezse tip alanı yok
function openAttachmentModal(fieldName, fieldId, documentTypeCategory) {
    window.attachmentDocumentTypeCategory = documentTypeCategory || null;
    showModal({
        url: '/Attachment/LayoutAttachment',
        title: 'Dosyalar',
        size: 'xl',
        isFile: true,
        successEvent: () => {
            $('#DocumentFieldName').val(fieldName);
            $('#DocumentFieldId').val(fieldId);
            getDocuments($('#DocumentFieldName').val(), $('#DocumentFieldId').val());
        }
    });
}


function getFileIcon(mimeType) {
    if (!mimeType) return 'fas fa-file';

    if (mimeType.startsWith("image/")) return 'fa-solid fa-image';
    if (mimeType === "application/pdf") return 'fas fa-file-pdf';
    if (mimeType.startsWith("video/")) return 'fas fa-video';
    if (mimeType.startsWith("audio/")) return 'fas fa-music';

    if (mimeType.includes("word")) return 'fas fa-file-word';
    if (mimeType.includes("excel") || mimeType.includes("spreadsheet")) return 'fas fa-file-excel';
    if (mimeType.includes("powerpoint")) return 'fas fa-file-powerpoint';

    if (mimeType.startsWith("text/")) return 'fas fa-file-alt';
    if (mimeType.includes("json") || mimeType.includes("xml")) return 'fas fa-file-code';

    return 'fas fa-file';
}

function getFileType(file) {
    if (!file || !file.name) return "other";

    const ext = file.name.split('.').pop().toLowerCase();
    const mime = file.type ? file.type.toLowerCase() : "";

    if (mime === "application/pdf" || ext === "pdf") return "pdf";
    if (mime.startsWith("image/")) return ext;
    if (mime.startsWith("video/")) return ext;
    if (mime.startsWith("audio/")) return ext;

    const knownExts = ["doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "zip", "rar", "7z"];
    if (knownExts.includes(ext)) return ext;

    return "other";
}


function shareOnWhatsApp(fileUrl, fileName) {
    const text = `${fileName}\n${fileUrl}`;
    const whatsappUrl = "https://wa.me/?text=" + encodeURIComponent(text);
    window.open(whatsappUrl, "_blank");
}

function shareOnTelegram(fileUrl, fileName) {
    const text = `${fileName}\n${fileUrl}`;
    const telegramUrl = "https://t.me/share/url?url=" + encodeURIComponent(fileUrl) + "&text=" + encodeURIComponent(fileName);
    window.open(telegramUrl, "_blank");
}
