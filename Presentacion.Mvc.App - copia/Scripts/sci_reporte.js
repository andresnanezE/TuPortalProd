/// John Nelson Rodriguez
// Febrero 2017
$(function () {
    $('.reporte, .reporte-com-pend').on('click', function () {

        $.descargarreporte(
            {

                action_report: $(this).data('action'),
                action_download: $('#actiondownload').val(),
                confirm: $(this).data('confirm'),
                confirm_html: '<b>SCI</b>',
                contenttype: $(this).data('contenttype')

            });



    });
});

//.N. Nicolas Sarmiento .N. 
// Se implementa de esta manera por tiempos .

function mostarModalAceptacionTerminosDescarga(accion,f){
    $('#confirmar-descarga-reporte .modal-footer #btnSi').attr('onclick',"javascript:" + accion + "()");
    //debugger;
    if ($('#confirmar-descarga-reporte') != 'undefined')
    {
        $('#confirmar-descarga-reporte').modal('toggle');
        return;
    }
}