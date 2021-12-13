/// Febrero 2017
// John Nelson Rodriguez.

jQuery.extend({
    descargarreporte: function (_options) {

        var options =
        {
            title_wait: typeof (_options.title_wait) !== 'undefined' ? _options.title_wait : "Procesando",
            msg_wait: typeof (_options.msg_wait) !== 'undefined' ? _options.msg_wait : 'Espera un momento...',
            title_modal: typeof (_options.title_wait) !== 'undefined' ? _options.title_wait : "Importante.",
            msg_modal: typeof (_options.msg_wait) !== 'undefined' ? _options.msg_body : 'No es posible mostrar el reporte en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con en área de tecnología.',
            confirm: typeof (confirm) != 'undefined' ? _options.confirm : '',
            confirm_html: _options.confirm_htm,

            action_report: _options.action_report,
            action_download: _options.action_download,
            contenttype: _options.contenttype
        };

        if (options.confirm === 'mostrar') {
            //debugger;
            if ($('#confirm-delete') != 'undefined') {
                $('#confirm-delete').modal('toggle');
                return;
            }
        }

        if (options.confirm === 'facturas') {
            //debugger;
            if ($('#facturas') != 'undefined') {
                $('#facturas').modal('toggle');

                return;
            }
        }

        $('.footer-alert').hide();
        $('.modal-title').html(options.title_wait);
        $('.modal-msg-alert').html(options.msg_wait);
        $('#modalAlert').modal('toggle');

        var frmValues = $('form').serializeArray()
            .filter(function (elem) {
                return $.trim(elem.value) != "";
            });

        $('#ajax-container').html('');

        $.addSpinner($('#ajax-container'));
        console.log('antes ajax');
        console.log(options.action_report);
        console.log("OPTIONS ", options);

        $.ajax({
            url: options.action_report,
            type: 'POST',
            data: frmValues,
            success: function (data) {
                console.log('peticion ok');
                $.removeSpinner($('#ajax-container'), function () {
                    $('#ajax-container').html('');
                });

                if (typeof (data.msgError) !== 'undefined' ||
                    (options.action_download == null || data.FileGuid == null)) {
                    console.log('error data ajax');
                    $('#modalAlert').modal('hide');

                    console.log(data.msgErrorException != null ? data.msgErrorException : "ation y/o guid no estan definidos.");

                    setTimeout(function () {
                        $('.modal-title').html(options.title_modal);

                        $('.modal-msg-alert').html(data.msgError != null
                            ? data.msgError : options.msg_modal);

                        $('#modalAlert').modal('toggle');
                        $('.footer-alert').show();
                    }, 1000);

                    return;
                }

                $('#modalAlert').modal('hide');
                console.log('antes windows.location');
                window.location.href = options.action_download
                    + '?fileGuid=' + data.FileGuid
                    + '&filename=' + data.FileName
                    + '&contentType=' + options.contenttype;

                console.log('despues windows.location');

                setTimeout(function () {
                    $('.modal-title').html('Reporte.');
                    $('.modal-msg-alert').html('Reporte descargado.<span>&#129151</span>');
                    $('#modalAlert').modal('toggle');
                    $('.footer-alert').show();
                }, 1000);
            },
            fail: function (datam) {
                $('#modalAlert').modal('hide');

                setTimeout(function () {
                    $('.modal-title').html(options.title_modal);
                    $('.modal-msg-alert').html(datam.msgError);
                    $('#modalAlert').modal('toggle');
                    $('.footer-alert').show();
                }, 1000);
            }
        });
    },

    /*
        Spinner:
    */

    addSpinner: function (el, static_pos) {
        var spinner = el.children('.spinner');
        if (spinner.length && !spinner.hasClass('spinner-remove')) return null;
        !spinner.length && (spinner = $('<div class="spinner' + (static_pos ? '' : ' spinner-absolute') + '"/>').appendTo(el));
        $.animateSpinner(spinner, 'add');
    },

    removeSpinner: function (el, complete) {
        var spinner = el.children('.spinner');
        spinner.length && $.animateSpinner(spinner, 'remove', complete);
    },

    animateSpinner: function (el, animation, complete) {
        if (el.data('animating')) {
            el.removeClass(el.data('animating')).data('animating', null);
            el.data('animationTimeout') && clearTimeout(el.data('animationTimeout'));
        }
        el.addClass('spinner-' + animation).data('animating', 'spinner-' + animation);
        el.data('animationTimeout', setTimeout(function () { animation == 'remove' && el.remove(); complete && complete(); }, parseFloat(el.css('animation-duration')) * 1000));
    }
});