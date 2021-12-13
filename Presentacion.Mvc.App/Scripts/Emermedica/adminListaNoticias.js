(function (adminListaNoticias, $, undefined) {
    var noticiaId;
    var grid;

    $(document).ajaxStop(function () {
        AjaxUpdateProgressHide();
    });

    //Load the grid
    cargarData();

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // FUNCIONES PRIVADAS
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function cargarData() {
        AjaxUpdateProgressShow();

        if (grid == null) {
            grid = $("#noticias-grid").kendoGrid
                ({
                    dataSource:
                    {
                        type: "json",
                        transport:
                        {
                            read:
                            {
                                url: urlTraerNoticias,
                                type: "POST",
                                data:
                                {
                                    soloActivas: false
                                }
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
                                    NOTICIAID: { type: "string" },
                                    TITULO: { type: "string" },
                                    DESCRIPCION: { type: "string" },
                                    FECHA: { type: "date" },
                                    ACTIVO: { type: "boolean" },
                                }
                            }
                        },
                        sort: { field: "FECHA", dir: "asc" },
                        pageSize: 10,
                        serverPaging: true,
                        serverFiltering: false,
                        serverSorting: true
                    },
                    filterable: false,
                    sortable: false,
                    pageable: true,
                    pageable:
                    {
                        messages:
                        {
                            display: "{1} de {2} registros",
                            empty: "No hay registros que mostrar",
                            page: "Página",
                            of: "de",
                            itemsPerPage: "registros por página",
                            first: "Ir a la primera página",
                            previous: "Ir a la página anterior",
                            next: "Ir a la página siguiente",
                            last: "Ir a la última página",
                            refresh: "Refrescar"
                        }
                    },
                    columns:
                        [
                            {
                                field: "Edit",
                                title: "Editar",
                                width: "80px",
                                template: kendo.template($("#noticias-grid-edit-template").html())
                            },
                            {
                                field: "Delete",
                                title: "Eliminar",
                                width: "80px",
                                template: kendo.template($("#noticias-grid-delete-template").html())
                            },
                            {
                                field: "Preview",
                                title: "Previsualizar",
                                width: "80px",
                                template: kendo.template($("#noticias-grid-preview-template").html())
                            },
                            {
                                field: "TITULO",
                                title: "Titulo"
                            },
                            //{
                            //    field: "IMAGEN",
                            //    title: "Imagen",
                            //    template: kendo.template($("#noticias-grid-image-template").html())
                            //},
                            {
                                field: "DESCRIPCION",
                                title: "Descripción",
                                template: kendo.template($("#noticias-grid-descripcion-template").html())
                            },
                            {
                                field: "FECHA",
                                title: "Fecha",
                                template: kendo.template($("#noticias-grid-date-template").html())
                            },
                            {
                                field: "ACTIVO",
                                title: "Activo",
                                template: kendo.template($("#noticias-grid-check-template").html())
                            }
                        ],
                    dataBound: function (e) {
                        var wp = e.sender.wrapper;

                        //Imagen
                        wp.find(".noticias-image-tg").each
                            (
                                function () {
                                    var tg = $(this);
                                    tg.kendoTooltip
                                        ({
                                            content: tg.find(".noticias-image").html(),
                                            position: "top"
                                        });
                                }
                            )

                        //Editar Noticia
                        wp.find(".editar-noticia").unbind("click");
                        wp.find(".editar-noticia").click
                            (
                                function () {
                                    window.location = urlEditarNoticia + "/" + $(this).data("id");
                                }
                            );

                        //Eliminar Noticia
                        wp.find(".eliminar-noticia").unbind("click");
                        wp.find(".eliminar-noticia").click
                            (
                                function () {
                                    noticiaId = $(this).data("id");
                                    confirmMessage("Confirmación", deleteMessage, null, borrarNoticia);
                                }
                            );

                        //Previsualizar Noticia
                        wp.find(".previsualizar-noticia").unbind("click");
                        wp.find(".previsualizar-noticia").click
                            (
                                function () {
                                    window.location = urlPrevisualizarNoticia + "/" + $(this).data("id");
                                }
                            );
                    }
                }).data("kendoGrid");
        }
        else {
            grid.dataSource.read();
        }
    }

    function borrarNoticia() {
        $.ajax
            ({
                type: 'POST',
                url: urlEliminarNoticia,
                data:
                {
                    id: noticiaId
                },
                dataType: "json",
                beforeSend: function () {
                    AjaxUpdateProgressShow();
                },
                success: function (data) {
                    if (data.Success) {
                        informationMessage("Información", "La noticia ha sido eliminado  exitosamente", cargarData);
                    }
                    else {
                        errorMessage("Error", "Se presento un problema guardando la información. Por favor trate nuevamente");
                    }
                }
            });
    }
}(window.adminListaNoticias = window.adminListaNoticias || {}, jQuery));