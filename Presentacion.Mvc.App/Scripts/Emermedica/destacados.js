(function (destacados, $, undefined)
{
    var grid;
    var validator;

    $(document).ajaxStop(function ()
    {
        AjaxUpdateProgressHide();
    });

    kendo.data.binders.checkedbool = kendo.data.Binder.extend
    ({
        init: function (element, bindings, options)
        {
            //Call the base constructor
            kendo.data.Binder.fn.init.call(this, element, bindings, options);

            var that = this;
            $(that.element).on("change", function ()
            {
                that.change();
            });
        },
        refresh: function ()
        {
            var that = this;
            var value = that.bindings["checkedbool"].get();
            var parent = $(that.element).closest(".btn");
            if (parent.length > 0)
            {
                parent.removeClass("btn-danger").addClass("btn-default");
            }
            if (value !== undefined)
            {
                if (value == null)
                {
                    value = "null";
                }

                if ($(that.element).val().toLowerCase() == value.toString().toLowerCase())
                {
                    $(that.element).prop('checked', true);
                    if (parent.length > 0)
                    {
                        parent.removeClass("btn-default").addClass("btn-danger");
                    }
                }
            }
            else
            {
                $(that.element).prop('checked', false);
            }
        },
        change: function ()
        {
            var value = this.element.value.toLowerCase();
            if (value === "null")
            {
                this.bindings["checkedbool"].set(null);
            }
            else
            {
                this.bindings["checkedbool"].set(value === "true");
            }
        }
    });

    //View model
    destacados.viewModel = kendo.observable
    ({
        destacado1:
        {
            DESTACADOID: 1,
            IMAGEN: "",
            URL: "",
            ABRIRENSITIO: false
        }, 
        destacado2:
        {
            DESTACADOID: 2,
            IMAGEN: "",
            URL: "",
            ABRIRENSITIO: false
        }, 
        destacado3:
        {
            DESTACADOID: 3,
            IMAGEN: "",
            URL: "",
            ABRIRENSITIO: false
        },
        imageRaw1: null,
        imageRaw2: null,
        imageRaw3: null,
        bannerId: '',
        bannerImage: '',
        bannerUrl: '',
        bannerPosicion: 0,
        bannerImageRaw: null,
        getImage: function (i)
        {
            var destacado = this.get("destacado" + i);
            if (destacado.IMAGEN)
            {
                return urlDestacadosPath + destacado.IMAGEN;
            }
            else
            {
                return urlDefaultDestacadoImage;
            }
        },
        getBannerImage: function ()
        {
            var img = this.get("bannerImage");
            if (img)
            {
                return urlBannersPath + img;
            }
            else
            {
                return urlDefaultBannerImage;
            }
        },
        cancel: function ()
        {
            var kw = $("#banner-window").data("kendoWindow");
            kw.close();
        }
    });
    kendo.bind($("#destacados-banners-container"), destacados.viewModel);
        
    //Carga la data
    traerDestacados();
    cargarData();

    $("#destacados-form").kendoValidator
    ({
        messages: kendoValidationMessage
    }).data("kendoValidator");

    $("#banner-form").kendoValidator
    ({
        messages: kendoValidationMessage
    }).data("kendoValidator");

    $("#add-banner").click
    (
        function () {
            //Model data
            destacados.viewModel.set("bannerId", null);
            destacados.viewModel.set("bannerUrl", "");
            destacados.viewModel.set("bannerPosicion", 0);
            destacados.viewModel.set("bannerImage", null);
            destacados.viewModel.set("bannerImageRaw", null);
            var kw = $("#banner-window").data("kendoWindow");
            kw.title("Adicionar Banner");
            kw.center();
            kw.open();
        }
    );

    $("#destacados-form").ajaxForm
    ({
        dataType: 'json',
        data: {},
        beforeSubmit: function ()
        {
            AjaxUpdateProgressShow();
        },
        success: function (data)
        {
            if (data.Success)
            {
                informationMessage("Información", "Los destacados han sido salvados exitosamente");
            }
            else
            {
                errorMessage("Error", "Se presento un problema guardando la información. Por favor trate nuevamente");
            }
        },
        error: function ()
        {
            errorMessage("Error", "Se presento un problema guardando la información. Por favor trate nuevamente");
        }
    });

    $("#banner-form").ajaxForm
    ({
        dataType: 'json',
        data: {},
        beforeSubmit: function ()
        {
            if ((!destacados.viewModel.get("bannerId") && !destacados.viewModel.get("bannerImageRaw")) || (destacados.viewModel.get("bannerId") && !destacados.viewModel.get("bannerImage") && !destacados.viewModel.get("bannerImageRaw")))
            {
                errorMessage("Error", "Debe selecionar una imagen para el banner");
                return false;
            }
            AjaxUpdateProgressShow();
        },
        success: function (data)
        {
            if (data.Success)
            {
                informationMessage("Información", "El banner ha sido salvado exitosamente", operationSuccessful);
            }
            else
            {
                errorMessage("Error", "Se presento un problema guardando la información. Por favor trate nuevamente");
            }
        },
        error: function ()
        {
            errorMessage("Error", "Se presento un problema guardando la información. Por favor trate nuevamente");
        }
    });


    //Ku Destacado
    $("input.image-destacado, input.image-banner").each
    (
        function ()
        {
            var key = $(this).attr("name");
            var type = $(this).data("type");
            var previewItem = $(this).data("preview-item");
            var id = $(this).data("id");
            var url = urlUploadImage + "?fileKey=" + key + "&imageType=" + type;

            $(this).kendoUpload
            ({
                multiple: false,
                async:
                {
                    saveUrl: url,
                    autoUpload: true
                },
                select: function (e)
                {
                    var files = e.files;
                    var success = true;
                    // Check the extension of each file and abort the upload if it is not .jpg|.gif|.png
                    $.each(files, function ()
                    {
                        if (!(this.extension.toLowerCase() == ".jpg" || this.extension.toLowerCase() == ".gif" || this.extension.toLowerCase() == ".png")) {
                            informationMessage("Información", "Solo archivos con extension .gif, .jpg o .png son permitidos. Inténtelo nuevamente.");
                            e.preventDefault();
                            success = false;
                        }
                        if (this.size > 1500000)
                        {
                            informationMessage("Información", "Por favor elija una imagen de maximo 160KB. Inténtelo nuevamente.");
                            e.preventDefault();
                            success = false;
                        }
                    });

                    if (success)
                    {
                        AjaxUpdateProgressShow();
                    }
                },
                success: function (e)
                {
                    if (e.response.Success)
                    {
                        $(previewItem).attr("src", e.response.Preview);

                        if (type == imageTypeDestacado)
                        {
                            //var destacado = destacados.viewModel.get("destacado" + id);
                            //destacado.imageRaw = e.response.Original;

                            //destacados.viewModel.set("destacado" + id, destacado);

                            destacados.viewModel.set("imageRaw" + id, e.response.Original);
                        }
                        else
                        {
                            destacados.viewModel.set("bannerImageRaw", e.response.Original);
                        }
                    }
                    else
                    {
                        errorMessage("Error", "Lo sentimos, no fue posible cargar la foto. Inténtelo nuevamente.");
                    }
                    AjaxUpdateProgressHide();
                }
            });
        }
    );

    //Uplaod avatar destacado
    $(".upload-image-destacado-tg").click
    (
        function ()
        {
            var id = $(this).data("id");
            $(this).parent().parent().find("input[name='image-destacado-" + id + "']").click();
        }
    );

    //Uplaod avatar
    $("#upload-image-banner-tg").click
    (
        function ()
        {
            $("input[name='image-banner']").click();
        }
    );

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // FUNCIONES PRIVADAS
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function operationSuccessful()
    {
        var kw = $("#banner-window").data("kendoWindow");
        kw.close();
        cargarData();
    }

    function traerDestacados()
    {
        $.ajax
        ({
            type: 'POST',
            url: urlTraerDestacados,
            data: { },
            dataType: "json",
            beforeSend: function ()
            {
                AjaxUpdateProgressShow();
            },
            success: function (data)
            {
                //Destacado 1
                var destacado1 = _.findWhere(data, { DESTACADOID: 1 });
                if (destacado1)
                {
                    destacado1.imageRaw = null;
                    destacados.viewModel.set("destacado1", destacado1);
                }

                //Destacado 2
                var destacado2 = _.findWhere(data, { DESTACADOID: 2 });
                if (destacado2)
                {
                    destacado2.imageRaw = null;
                    destacados.viewModel.set("destacado2", destacado2);
                }

                //Destacado 3
                var destacado3 = _.findWhere(data, { DESTACADOID: 3 });
                if (destacado3)
                {
                    //destacado3.imageRaw = null;
                    destacados.viewModel.set("destacado3", destacado3);
                }
                
            },
            error: function (request, status, error)
            {
                alert(request.responseText);
            }
        });
    }

    function cargarData()
    {
        AjaxUpdateProgressShow();

        if (grid == null)
        {
            grid = $("#banners-grid").kendoGrid
            ({
                dataSource:
                {
                    type: "json",
                    transport:
                    {
                        read:
                        {
                            url: urlTraerBanners,
                            type: "POST"
                        }
                    },
                    schema:
                    {
                        data: "data",
                        total: "total",
                        model:
                        {
                            fields:
                            {
                                BANNERID: { type: "string" },
                                IMAGEN: { type: "string" },
                                URL: { type: "string" },
                                POSICION: { type: "int" },
                            }
                        }
                    },
                    sort: { field: "POSICION", dir: "asc" },
                    serverPaging: false,
                    serverFiltering: false,
                    serverSorting: false
                },
                filterable: false,
                sortable: false,
                pageable: false,
                columns:
                [
                    {
                        field: "Delete",
                        title: "Eliminar",
                        width: "50px",
                        template: kendo.template($("#banners-grid-delete-template").html())
                    },
                    {
                        field: "Edit",
                        title: "Editar",
                        width: "50px",
                        template: kendo.template($("#banners-grid-edit-template").html())
                    },
                    {
                        field: "IMAGEN",
                        title: "Imagen",
                        width: "50px",
                        template: kendo.template($("#banners-grid-image-template").html())
                    },
                    {
                        field: "URL",
                        title: "Url",
                        width: "400px"
                    },
                    {
                        field: "POSICION",
                        title: "Posición",
                        width: "210px"
                    }
                ],
                dataBound: function (e) {
                    var wp = e.sender.wrapper;
                    wp.find(".banner-image-tg").each
                    (
                        function ()
                        {
                            var tg = $(this);
                            tg.kendoTooltip
                            ({
                                content: tg.find(".banner-image").html(),
                                position: "top"
                            });

                        }
                    )
                    wp.find(".delete-banner").unbind("click");
                    wp.find(".delete-banner").click
                    (
                        function ()
                        {
                            //Model data
                            destacados.viewModel.set("bannerId", $(this).data("id"));
                            confirmMessage("Confirmación", deleteMessage, null, deleteItem);
                        }
                    );
                    wp.find(".edit-banner").unbind("click");
                    wp.find(".edit-banner").click
                    (
                        function () {
                            //Model data
                            destacados.viewModel.set("bannerId", $(this).data("id"));
                            EditItem();
                        }
                    );
                }
            }).data("kendoGrid");
        }
        else {
            grid.dataSource.read();
        }
    }

    function EditItem()
    {
        var id = destacados.viewModel.get("bannerId")
        $.ajax
        ({
            type: 'POST',
            url: urlTraerBanner,
            data:
            {
                bannerId: id
            },
            dataType: "json",
            beforeSend: function ()
            {
                AjaxUpdateProgressShow();
            },
            success: function (data)
            {

                destacados.viewModel.set("bannerImage", data.IMAGEN);
                destacados.viewModel.set("bannerUrl", data.URL);
                destacados.viewModel.set("bannerPosicion", data.POSICION);
                destacados.viewModel.set("bannerImageRaw", null);
                var kw = $("#banner-window").data("kendoWindow");
                kw.title("Editar el Banner");
                kw.center();
                kw.open();
            }
        });
    }

    function deleteItem()
    {
        var id = destacados.viewModel.get("bannerId")
        $.ajax
        ({
            type: 'POST',
            url: urlEliminarBanner,
            data:
            {
                id: id
            },
            dataType: "json",
            beforeSend: function ()
            {
                AjaxUpdateProgressShow();
            },
            success: function (data)
            {
                if (data.Success)
                {
                    informationMessage("Información", "El banner ha sido eliminado.", operationSuccessful);
                }
                else
                {
                    errorMessage("Error", "Se presento un problema guardando la información. Por favor trate nuevamente");
                }
            }
        });
    }

}(window.destacados = window.destacados || {}, jQuery));