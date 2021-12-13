$(function () {

    var _URL = window.URL || window.webkitURL;

    $('body').on('click', '.glyphicon-remove', function () {
        $(this).html('');
    });

    $("#enviar").prop("disabled", true);

    $('.fileup').on('click', function () {

        $('#confirm-delete').modal('toggle');

    });

    $('body').on('change', '.input-file', function (e) {

        var reader = {};
        var $file = $(this);
        var id = $file.attr('id');
        var pos = $file.val().lastIndexOf("\\");

        var nobreArchivo = $file.val().substr(pos + 1, $file.val().length - pos);


        reader = new FileReader();
        $file.parent().parent().next().html("");
        $file.parent().parent().next().next().html("");

        $('.glyphicon-remove').hover(function () {
            $(this).css('cursor', 'pointer');
        });


        file = $file.get(0).files[0];

        var file, img;


        if ((file = this.files[0])) {


            img = new Image();
            img.onload = function () {

                //if (this.width == 4750 && this.height == 542) {
                if (1==1) {
                    reader.readAsDataURL(file);
                    reader.onload = function () {

                        $file.data("data", {
                            nombre: id + ".jpg",
                            fileData: reader.result,
                            ruta: "",
                            KeyConfig: "HeadBanners",
                            Extension: "jpg",
                            labels: "",
                            BorrarActuales: false
                        });

                        $("#enviar").prop("disabled", false);
                        $file.parent().parent().next().html(nobreArchivo);
                        $file.parent().parent().next().next().
                            append("<span class='glyphicon glyphicon-remove navbar-brand'></span>");

                    };
                    reader.onerror = function (error) {
                        console.log('Error: ', error);
                    };
                }
                else {

                    $file.val("");

                    setTimeout(function () {
                        $('.modal-title').html('Cargar imagenes banner');
                        $('.modal-msg-alert').html('El tamaño de la imagen debe ser de 4750 x 542 px.'
                            + '</b>');
                        $("#alert-img").attr("src", "../Image/alerta.png");
                        $('#modalAlert').modal('toggle');
                        $('.footer-alert').show(); $("#enviar").prop("disabled", true);
                    }, 800);

                }
            };
            img.onerror = function () {
                alert("not a valid file: " + file.type);
            };
            img.src = _URL.createObjectURL(file);


        }


    });


    $('body').on('click', '.delete', function (e) {

        $("input#" + $(this).data("id")).val("");

        $("td#" + $(this).data("id") + "td").html("");
        $(this).html("");

        if (filesData().length > 0) {
            $("#enviar").prop("disabled", false);
        }
        else {
            $("#enviar").prop("disabled", true);
        }

    });


    $('.lst-menu').each(function () {

        var btn = $(this).attr('value').trim();

        btn = btn.replace(' ', '_');

        $('tbody').append('<tr><td>' + $(this).attr('value').trim()
            + '</td><td> <input type="file" class="input-file btn-xs" name="Archivos" id="'
            + btn + '"></td><td class = "filename" id="' + btn + 'td"></td><td class="delete" data-id="' + btn + '">&nbsp;</td></tr>');


        $('.glyphicon-remove').hover(function () {
            $(this).css('cursor', 'pointer');
        });


        $('#' + btn).filer({
            limit: 1,
            maxSize: 1,
            extensions: ["jpg"],
            addMore: false
        });

        console.log($(this).get(0).innerText.trim());
    });

});


function send() {


    var files = [];


    $("#enviar").prop("disabled", true);

    $.show_wait({ title: "Procesando.", msg: "Subiendo imagenes..." });


    setTimeout(function () {

        $(':file').each(function () {

            file = $(this).get(0).files[0];

            if (typeof (file) != 'undefined') {

                files.push($(this).data('data'));
            }

        });

        var imgNoSuccess = _ajax(files);

        if (imgNoSuccess.length <= 0) {


            $("input:file").val('');
            $('.filename').html('');
            $('.delete').html('');

            $.show_modal({ title: "Cargar imagenes banner.", msg: "Los archivos se han cargado con éxito." });

        }
        else {

            $.show_error({
                title: "Cargar imagenes banner.", msg: "Ocurrio algo inesperado subiendo algún(os) archivo(s):</br><b>"
                        + imgNoSuccess.join() + "</b>"
            });
        }

    }, 2000);


    




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

    var imgNoSuccess = [];


   

    for (var d in data) {

        $.ajax({
            type: "POST",
            url: $("#actionupload").val(),
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ 'data': data[d] }),
            dataType: "json",
            async: false
        }).success(function (data) {

            $('#modalAlert').modal('hide');

            if (typeof (data.msgError) !== 'undefined') {

                console.log(data.msgErrorException);
                imgNoSuccess.push(data.fileName);

            }


        }).fail(function () {

            imgNoSuccess.push(data[d].nombre);
        });


    }// fin for    


    return imgNoSuccess;

}


