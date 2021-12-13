(function (editarnoticia, $, undefined) {
    var validator;

    $(document).ajaxStop(function () {
        AjaxUpdateProgressHide();
    });

    kendo.data.binders.checkedbool = kendo.data.Binder.extend
        ({
            init: function (element, bindings, options) {
                //Call the base constructor
                kendo.data.Binder.fn.init.call(this, element, bindings, options);

                var that = this;
                $(that.element).on("change", function () {
                    that.change();
                });
            },
            refresh: function () {
                var that = this;
                var value = that.bindings["checkedbool"].get();
                var parent = $(that.element).closest(".btn");
                if (parent.length > 0) {
                    parent.removeClass("btn-danger").addClass("btn-default");
                }
                if (value !== undefined) {
                    if (value == null) {
                        value = "null";
                    }

                    if ($(that.element).val().toLowerCase() == value.toString().toLowerCase()) {
                        $(that.element).prop('checked', true);
                        if (parent.length > 0) {
                            parent.removeClass("btn-default").addClass("btn-danger");
                        }
                    }
                }
                else {
                    $(that.element).prop('checked', false);
                }
            },
            change: function () {
                var value = this.element.value.toLowerCase();
                if (value === "null") {
                    this.bindings["checkedbool"].set(null);
                }
                else {
                    this.bindings["checkedbool"].set(value === "true");
                }
            }
        });

    //kdp
    $("input[name='fecha']").kendoDatePicker();

    //wysiwyg
    $("#contenido").kendoEditor(
        {
            resizable:
            {
                content: false,
                toolbar: true
            },
            tools:
                [
                    "bold",
                    "italic",
                    "underline",
                    "strikethrough",
                    "justifyLeft",
                    "justifyCenter",
                    "justifyRight",
                    "justifyFull",
                    "insertUnorderedList",
                    "insertOrderedList",
                    "indent",
                    "outdent",
                    "createLink",
                    "unlink",
                    "insertImage",
                    //"insertFile",
                    "subscript",
                    "superscript",
                    "createTable",
                    "addRowAbove",
                    "addRowBelow",
                    "addColumnLeft",
                    "addColumnRight",
                    "deleteRow",
                    "deleteColumn",
                    "viewHtml",
                    "formatting",
                    "cleanFormatting",
                    "fontName",
                    "fontSize",
                    "foreColor",
                    "backColor",
                    "print"
                ],
            imageBrowser:
            {
                messages:
                {
                    dropFilesHere: "Agregue sus imagenes acá"
                },
                transport:
                {
                    read: urlLecturaImagenes,
                    destroy:
                    {
                        url: urlEliminarImagen,
                        type: "POST"
                    },
                    create:
                    {
                        url: urlCrearImagen,
                        type: "POST"
                    },
                    thumbnailUrl: urlThumbnail,
                    uploadUrl: urlSubirImageKendo,
                    imageUrl: urlConsultaArchivo
                }
            },
            //fileBrowser:
            //{
            //    messages:
            //    {
            //        dropFilesHere: "Agregue sus archivos acá"
            //    },
            //    transport:
            //    {
            //        read: "/kendo-ui/service/FileBrowser/Read",
            //        destroy:
            //        {
            //            url: "/kendo-ui/service/FileBrowser/Destroy",
            //            type: "POST"
            //        },
            //        create:
            //        {
            //            url: "/kendo-ui/service/FileBrowser/Create",
            //            type: "POST"
            //        },
            //        uploadUrl: "/kendo-ui/service/FileBrowser/Upload",
            //        fileUrl: "/kendo-ui/service/FileBrowser/File?fileName={0}"
            //    }
            //}
        });

    //View model
    editarnoticia.viewModel = kendo.observable
        ({
            noticiaId: '',
            titulo: '',
            descripcion: '',
            contenido: '',
            activo: false,
            fecha: '',
            imagen: '',
            imagenRaw: null,
            banner: '',
            bannerRaw: null,
            traerImagen: function () {
                var img = this.get("imagen");
                if (img) {
                    return urlImagenNoticia + img;
                }
                else {
                    return urlImagenPorDefecto;
                }
            },
            traerBanner: function () {
                var img = this.get("banner");
                if (img) {
                    return urlBanner + img;
                }
                else {
                    return urlBannerPorDefecto;
                }
            },
            traerFecha: function () {
                var d = parseDate(this.get("fecha"));
                if (d) {
                    $("input[name='fecha']").data('kendoDatePicker').value(convertDateToUTC(d));
                    return this.get("fecha");
                }
                else {
                    $("input[name='fecha']").data('kendoDatePicker').value("");
                    return "";
                }
            }
        });
    kendo.bind($("#noticia-container"), editarnoticia.viewModel);

    $("#noticia-form").kendoValidator
        ({
            messages: kendoValidationMessage
        }).data("kendoValidator");

    $("#noticia-form").ajaxForm
        ({
            dataType: 'json',
            data: {},
            beforeSubmit: function () {
                if ((!editarnoticia.viewModel.get("noticiaId") && !editarnoticia.viewModel.get("imagenRaw")) ||
                    (editarnoticia.viewModel.get("noticiaId") && !editarnoticia.viewModel.get("imagen") && !editarnoticia.viewModel.get("imagenRaw"))) {
                    errorMessage("Error", "Debe selecionar una imagen para la noticia");
                    return false;
                }
                AjaxUpdateProgressShow();
            },
            success: function (data) {
                if (data.Success) {
                    informationMessage("Información", "La noticia ha sido salvada exitosamente", operacionExitosa);
                }
                else {
                    errorMessage("Error", "Se presento un problema guardando la información. Por favor trate nuevamente");
                }
            },
            error: function () {
                errorMessage("Error", "Se presento un problema guardando la información. Por favor trate nuevamente");
            }
        });

    //Ku
    $("input[name='imagen'], input[name='banner']").each
        (
            function () {
                var key = $(this).attr("name");
                var previewItem = $(this).data("preview-item");
                var imageType = $(this).data("image-type");
                var rawItem = $(this).data("raw-item");
                var url = urlSubirImagen + "?fileKey=" + key + "&imageType=" + imageType;

                $(this).kendoUpload
                    ({
                        multiple: false,
                        async:
                        {
                            saveUrl: url,
                            autoUpload: true
                        },
                        select: function (e) {
                            var files = e.files;
                            var success = true;
                            // Check the extension of each file and abort the upload if it is not .jpg|.gif|.png
                            $.each(files, function () {
                                if (!(this.extension.toLowerCase() == ".jpg" || this.extension.toLowerCase() == ".gif" || this.extension.toLowerCase() == ".png")) {
                                    informationMessage("Información", "Solo archivos con extension .gif, .jpg o .png son permitidos. Inténtelo nuevamente.");
                                    e.preventDefault();
                                    success = false;
                                }
                                if (this.size > 1500000) {
                                    informationMessage("Información", "Por favor elija una imagen de maximo 160KB. Inténtelo nuevamente.");
                                    e.preventDefault();
                                    success = false;
                                }
                            });

                            if (success) {
                                AjaxUpdateProgressShow();
                            }
                        },
                        success: function (e) {
                            if (e.response.Success) {
                                $(previewItem).attr("src", e.response.Preview);
                                editarnoticia.viewModel.set(rawItem, e.response.Preview);
                            }
                            else {
                                errorMessage("Error", "Lo sentimos, no fue posible cargar la foto. Inténtelo nuevamente.");
                            }
                            AjaxUpdateProgressHide();
                        }
                    });
            }
        );

    //Uplaod avatar
    $("#subir-imagen-tg").click
        (
            function () {
                $("input[name='imagen']").click();
            }
        );

    //Uplaod avatar
    $("#subir-banner-tg").click
        (
            function () {
                $("input[name='banner']").click();
            }
        );

    //Carga los datos
    if (noticiaId) {
        editarnoticia.viewModel.set("noticiaId", noticiaId);
        traerNoticia();
    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // FUNCIONES PRIVADAS
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function operacionExitosa() {
        window.location = urlListaNoticias;
    }

    function traerNoticia() {
        $.ajax
            ({
                type: 'POST',
                url: urlTraerNoticia,
                data:
                {
                    id: noticiaId
                },
                dataType: "json",
                beforeSend: function () {
                    AjaxUpdateProgressShow();
                },
                success: function (data) {
                    editarnoticia.viewModel.set("titulo", data.TITULO);
                    editarnoticia.viewModel.set("descripcion", data.DESCRIPCION);
                    editarnoticia.viewModel.set("contenido", data.CONTENIDO);
                    editarnoticia.viewModel.set("activo", data.ACTIVO);
                    editarnoticia.viewModel.set("fecha", data.FECHA);
                    editarnoticia.viewModel.set("imagen", data.IMAGEN);
                    editarnoticia.viewModel.set("banner", data.BANNER);
                },
                error: function (request, status, error) {
                    alert(request.responseText);
                }
            });
    }
}(window.editarnoticia = window.editarnoticia || {}, jQuery));