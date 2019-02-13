jQuery(document).ready(function () {

    var btn = $('#index-caption');
    var navbar = $('#main-navbar');

    btn.on('click', function (e) {
        e.preventDefault();
        $("html, body").animate({ scrollTop: ($('#games').offset().top - navbar.height() - 20)}, 600);
    });

    var contactPage = $('.index-page');

    contactPage.removeClass('initially-hidden');

    new WOW().init();
});