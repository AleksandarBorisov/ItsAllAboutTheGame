function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31// && charCode != 46
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

$(function () {

    $('.padding-form').on('submit', '#deposit-form', function (event) {

        //if ($(this).checkValidity())
        //{
        const $depositForm = $(this);

        var $buttonName = $depositForm.find("button[type=submit]:focus").val();

        event.preventDefault();

        if ($buttonName == 'deposit') {

            const dataToSend = $depositForm.serialize();

            $('#loading-spinner').delay(200).show(0);

            $.post($depositForm.attr('action'), dataToSend, function (response) {

                $('#loading-spinner').hide(0);

                if (response.balance == undefined) {
                    window.location.href = "/";

                } else {

                    const $balance = $('#user-balance');
                    $balance.data('balance', response.balance);

                    const symbol = $balance.data('symbol');
                    const $balanceText = $('#user-balance-text');
                    $balanceText.text(response.balance.toFixed(2) + ' ' + symbol);

                    console.log(response);
                }

            });

        } else {

            var $currentBalance = parseInt($('#user-balance-text').text());

            var $amount = $(this).find('#transaction-amount');

            var $amountVal = parseInt($amount.val());

            if ($amountVal === 0 || $amountVal > $currentBalance) {

                $amount.val($currentBalance);

            } else {

                const dataToSend = $depositForm.serialize();

                $('#loading-spinner').delay(200).show(0);

                $.post('/Transaction/Withdraw', dataToSend, function (response) {

                    $('#loading-spinner').hide(0);

                    if (response.balance == undefined) {
                        window.location.href = "/";

                    } else {

                        const $balance = $('#user-balance');
                        $balance.data('balance', response.balance);

                        const symbol = $balance.data('symbol');
                        const $balanceText = $('#user-balance-text');
                        $balanceText.text(response.balance.toFixed(2) + ' ' + symbol);

                        console.log(response);
                    }
                });
            }
        }


    });
});