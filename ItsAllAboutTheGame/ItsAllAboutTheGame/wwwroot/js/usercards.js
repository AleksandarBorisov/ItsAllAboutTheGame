$(function () {

    $(document).on('click', '.delete-card-button', function () {

        $.get('/Transaction/GetAllCards', function (serverData) {

            var $form = '' +
                '<form asp-area="" asp-controller="Transaction" asp-action="DeleteCard">' +
                '<select class="w-100">';

            $.each(serverData, function (i, card) {
                $form += '<option class="' + '" value="' + card.value + '">' + card.text + '</option>';
            });

            $form += '</select >' +
                '</form>';

            //If we have no cards in the database
            if (serverData.length == 0) {
                $.confirm({
                    title: 'Delete Card!',
                    content: $form,
                    buttons: {
                        cancel: function () {
                            //close
                        },
                    }
                });
            } else {
                $.confirm({
                    title: 'Delete Card!',
                    content: $form,
                    buttons: {
                        formSubmit: {
                            text: 'Delete',
                            btnClass: 'btn-danger',
                            action: function () {

                                $('#loading-spinner').delay(200).show(0);

                                var $selected = this.$content.find(':selected').val();

                                var $selectedName = this.$content.find(':selected').text();

                                var token = $('#deposit-form').find('[name=__RequestVerificationToken]').val();

                                const dataToSend = {
                                    cardId: $selected,
                                    '__RequestVerificationToken': token
                                }
                                $.post('/Transaction/DeleteCard', dataToSend, function (serverData) {

                                    $('#loading-spinner').hide(0);

                                    //If the result is redirect or error page 
                                    if (serverData.indexOf("option") < 0) {
                                        //Redirect to local
                                        window.location.href = "/";

                                    } else {

                                        $.alert({
                                            title: 'Card Deleted!',
                                            content: 'You deleted card: ' + $selectedName,
                                        });

                                        var $newCollection = '';

                                        $.each(serverData, function (i, card) {
                                            var $expired = card.disabled == true ? 'disabled="disabled"' : '';
                                            $newCollection += '<option ' + $expired + 'value="' + card.value + '">' + card.text + '</option>';
                                        });

                                        $('#card-collection').html($newCollection);
                                    }
                                });
                            }
                        },
                        cancel: function () {
                            //close
                        },
                    },
                });
            }
        });
    });
});