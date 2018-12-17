function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31// && charCode != 46
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

$(document).ready(function () {
    $('#card-number').mask('0000 0000 0000 0000');
    $('#expiry-date').mask('00/00');

    var paddingForm = $('.padding-form');

    paddingForm.removeClass('initially-hidden');

    new WOW().init();
});
