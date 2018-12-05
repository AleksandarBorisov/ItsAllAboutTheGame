﻿$(function () {
    const $depositForm = $('#deposit-form');
    debugger;
    $depositForm.on('submit', function (event) {
        event.preventDefault();

        const dataToSend = $depositForm.serialize();

        
        
        $.post($depositForm.attr('action'), dataToSend, function (response) {

            const $balance = $('#user-balance');            
            $balance.data('balance', response.balance);

            const symbol = $balance.data('symbol');
            const $balanceText = $('#user-balance-text');
            $balanceText.text(response.balance.toFixed(2) + ' ' + symbol);

            console.log(response);            
        });
    });

});