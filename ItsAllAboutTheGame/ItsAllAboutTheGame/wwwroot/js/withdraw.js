$(function () {
    const $withdrawForm = $('#withdraw-form');

    $withdrawForm.on('submit', function (event) {

        event.preventDefault();

        const dataToSend = $withdrawForm.serialize();

        $('#loading-spinner').delay(200).show(0);

        $.post($withdrawForm.attr('action'), dataToSend, function (response) {

            $('#loading-spinner').hide(0);

            const $balance = $('#user-balance');

            $balance.data('balance', response.balance);

            const symbol = $balance.data('symbol');

            const $balanceText = $('#user-balance-text');

            $balanceText.text(response.balance.toFixed(2) + ' ' + symbol);
        });
    });
});