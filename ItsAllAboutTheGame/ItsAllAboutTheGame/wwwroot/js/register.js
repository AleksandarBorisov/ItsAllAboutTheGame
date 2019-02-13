$(document).ready(function () {
    $('#birth-date').mask('00.00.0000');

    var paddingForm = $('.padding-form');

    paddingForm.removeClass('initially-hidden');

    new WOW().init();
});