$(function () {
    "use strict";



    $(".dark-mode").on("click", function () {
        if ($(".dark-mode-icon i").attr("class") == 'bx bx-sun') {
            $(".dark-mode-icon i").attr("class", "bx bx-moon");
            $("html").attr("class", "light-theme")
            localStorage.setItem('theme', 'light');
            localStorage.setItem('theme-class', 'light-theme');
            localStorage.setItem('dx-theme', 'generic.light');
            DevExpress.ui.themes.current("generic.light");

        } else {
            $(".dark-mode-icon i").attr("class", "bx bx-sun");
            $("html").attr("class", "dark-theme")
            localStorage.setItem('theme', 'dark');
            localStorage.setItem('theme-class', 'dark-theme');
            localStorage.setItem('dx-theme', 'generic.dark');
            DevExpress.ui.themes.current("generic.dark");
        }
    }),

        $(".toggle-icon").click(function () {
            $(".wrapper").toggleClass("toggled");
            $(this).find("i").toggleClass("rotate");
        });
    $(".mobile-toggle-menu").click(function () {
        $(".wrapper").toggleClass("toggled");
    });
    $(".mobile-toggle-menu-close").click(function () {
        $(".wrapper").toggleClass("toggled");
    });
    $(document).ready(function () {
        $(window).on("scroll", function () {
            $(this).scrollTop() > 300 ? $(".back-to-top").fadeIn() : $(".back-to-top").fadeOut()
        }), $(".back-to-top").on("click", function () {
            return $("html, body").animate({
                scrollTop: 0
            }, 600), !1

        })

    }),

        //$(function () {
        //    for (var e = window.location, o = $(".metismenu li a").filter(function () {
        //        return this.href == e
        //    }).addClass("").parent().addClass("mm-active"); o.is("li");) o = o.parent("").addClass("mm-show").parent("").addClass("mm-active")
        //}),
        $(function () {
            $("#menu").metisMenu()
        }),

        $(".chat-toggle-btn").on("click", function () {
            $(".chat-wrapper").toggleClass("chat-toggled")
        }), $(".chat-toggle-btn-mobile").on("click", function () {
            $(".chat-wrapper").removeClass("chat-toggled")
        }),


        $(".email-toggle-btn").on("click", function () {
            $(".email-wrapper").toggleClass("email-toggled")
        }), $(".email-toggle-btn-mobile").on("click", function () {
            $(".email-wrapper").removeClass("email-toggled")
        }), $(".compose-mail-btn").on("click", function () {
            $(".compose-mail-popup").show()
        }), $(".compose-mail-close").on("click", function () {
            $(".compose-mail-popup").hide()
        }),


        $(".switcher-btn").on("click", function () {
            $(".switcher-wrapper").toggleClass("switcher-toggled")
        }), $(".close-switcher").on("click", function () {
            $(".switcher-wrapper").removeClass("switcher-toggled")

        })

});


