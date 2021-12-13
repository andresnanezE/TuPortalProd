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
    popup: function (title, msg) {

        $('<div id="popup">' + msg + '</div>').dialog({
            modal: true,
            width: 450,
            //autoOpen: false,
            resizable: false,
            closeOnEscape: false,
            title: title,
            beforeClose: function (event, ui) { },
            hide: { effect: "fade", duration: 800 },
            open: function () {
                $(this)
                      .closest(".ui-dialog")
                      .find(".ui-dialog-titlebar-close:first")
                      .hide();
                $(this)
                   .closest(".ui-dialog")
                   .find(".ui-dialog-titlebar:first")
                   .css({
                       'background-color': '#3276B1',
                       'color': '#fff'
                   });

            },
            close: function () {
                $(this).remove();
            },
            buttons: {
                "Cerrar": function () {
                    $(this).dialog("close");
                }
            }
        }).prev(".ui-dialog-titlebar").css("background", "#22558D");
    },
    popupSI: function (title, msg, width) {

        $('<div id="popup" style="overflow-y:hidden;">' + msg + '</div>').dialog({
            modal: true,
            maxWidth: 650,
            maxHeight: 750,
            width: 650,
            height: 750,
            resizable: false,
            closeOnEscape: true,
            title: title,
            beforeClose: function (event, ui) { },
            hide: { effect: "fade", duration: 800 },
            open: function () {
                $(this)
                      .closest(".ui-dialog")
                      .find(".ui-dialog-titlebar-close:first")
                      .hide();
                $(this)
                   .closest(".ui-dialog")
                   .find(".ui-dialog-titlebar:first")
                   .css({
                       'background-color': '#3276B1',
                       'color': '#fff'
                   });

            },
            close: function () {
                $(this).remove();
            },
            //buttons: {
            //    "Enviar Nota": function () {
            //        mostrarDetalle();
            //        $(this).dialog("close");
            //    },
            //    "Cerrar": function () {
            //        $(this).dialog("close");
            //    }
            //}
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
    ,
    // formatea la fecha : dd/mm/yyy
    formatDate: function (d) {

        var dd = d.getDate();

        if (dd < 10) dd = '0' + dd;
        var mm = d.getMonth() + 1;
        if (mm < 10) mm = '0' + mm;
        //var yy = d.getFullYear() % 100
        //if ( yy < 10 ) yy = '0' + yy

        var fecha =
               {
                   dia: dd,
                   mes: mm,
                   anio: d.getFullYear(),
                   format: dd + '/' + mm + '/' + d.getFullYear()
               };

        return fecha;
    }
    ,
    // formatea cantidades numericas con (.) decimal
    addCommas: function (nStr) {
        nStr += '';
        x = nStr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    }
    ,
    SCIalert: function (msg, tagId) {

        $('#' + tagId).html('');
        $('#' + tagId).removeClass('hidden');
        $('#' + tagId).append(' <div class="col-md-10 col-md-offset-1">' +
            '<div class="alert alert-info" role="alert" id="_alert">' +
            '   <span class="glyphicon glyphicon-exclamation-sign" aria-hidden="true"></span>' +
            '  <span class="sr-only">Error:</span>' 
            +msg +
            '</div>' +
            '</div>');
    }
    ,
    SCIalertHidden: function (tagId) {

        $("#" + tagId).addClass('hidden');
    }
});