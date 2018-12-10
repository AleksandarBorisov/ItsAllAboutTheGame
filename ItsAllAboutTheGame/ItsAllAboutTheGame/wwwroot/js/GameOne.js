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

            const $gameContainer = $('.game-container');
            //We replace the whole html part
            $gameContainer.html(serverData);
            //We find the new balance data
            var $newBalance = $gameContainer.find('#balance').data('balance');
            //We are taking the field user-balance
            const $balance = $('#user-balance');
            //We update its data-balance with the new balance
            $balance.attr('data-balance', $newBalance);
            //We take its data-symbol
            const symbol = $balance.data('symbol');
            //Finally we update the text of the text field
            $('#user-balance-text').text($newBalance + ' ' + symbol);
        });
    });

    //Disable scrolling
    $('html, body').css({
        overflow: 'hidden',
        height: '100%'
    });

    //Enable scrolling
    //$('html, body').css({
    //    overflow: 'auto',
    //    height: 'auto'
    //});
});