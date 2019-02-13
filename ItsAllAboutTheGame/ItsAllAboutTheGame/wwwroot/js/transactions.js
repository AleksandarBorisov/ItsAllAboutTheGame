function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31// && charCode != 46
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}

$(document).ready(function () {

    var paddingForm = $('#transactions-management');

    paddingForm.removeClass('initially-hidden');

    new WOW().init();
});

$(function () {

    const $container = $('#transactions-container');

    //Page Count Form
    $container.on('click', '.page-count-form-button', function (event) {

        event.preventDefault();

        var $currentSize = $(this).parent().siblings('.transactions-count').data('transactions-count');

        var $input = $(this).parent().siblings('.transactions-count');

        var $inputVal = $input.val();

        if ($inputVal > 0) {

            $('#loading-spinner').delay(200).show(0);

            $button = $(this);

            $form = $button.parents('form:first');

            dataToSend = $form.serialize();

            $.post($form.attr('action'), dataToSend, function (serverData) {

                $('#loading-spinner').hide(0);

                $container.html(serverData);
            });
        } else {

            $input.val($currentSize);
        }
    });

    //Search Form
    $container.on('click', '.search-form-button', function (event) {

        $('#loading-spinner').delay(200).show(0);

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {

            $('#loading-spinner').hide(0);

            $container.html(serverData);
        });
    });

    //Pagination Form
    $container.on('click', '.pagination-form-button', function (event) {

        $('#loading-spinner').delay(200).show(0);

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {

            $('#loading-spinner').hide(0);

            $container.html(serverData);
        });
    });

    //Sorting Form
    $container.on('click', '.sorting-form-button', function (event) {

        $('#loading-spinner').delay(200).show(0);

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {

            $('#loading-spinner').hide(0);

            $container.html(serverData);
        });
    });
});