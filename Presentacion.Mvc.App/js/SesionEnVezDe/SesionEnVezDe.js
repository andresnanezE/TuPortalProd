// Autor    :   j0hNn3LS0n r0DrIGu3Z.
jQuery.extend({
    //https://api.jqueryui.com/dialog/
    mpnotify: function () {
        $('body').append(
            '<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">' +
            '<div class="modal-dialog">' +
            '<div class="modal-content">' +
            '<div class="modal-header">' +
            '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>' +
            '<h4 class="modal-title" id="myModalLabel">Modal title</h4>' +
            '</div>' +
            '<div class="modal-body">' +
            'Hello, world!' +
            '</div>' +
            '<div class="modal-footer">' +
            '<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>'
        );

        $.pnotify.defaults.styling = "bootstrap3";
        $.fn.modal && $('#myModal').modal();
    },
    notice: function (title, text) {
        $.pnotify && $.pnotify({
            title: title,
            text: text,
            nonblock: true,
            history: false,
            delay: 6e3,
            hide: true
        });
    },
    popupSEVD_true: function (title, msg, obj, url) {
        $('<div id="popup">' + msg + '</div>').dialog({
            modal: true,
            width: 450,
            resizable: true,
            closeOnEscape: true,
            title: title,
            beforeClose: function (event, ui) { },

            hide: { effect: "fade", duration: 800 },
            open: function () {
                $(this)
                    .closest(".ui-dialog")
                    .find(".ui-dialog-titlebar-close:first")
                    .css({
                        'background-color': '#22558D',
                        'color': '#F4F4F4'
                    })
                    .hide();
                $(this)
                    .closest(".ui-dialog")
                    .find(".ui-dialog-titlebar:first")
                    .css({
                        'background-color': '#22558D',
                        'color': '#F4F4F4'
                    });
                effect: "fade"; duration: 800;
            },
            close: function () {
                $(this).remove();
            },
            buttons: {
                "Aceptar": function () {
                    console.log("obj.USUARIO2");
                    console.log(obj.USUARIO2);
                    $.ajax({
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        url: url,
                        data: window.JSON.stringify({ documento: obj.USUARIO2 }),
                        dataType: 'json',
                        complete: function (data) {
                            console.log(data);
                        }
                    })
                        .fail();
                    $(this).dialog("close");
                },
                "Cerrar": function () {
                    $(this).dialog("close");
                }
            }
        });
    },
    popupSEVD_false: function (title, msg) {
        $('<div id="popup">' + msg + '</div>').dialog({
            modal: true,
            width: 450,
            resizable: true,
            closeOnEscape: true,
            title: title,
            beforeClose: function (event, ui) { },
            hide: { effect: "fade", duration: 800 },
            open: function () {
                $(this)
                    .closest(".ui-dialog")
                    .find(".ui-dialog-titlebar-close:first")
                    .css({
                        'background-color': '#22558D',
                        'color': '#FFF'
                    })
                    .hide();
                $(this)
                    .closest(".ui-dialog")
                    .find(".ui-dialog-titlebar:first")
                    .css({
                        'background-color': '#22558D',
                        'color': '#FFF'
                    });
            },
            close: function () {
                $(this).remove();
                // alert("cerrar")
            },
            buttons: {
                "Cerrar": function () {
                    $(this).dialog("close");
                }
            }
        });
    },
    wait: function (msg) {
        var o = {};
        $('<div id="wait">' + msg + '</div>').dialog({
            height: 100,
            width: 200,
            modal: true,
            position: { of: $("form"), my: 0, at: 0 },
            open: function () {
                o = $(this);
                $(this)
                    .closest(".ui-dialog")
                    .find(".ui-dialog-titlebar:first")
                    .hide();
            },
            dialogClass: 'ui-dialog-content1'
        });

        return o;
    },
    closeWait: function () {
        $('.ui-dialog-content1').remove();
        $('.ui-dialog-content1').hide();

        $('.ui-front').remove();
        $('.ui-front').hide();
    }
});