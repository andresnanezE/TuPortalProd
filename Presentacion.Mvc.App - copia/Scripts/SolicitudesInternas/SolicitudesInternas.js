///-- JOhn Nelson Rodriguez
///-- Validación valores formulario envio de solicitud.
///-- Enero 2017
$(function () {



    $('#_alert').hide();

    $('#Attach').filer({
        showThumbs: true,
        limit: 3,
        maxSize: 3,
        extensions: ["jpg", "png", "pdf", "xls", "xlsx", "doc", "docx"],
        addMore: true,
        allowDuplicates: false
    });





    //$.SCIalertHidden('ialert');

    //if ($('#hMensaje').val().length > 0) {
    //    $.SCIalert($('#hMensaje').val(), 'ialert');

    //    $('#Ciudad_idCiudad').val("0");
    //    $('#TipoRequerimiento_idTRequerimiento').val("0");
    //    $('#Area_idArea').val("0");
    //    $("#Asunto").val('');
    //    $("#Descripcion").val('');
    //} else {
    //    $.SCIalertHidden('ialert');
    //}

    $("#Asunto, #Descripcion, #Area_idArea, #Ciudad_idCiudad, #TipoRequerimiento_idTRequerimiento")
       .on('click', function () {
           $.SCIalertHidden('ialert');
       });

});

function validar() {

    var enviar = true;

    if ($("#Descripcion").val().length > 300) {
        var error = new PNotify({
            title: 'Mensaje',
            text: 'Longitud maxima permitida 300 caractéres.',
            type: 'error'
        });
        enviar = false;
    }

    if ($("#Asunto").val() == "" || $('#Descripcion').val() == "" || $('#Area_idArea').val() == "0" ||
        $('#Ciudad_idCiudad').val() == "0" || $('#TipoRequerimiento_idTRequerimiento').val() == "0") {
        var error = new PNotify({
            title: 'Mensaje',
            text: 'Los campos Asunto, Ciudad, Área, Tipo  de Requerimiento y Descripción son necesarios.',
            type: 'error'
        });
        enviar = false;
        $.SCIalertHidden('ialert');
    }

    if (enviar) {

        $('#enviar, #btn-remove').prop("disabled", true);

        $('#_alert').show();
        $('#ainfo').append('<span> &nbsp &nbsp &nbsp &nbspEnviando, espera un momento ...</span>');

        $('form').submit();
    }

}