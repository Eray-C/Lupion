$(document).ajaxSend((event, jqXHR, settings) => {
    const token = localStorage.getItem("access_token");
    if (token) {
        jqXHR.setRequestHeader("Authorization", "Bearer " + token);
    }
    if (settings.type === "POST" && settings.data && !(settings.data instanceof FormData)) {
        jqXHR.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    }
    $("#loader").show();
});

$(document).ajaxSuccess((event, jqXHR, settings) => {
    showSuccessMessage(jqXHR, settings);
    if (settings.type && settings.type.toUpperCase() === "POST") {
        const $lastModal = $(".modal.show").last();
        if ($lastModal.length) {
            const modal = bootstrap.Modal.getInstance($lastModal[0]);
            if (modal) {
                modal.hide();
            }
        }
    }
});


$(document).ajaxError((event, jqXHR, settings, thrownError) => {
    try {
        if (jqXHR && jqXHR.status != 200) {
            showErrorMessage(jqXHR, settings);
        }
    }
    catch (e) {
    }
    finally {
        $("#loader").hide();
    }
});

$(document).ajaxComplete(() => {
    $("#loader").hide();
});


$(document).ready(() => {
    setThemeIcon()
    var url = window.location.href
    if (url.includes("Dashboard")) {
        $(".wrapper").removeClass("toggled");
    }
    DevExpress.localization.locale("tr");
});
document.addEventListener("DOMContentLoaded", function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl, {
            sanitize: false
        });
    });
});



const setThemeIcon = () => {
    const theme = localStorage.getItem("theme");
    if (!theme) return;

    const iconElement = document.querySelector(".dark-mode-icon i");
    if (!iconElement) return;

    if (theme === "light") {
        iconElement.className = "bx bx-moon";
        DevExpress.ui.themes.current("generic.light");
    }
    else if (theme === "dark") {
        iconElement.className = "bx bx-sun";
        DevExpress.ui.themes.current("generic.dark");
    }

};


(function () {
    function makeSearchable($select) {

        if (!($select instanceof jQuery)) {
            $select = $($select);
        }

        if (!$.fn.select2) return;

        if ($select.closest('.select2-swal2-select-container').length) return;
        if ($select.closest('#swal2-select').length) return;

        if ($select.hasClass('selectpicker') || $select.data('live-search') !== undefined) return;

        if ($select.data('select2') === 'off') return;

        if (!$select.data('select2')) {
            $select.select2({
                theme: 'bootstrap-5',
                width: '100%',
                placeholder: $select.attr('placeholder') || 'Seçiniz',
                allowClear: true,
                language: 'tr',
                dropdownParent: $(document.body)
            });
        }
    }


    // Mevcut selectler
    $('select').each(function () { makeSearchable($(this)); });

    // Sonradan eklenenler
    new MutationObserver(function (mutations) {
        mutations.forEach(function (m) {
            m.addedNodes.forEach(function (node) {
                // Sadece element düğümler
                if (node.nodeType !== 1) return;

                // Tek başına <select> eklendiyse
                if (node.tagName === 'SELECT') {
                    makeSearchable($(node));
                }

                // İçinde <select> barındırıyorsa (tamamen jQuery ile)
                $(node).find('select').each(function () {
                    makeSearchable($(this));
                });
            });
        });
    }).observe(document.body, { childList: true, subtree: true });

})();
