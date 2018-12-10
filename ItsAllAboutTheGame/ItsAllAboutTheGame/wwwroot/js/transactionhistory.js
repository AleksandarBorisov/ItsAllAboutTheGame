
$(function () {
    debugger;
    //We set an event on all forms
    const $container = $('#transactions-history-container');


    //Page Count Form
    $container.on('click', '.page-count-form-button', function (event) {

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {
            $('#transaction-table-pagination').html(serverData);
        });
    });


    //Pagination Form
    $container.on('click', '.pagination-form-button', function (event) {

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {
            $('#transaction-table-pagination').html(serverData);
        });
    });

    //Sorting Form
    $container.on('click', '.sorting-form-button', function (event) {

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {
            $('#transaction-table-pagination').html(serverData);
        });
    });
});