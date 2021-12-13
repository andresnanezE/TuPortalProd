jQuery.extend({
    popup: function (msg, title) {
        $('.popup').html(msg).attr('title', title);
        $(function () {
            $('.popup').dialog({
                modal: true,
                width: 450,
                open: function () {
                },
                buttons: {
                    Ok: function () {
                        $(this).dialog('close');
                    }
                }
            });
        });
    }
});