$(function () {


    $('#_alert').hide();

    $('#Archivos').filer({
        showThumbs: true,
        limit: 1,
        maxSize: 3,
        extensions: ["csv"],
        addMore: false,
        allowDuplicates: false
    });

    $('#Archivos').on('click', function () {
        $('.alert-head').hide();
    });

});

$('[data-toggle="popover"]').popover();

function validar() {

    if ($('#Archivos').val().length > 0) {


        $('#enviar, #btn-remove').prop("disabled", true);

        $('#_alert').show();
        $("#usuario-form").submit();
    }




}