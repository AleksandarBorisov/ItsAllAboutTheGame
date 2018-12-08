function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

$(function () {
    var $currentBalance = parseInt($('#balance').text());

    //Client-Side Validation if the Stake is less than the Balance
    $(document).on('input', '#stake-amount', function (e) {

        var $amount = $(this);

        var $amountVal = parseInt($(this).val());

        if ($amountVal === 0 || $amountVal > $currentBalance) {

            $(this).val($amount.val().slice(0, -1));
        }
    });

    const $container = $('.game-container');

    //Submitting Spin Form
    $container.on('submit', '.spin-form', function (event) {

        $currentForm = $(this);

        event.preventDefault();

        dataToSend = $currentForm.serialize();

        $.post($currentForm.attr('action'), dataToSend, function (serverData) {

            $('.game-one-table').html(serverData);
        });
    });
});