/*

    John Nelson Rodriguez
    Mayo 2017
    CargueCondiciones Plan de Vuelo

*/

$(function () {
    $(".fileUpload").filer({
        limit: 1,
        maxSize: 1,
        extensions: ["pdf"],
        addMore: false
    });

    $("#enviar").prop("disabled", true);

    $('.fileup').on('click', function () {
        $('#confirm-delete').modal('toggle');
    });

    $("body").on("change", ".fileUpload", function () {
        var reader = {};
        var $file = $(this);
        var id = $file.attr('id');
        var pos = $file.val().lastIndexOf("\\");

        var nobreArchivo = $file.val().substr(pos + 1, $file.val().length - pos);

        reader = new FileReader();
        $(this).parent().parent().next().html("");
        $(this).parent().parent().next().html(nobreArchivo);
        $(this).parent().parent().next().next().html("");
        $(this).parent().parent().next().next().append("<span class='glyphicon glyphicon-remove navbar-brand'></span>");

        $('.glyphicon-remove').hover(function () {
            $(this).css('cursor', 'pointer');
        });

        file = $file.get(0).files[0];

        reader.readAsDataURL(file);
        reader.onload = function () {
            $file.data("data", {
                nombre: nobreArchivo,
                fileData: reader.result.replace('data:application/pdf;base64,', '').trim(),
                ruta: "",
                KeyConfig: $file.data("path"),
                Extension: "pdf",
                labels: "",
                BorrarActuales: true
            });

            $("#enviar").prop("disabled", false);
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
        };
    });

    $(".delete").on("click", function () {
        $("input#" + $(this).attr("id")).val("");

        $(this).parent().children("td#" + $(this).attr("id") + ".filename").html("");
        $(this).parent().children("td#" + $(this).attr("id") + ".delete").html("");

        if (filesData().length > 0) {
            $("#enviar").prop("disabled", false);
        }
        else {
            $("#enviar").prop("disabled", true);
        }
    });
});

function send() {
    var f = filesData();

    if (f.length <= 0) {
        $('.modal-title').html("Gana tu PIM");

        $('.modal-msg-alert').html("No hay archivos pra subir aun.");

        $('#modalAlert').modal('toggle');
        $('.footer-alert').show();

        return;
    }

    _ajax(f);
}

function filesData() {
    var files = [];

    $(':file').each(function () {
        file = $(this).get(0).files[0];

        if (typeof (file) != 'undefined') {
            files.push($(this).data('data'));
        }
    });

    return files;
}

function _ajax(data) {
    data = JSON.stringify({ 'data': data });

    $('#ajax-container').html('');

    $.ajax({
        type: "POST",
        url: $("#actionupload").val(),
        contentType: "application/json; charset=utf-8",
        data: data,
        dataType: "json",
    }).success(function (data) {
        if (typeof (data.msgError) !== 'undefined') {
            $('#modalAlert').modal('hide');

            setTimeout(function () {
                $('.modal-title').html("Gana tu PIM.");
                $('.modal-msg-alert').html(data.msgError);
                $("#alert-img").prop("src", "../Image/alerta.png");
                $('#modalAlert').modal('toggle');
                $('.footer-alert').show();
            }, 1000);

            return;
        }

        $('#modalAlert').modal('hide');

        setTimeout(function () {
            $('.modal-title').html('Gana tu PIM.');
            $('.modal-msg-alert').html('Los archivos se han cargado con éxito.');
            $("#alert-img").prop("src", "../Image/ok.png");
            $('#modalAlert').modal('toggle');
            $('.footer-alert').show();
        }, 1000);
    }).fail(function (data) {
        $('#modalAlert').modal('hide');

        setTimeout(function () {
            $('.modal-title').html('Gana tu PIM');
            $('.modal-msg-alert').html('Ocurrio algo inesperado subiendo los archivos.');
            $("#alert-img").attr("src", "../Image/alerta.png");
            $('#modalAlert').modal('toggle');
            $('.footer-alert').show();
        }, 1000);
    });
}