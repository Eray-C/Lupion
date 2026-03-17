function applyFormValidation(formId, rules, messages) {
    $("#" + formId).validate({
        rules: rules,
        messages: messages,
        errorElement: 'span',
        errorClass: 'text-danger',
        highlight: function (element) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element) {
            $(element).removeClass('is-invalid');
        }
    });
}
