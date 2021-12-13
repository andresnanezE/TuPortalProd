/// Enero 2017.
/// John Nelson Rodriguez G
/// Dependiendo desde la url que se acceda a Tu Portal, ...
/// ... en el menú Cotizador, crea un enlace para Interno o Externo.
$(function () {
    var interno = false;
    var $urlCotExterna = null;
    var $urlCotInterna = null;

    var location = window.location.hostname + ':' + window.location.port;

    if (location === $('#urlPortalPruebas').val() ||
        location === $('#urlPortalProduccion').val() ||
        location === $('#urlPortalDesarrollo').val()
    ) { interno = true; }

    $('a').each(function () {
        if ($(this).get(0).text === $('#descMenuExterno').val()) {
            $urlCotExterna = $(this);
        }
        if ($(this).get(0).text === $('#descMenuInterno').val()) {
            $urlCotInterna = $(this);
        }
    });

    if ($urlCotExterna != null && $urlCotInterna != null) {
        $urlCotExterna.attr('target', '_blank');
        $urlCotExterna.attr('href', $('#urlCotizadorExterna').val());

        $urlCotInterna.attr('target', '_blank');
        $urlCotInterna.attr('href', $('#urlCotizadorInterna').val());

        if (interno == true) {
            $urlCotExterna.hide();
        } else {
            $urlCotInterna.hide();
        }
    }
});