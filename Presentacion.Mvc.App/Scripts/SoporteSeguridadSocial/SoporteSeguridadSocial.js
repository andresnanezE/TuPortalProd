$(function () {
    $('#_alert').hide();

    $('#Archivos').filer({
        showThumbs: true,
        limit: 3,
        maxSize: 3,
        extensions: ["jpg", "png", "pdf"],
        addMore: true,
        allowDuplicates: false
    });

    $('#ID_MOTIVO, #OBSERVACION, #Archivos').on('click', function () {
        $('.alert-head').hide();
    });
});

$('[data-toggle="popover"]').popover();

function validar() {
    var submit = true;

    if ($("#OBSERVACION").val().length > 100) {
        var error = new PNotify({
            title: 'Mensaje',
            text: 'Longitud maxima permitida 100 caractéres.',
            type: 'error'
        }); submit = false;
    }
    if ($("#OBSERVACION").val() == "" || $('#ID_MOTIVO').val() == "" || $("#Archivos")[0].files.length <= 0) {
        var error = new PNotify({
            title: 'Mensaje',
            text: 'Es necesario seleccionar un motivo, agregar una observacón y adjuntar un archivo, para enviar esta solicitud.',
            type: 'error'
        }); submit = false;
    }
    if ($("#OBSERVACION").val().indexOf('<') >= 0 || $("#OBSERVACION").val().indexOf('>') >= 0) {
        var error = new PNotify({
            title: 'Mensaje',
            text: 'Hay carácteres no validos en Observación.',
            type: 'error'
        });
        submit = false;
    }

    if (submit) {
        $('#enviar, #btn-remove').prop("disabled", true);

        $('#_alert').show();
        $("#usuario-form").submit();
    }
}