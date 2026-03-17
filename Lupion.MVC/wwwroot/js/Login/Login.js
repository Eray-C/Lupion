var isLoginInProgress = false;

$(document).ready(function () {
    $('#loginForm').on('submit', function (e) {
        e.preventDefault();
        if (isLoginInProgress) return;
        Login();
    });

    $('#forgotPasswordLink').on('click', function (e) {
        e.preventDefault();
        $('#forgotPasswordModal').modal('show');
    });

    $('#sendResetEmail').on('click', function () {
        var email = $('#ResetEmail').val()?.trim();
        if (!email) return;
        $.ajax({
            url: '/Login/ForgotPassword',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ email }),
            success: function (response) {
                if (response.success) {
                    $('#ResetEmailConfirm').val(email);
                    $('#ResetCode').val('');
                    $('#NewPassword').val('');
                    $('#forgotPasswordModal').modal('hide');
                    $('#resetPasswordModal').modal('show');
                }
            },
            error: function (xhr) {
                var msg = xhr.responseJSON?.message || 'İşlem başarısız.';
                if (typeof toastr !== 'undefined') toastr.warning(msg);
            }
        });
    });

    $('#ResetCode').on('input', function () {
        this.value = this.value.replace(/\D/g, '').slice(0, 6);
    });

    $('#resetPasswordButton').on('click', function () {
        var email = $('#ResetEmailConfirm').val()?.trim();
        var code = $('#ResetCode').val()?.trim();
        var password = $('#NewPassword').val();
        if (!email || !code || code.length !== 6) {
            if (typeof toastr !== 'undefined') toastr.warning('Lütfen geçerli 6 haneli kodu girin.');
            return;
        }
        if (!password || password.length < 6) {
            if (typeof toastr !== 'undefined') toastr.warning('Yeni şifre en az 6 karakter olmalıdır.');
            return;
        }
        $.ajax({
            url: '/Login/ResetPassword',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ email, token: code, password }),
            success: function (response) {
                if (response.success) {
                    if (typeof toastr !== 'undefined') toastr.success(response.message || 'Şifre güncellendi.');
                    $('#resetPasswordModal').modal('hide');
                }
            },
            error: function (xhr) {
                var msg = xhr.responseJSON?.message || 'Şifre güncellenemedi.';
                if (typeof toastr !== 'undefined') toastr.warning(msg);
            }
        });
    });
});

function Login() {
    if (isLoginInProgress) return;
    var emailOrUserName = $("#EmailOrUserName").val();
    var password = $("#Password").val();

    isLoginInProgress = true;
    var $btn = $('#loginForm button[type="submit"]');
    var originalText = $btn.html();
    $btn.prop('disabled', true).html('<i class="bx bx-loader-alt bx-spin"></i> Giriş yapılıyor...');

    $.ajax({
        url: "/Login/Login",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ emailOrUserName, password }),
        success: function (response) {
            if (response.success) {
                localStorage.setItem("access_token", response.data.token);
                window.location.href = "/Dashboard";
                return;
            }
            resetLoginButton($btn, originalText);
        },
        error: function (xhr) {
            resetLoginButton($btn, originalText);
            var msg = xhr.responseJSON?.message || (xhr.status === 429 ? 'Giriş işlemi zaten devam ediyor. Lütfen bekleyin.' : 'Giriş başarısız.');
            if (typeof toastr !== 'undefined') toastr.warning(msg);
        },
        complete: function () {
            isLoginInProgress = false;
        }
    });
}

function resetLoginButton($btn, originalText) {
    $btn.prop('disabled', false).html(originalText);
}
