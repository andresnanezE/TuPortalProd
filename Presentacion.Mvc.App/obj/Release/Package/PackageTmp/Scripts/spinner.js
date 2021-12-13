jQuery.extend({
    show_wait: function (options) {
        var opc = {
            title: typeof (options.title) !== 'undefined' ? options.title : "Procesando",
            msg: options.msg,
        };

        setTimeout(function () { $('.modal').modal('hide'); }, 1000);

        setTimeout(function () {
            $('.modal-title').html(opc.title);
            $('.modal-msg-alert').html(opc.msg);
            $('#ajax-container').html('');
            $.addSpinner($('#ajax-container'));
            $('.footer-alert').hide();
            $('#modalWait').modal('toggle');
        }, 1000);
    },

    show_modal: function (options) {
        var opc = {
            title: typeof (options.title) !== 'undefined' ? options.title : "Procesando",
            msg: options.msg,
        };

        setTimeout(function () { $('.modal').modal('hide'); }, 1000);

        setTimeout(function () {
            $('.modal-title').html(opc.title);
            $('.modal-msg-alert').html(opc.msg);
            $("#alert-img").prop("src", "../Image/ok.png");
            $('.footer-alert').show();
            $('#modalAlert').modal('toggle');
        }, 2000);
    },

    show_error: function (options) {
        var opc = {
            title: typeof (options.title) !== 'undefined' ? options.title : "Procesando",
            msg: options.msg,
        };

        setTimeout(function () { $('.modal').modal('hide'); }, 1000);

        setTimeout(function () {
            $('.modal-title').html('Cargar imagenes banner');
            $('.modal-msg-alert').html(opc.msg);
            $("#alert-img").attr("src", "../Image/alerta.png");
            $('.footer-alert').show();
            $('#modalAlert').modal('toggle');
        }, 2000);
    },

    cerrar: function (options) {
        setTimeout(function () { $('.modal').modal('hide'); }, 1000);
    },

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