function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;
    return true;
}
//If we wrap the entire jQuery function we will take advantage of document.load event for which jQuery lisens by default 

$(function () {
    //We set an event on all forms
    const $container = $('#users-container');
    //const $lockoutForms = $('.lockout-form');

    //$lockoutForms.on('submit', function (event) {
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
                    var lockoutFor = $currentForm.find('.lockout-for').val();
                    var userId = $currentForm.find('.user-id').val();
                    var token = $currentForm.find('[name=__RequestVerificationToken]').val();

                    //const dataToSend = $currentForm.serialize();
                    const dataToSend = {
                        userId: userId,
                        lockoutFor: lockoutFor,
                        '__RequestVerificationToken': token
                    }

                    $.post($currentForm.attr('action'), dataToSend, function (serverData) {
                        $($currentForm.parent('tr')).html(serverData);
                    });
                    $.alert({
                        title: 'Locked!',
                        content: 'You locked out the user with username: ' + $username,
                    });
                },
                cancel: function () {

                },
            }
        });
    });

    //Delete Form
    //const $deleteForms = $('.delete-form');

    $container.on('click', '.delete-button', function (event) {

        $checkbox = $(this);

        $username = $(this).parents('td').siblings('.username').text();

        $message = $checkbox.prop('checked') ? 'Delete' : 'Restore';

        //const dataToSend = $('delete-form').serialize();

        $.confirm({
            title: 'Confirm!',
            content: `${$message} user with username: ${$username}?`,
            buttons: {
                confirm: function () {
                    var url = '/Administration/Users/Delete';
                    var userId = $checkbox.siblings('.user-id').val();
                    var token = $checkbox.siblings('[name=__RequestVerificationToken]').val();

                    $.post(url,
                        {
                            userId: userId,
                            '__RequestVerificationToken': token
                        }
                        , function (serverData) {
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
                //somethingElse: {
                //    text: 'Something else',
                //    btnClass: 'btn-blue',
                //    keys: ['enter', 'shift'],
                //    action: function () {
                //        $.alert('Something else?');
                //    }
                //}
            }
        });
    });

    //Toggle Admin Form
    //const $toggleAdmin = $('.toggleadmin-form');

    $container.on('click', '.toggle-button', function (event) {

        $checkbox = $(this);

        $username = $(this).parents('td').siblings('.username').text();

        $message = $checkbox.prop('checked') ? 'Assign' : 'Remove';

        //const dataToSend = $('toggleadmin-form').serialize();

        $.confirm({
            title: 'Confirm!',
            content: `${$message} Admin Role to user with username: ${$username}?`,
            buttons: {
                confirm: function () {
                    var url = '/Administration/Users/ToggleAdmin';
                    var userId = $checkbox.siblings('.user-id').val();
                    var token = $checkbox.siblings('[name=__RequestVerificationToken]').val();

                    $.post(url,
                        {
                            userId: userId,
                            '__RequestVerificationToken': token
                        }
                        , function (serverData) {
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
    //const $pageCountForm = $container.find('.page-count-form');

    //$pageCountForm.on('submit', function (event) {
    //    $currentForm = $(this);

    //    event.preventDefault();

    //    const dataToSend = $currentForm.serialize();

    //    $.post($currentForm.attr('action'), dataToSend, function (serverData) {
    //        debugger;
    //        $('#users-table-pagination').html(serverData);
    //    });
    //});
    //const $pageCountForm = $container.find('.page-count-form');
    //const $userCount = $container.find('.users-count');

    //Page Count Form
    $container.on('click', '.page-count-form-button', function (event) {

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {
            $('#users-table-pagination').html(serverData);
        });
    });

    //Search Form
    $container.on('click', '.search-form-button', function (event) {

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {
            $('#users-table-pagination').html(serverData);
        });
    });

    //Pagination Form
    $container.on('click', '.pagination-form-button', function (event) {

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {
            $('#users-table-pagination').html(serverData);
        });
    });

    //Sorting Form
    $container.on('click', '.sorting-form-button', function (event) {

        event.preventDefault();

        $button = $(this);

        $form = $button.parents('form:first');

        dataToSend = $form.serialize();

        $.post($form.attr('action'), dataToSend, function (serverData) {
            $('#users-table-pagination').html(serverData);
        });
    });
});