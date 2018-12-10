function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

$(function () {
    var $currentBalance = parseInt($('#balance').text());

    $(document).on('input', '#stake-amount', function (e) {
        var $amount = $(this);
        var $amountVal = parseInt($(this).val());
        if ($amountVal > $currentBalance) {
            $(this).val($amount.val().slice(0, -1));
        }
    });
});