function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31/* && charCode != 46*/
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
//If we wrap the entire jQuery function we will take advantage of document.load event for which jQuery lisens by default 

$(document).ready(function () {

    var paddingForm = $('#users-management');

    paddingForm.removeClass('initially-hidden');

    new WOW().init();
});

$(function () {

    const $container = $('#users-container');

    $container.on('submit', '.lockout-form', function (event) {

        $currentForm = $(this);

        $username = $currentForm.parent().siblings('.username').text();

        $days = $currentForm.find('#LockoutFor').val();

        event.preventDefault();

        $.confirm({
            title: 'Lockout!',
            content: `You are about to lockout user with username ${$username} for ${$days} days!`,
            buttons: {
                confirm: function () {
                    $('#loading-spinner').delay(200).show(0);

                    var lockoutFor = $currentForm.find('.lockout-for').val();
                    var userId = $currentForm.find('.user-id').val();
                    var token = $currentForm.find('[name=__RequestVerificationToken]').val();

                    const dataToSend = {
                        userId: userId,
                        lockoutFor: lockoutFor,
                        '__RequestVerificationToken': token
                    }

                    $.post($currentForm.attr('action'), dataToSend, function (serverData) {

                        $('#loading-spinner').hide(0);

                        $($currentForm.parent('tr')).html(serverData);

                        $.alert({
                            title: 'Locked!',
                            content: 'You locked out the user with username: ' + $username,
                        });
                    });
                },
                cancel: function () {

                },
            }
        });
    });

    //Delete Form
    $container.on('click', '.delete-button', function (event) {

        $checkbox = $(this);

        $username = $(this).parents('td').siblings('.username').text();

        $message = $checkbox.prop('checked') ? 'Delete' : 'Restore';

        $.confirm({
            title: 'Confirm!',
            content: `${$message} user with username: ${$username}?`,
            buttons: {
                confirm: function () {
                    $('#loading-spinner').delay(200).show(0);

                    var url = '/Administration/Users/Delete';
                    var userId = $checkbox.siblings('.user-id').val();
                    var token = $checkbox.siblings('[name=__RequestVerificationToken]').val();

                    $.post(url,
                        {
                            userId: userId,
                            '__RequestVerificationToken': token
                        }
                        , function (serverData) {

                            $('#loading-spinner').hide(0);

                            $($currentForm.parent('tr')).html(serverData);
                        });
                },
                cancel: function () {
                    if ($checkbox.prop('checked')) {
                        $checkbox.prop('checked', false);
                    }
                    else {
                        $checkbox.prop('checked', true);
                    }
                },
            }
        });
    });

    //Toggle Admin Form
    $container.on('click', '.toggle-button', function (event) {

        $checkbox = $(this);

        $username = $(this).parents('td').siblings('.username').text();

        $message = $checkbox.prop('checked') ? 'Assign' : 'Remove';

        $.confirm({
            title: 'Confirm!',
            content: `${$message} Admin Role to user with username: ${$username}?`,
            buttons: {
                confirm: function () {
                    $('#loading-spinner').delay(200).show(0);

                    var url = '/Administration/Users/ToggleAdmin';
                    var userId = $checkbox.siblings('.user-id').val();
                    var token = $checkbox.siblings('[name=__RequestVerificationToken]').val();

                    $.post(url,
                        {
                            userId: userId,
                            '__RequestVerificationToken': token
                        }
                        , function (serverData) {

                            $('#loading-spinner').hide(0);

                            $($currentForm.parent('tr')).html(serverData);
                        });
                },
                cancel: function () {
                    if ($checkbox.prop('checked')) {
                        $checkbox.prop('checked', false);
                    }
                    else {
                        $checkbox.prop('checked', true);
                    }
                },
            }
        });
    });

    //Page Count Form
    $container.on('click', '.page-count-form-button', function (event) {

        event.preventDefault();

        var $currentSize = $(this).parent().siblings('.users-count').data('users-count');

        var $input = $(this).parent().siblings('.users-count');

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