let modalCounter = 0;

async function showModal(options) {
    modalCounter++;
    const modalId = "dynamicModal" + modalCounter;
    const dialogId = "dynamicModalDialog" + modalCounter;
    const labelId = "dynamicModalLabel" + modalCounter;
    const bodyId = "dynamicModalBody" + modalCounter;

    var url = options.url;
    var title = options.title || '';
    var size = options.size || 'md';
    var successEvents = options.successEvent;
    var isFile = options.isFile || false;
    var ajaxType = options.type || 'GET';
    var ajaxData = options.data || {};

    var hasVariable = options.hasOwnProperty('variable');

    if (hasVariable) {
        var v = options.variable;
        if (v === null || v === undefined || v === 0 || v === '') {
            toastr.error("Lütfen ilgili kaydı seçin!");
            return;
        }
    }

    const modalHtml = `
        <div class="modal fade" id="${modalId}" tabindex="-1" aria-labelledby="${labelId}" aria-hidden="true">
            <div class="modal-dialog modal-${size}" id="${dialogId}">
                <div class="modal-content">
                    <div class="modal-header text-white" style="background-color: #ffb357;">
                        <h6 class="modal-title" id="${labelId}">${title}</h6>
                        <button type="button" class="btn-close ms-auto" data-bs-dismiss="modal" aria-label="Kapat"></button>
                    </div>
                    <div class="modal-body" id="${bodyId}">
                        <div class="p-3 text-center">Yükleniyor...</div>
                    </div>
                    <div class="modal-footer" id="${modalId}_footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" id='closeModalButton'>Kapat</button>
                    </div>
                </div>
            </div>
        </div>`;

    $("body").append(modalHtml);
    $(`#${modalId}`).one('hidden.bs.modal', function () {
        $(this).remove();
    });
    $.ajax({
        url: url,
        type: ajaxType,
        data: ajaxData,
        success: function (data) {
            $(`#${bodyId}`).html(data);

            if (isFile) {
                $("body").find("#files_grid").closest("div.mt-3").remove();

                $(`#${bodyId}`).append(`
                    <div class="mt-3">
                        <div id="files_grid"></div>
                    </div>
                `);
            }

            $(`#${modalId}`).modal({
                backdrop: 'static',
                keyboard: false
            }).modal('show');

            $(`#${dialogId}`).draggable({
                handle: ".modal-header"
            });

            if (typeof successEvents === "function") {
                successEvents();
                $('.selectpicker').selectpicker();
            } else if (Array.isArray(successEvents)) {
                successEvents.forEach(function (fn) {
                    if (typeof fn === "function") fn();
                });
            }

            if (isFile && typeof createFilesDatagrid === "function") {
                createFilesDatagrid();
            }

            const $buttons = $(`#${bodyId}`).find(".footer_btn");
            if ($buttons.length) {
                $(`#${modalId} .modal-footer`).append($buttons.detach());
            }

        },
        error: function (xhr, status, error) {
            const message = xhr.status === 403 ? 'İznin yok.' : `İçerik yüklenemedi! <br> ${error}`;
            $(`#${bodyId}`).html(
                `<div class="alert alert-danger m-3">${message}</div>`
            );
            $(`#${modalId}`).modal({
                backdrop: 'static',
                keyboard: false
            }).modal('show');
        }
    });
}
