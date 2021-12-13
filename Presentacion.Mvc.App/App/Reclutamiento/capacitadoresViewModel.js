function capacitadoresViewModel() {

    var self = this;

    self.id = ko.observable();
    self.nombres = ko.observable();
    self.apellidos = ko.observable();
    self.idUsr = ko.observable();
    self.activo = ko.observable(true);
    self.capacitadores = ko.observableArray();

    self.tipo = ko.observable();
    self.textModal = ko.observable();
    self.titleModal = ko.observable();
    self.titleModal1 = ko.observable();

    self.gestionarAdicion = function () {

        self.id(undefined);
        self.idUsr(undefined);
        self.nombres(undefined);
        self.apellidos(undefined);

        self.tipo("Create");
        self.titleModal1("Agregar Capacitador");
        $("#mdl-form").modal("show");
    };

    self.gestionarEliminacion = function (id) {

        self.id(id);
        $("#mdl-delete").modal("show");
    };

    self.gestionarActualizacion = function (id) {

        self.id(id);
        self.tipo("Update");
        self.titleModal1("Actualizar Capacitador");

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerCapacitadoresAdmin/',
            data: { id: id },
            dataType: 'json',
            success: function (result) {

                self.idUsr(result[0].IdUsr);
                self.nombres(result[0].Nombres);
                self.apellidos(result[0].Apellidos);
                self.activo(result[0].Activo);

                $("#mdl-form").modal("show");

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });

    };

    self.eliminarCapacitador = function () {

        $.ajax({
            url: '/Reclutamiento/GestionarCapacitador?id=' + self.id() + '&idUsr=0&nombres=&apellidos=&activo=true&tipo=Delete',
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {

                if (data) {

                    $("#mdl-delete").modal("hide");

                    self.titleModal("Advertencia");
                    self.textModal("Se elimino el registro de forma exitosa.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");

                    self.obtenerCapacitadores();
                }

            },
            error: function () {
                self.titleModal("Advertencia");
                self.textModal("No es posible eliminar el capacitador en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
            }
        });

    };

    self.actualizarCapacitador = function () {

        $.ajax({
            url: '/Reclutamiento/GestionarCapacitador?id=' + self.id() + '&idUsr=' + self.idUsr() + '&nombres=' + self.nombres() + '&apellidos=' + self.apellidos() + '&activo=' + self.activo() + '&tipo=Update',
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {

                if (data) {

                    $("#mdl-form").modal("hide");

                    self.titleModal("Advertencia");
                    self.textModal("Se actualizo el registro de forma exitosa.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");

                    self.obtenerCapacitadores();
                }

            },
            error: function () {
                self.titleModal("Advertencia");
                self.textModal("No es posible actualizar el capacitador en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
            }
        });

    };

    self.agregarCapacitador = function () {

        $.ajax({
            url: '/Reclutamiento/GestionarCapacitador?id=0&idUsr=' + self.idUsr() + '&nombres=' + self.nombres() + '&apellidos=' + self.apellidos() + '&activo=' + self.activo() + '&tipo=Create',
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {

                if (data) {

                    $("#mdl-form").modal("hide");

                    self.titleModal("Advertencia");
                    self.textModal("Se agrego el capacitador de forma exitosa.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");

                    self.obtenerCapacitadores();
                }

            },
            error: function () {
                self.titleModal("Advertencia");
                self.textModal("No es posible agregar el capacitador en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
            }
        });

    };

    self.obtenerCapacitadores = function () {

        self.clearTable();

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerCapacitadoresAdmin/',
            data: { id: null },
            dataType: 'json',
            success: function (result) {

                self.capacitadores(result);
                self.drawTable();

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.clearTable = function () {

        $("#search-tbl-ctzn").val("");

        $("#dv-tb-ctzn").hide();
        $("#dv-loading-data").show();

        $("#rw-cnt-bsq").css("cursor", "wait");
        $("#dv-cnt-bsq").addClass("dsb-dv");

        self.id(undefined);
        self.idUsr(undefined);
        self.nombres(undefined);
        self.apellidos(undefined);

        var table = $("#tb-ctzn").DataTable();
        table.clear().draw();

        $("#tb-ctzn").dataTable().fnDestroy();
    };

    self.drawTable = function () {

        $('#tb-ctzn').DataTable({
            processing: true,
            "lengthChange": false,
            dom: 'Bfrtip',
            buttons: [{
                extend: 'excelHtml5'
                , text: '<i class="fa fa-file-excel-o" aria-hidden="true"></i> Descargar Excel'
                , className: 'btn-export-xsl'
                , autoFilter: true
                , titleAttr: 'Descargar'
                , sheetName: 'Data Cargue'
            }],
            initComplete: function () {

                $(".dv-search div").remove();
                $("#tb-ctzn_filter").detach().appendTo('.dv-search');

                $("#tb-ctzn_filter label").css("width", "100%");
                $("#tb-ctzn_filter label input")
                    .addClass("form-control")
                    .attr("placeholder", "Parámetro de búsqueda");


                $("#dv-loading-data").hide();
                $("#dv-tb-ctzn").fadeIn();
                $("#search-tbl-ctzn").prop("disabled", false);
            },
            language: {
                "lengthMenu": "Listar _MENU_",
                "zeroRecords": "Nada encontrado - lo siento",
                "info": "Mostrando página _PAGE_ de _PAGES_",
                "infoEmpty": "No hay registros disponibles",
                "infoFiltered": "(filtered from _MAX_ total records)",
                "sSearch": "",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                }
            },
            scrollY: "300px",
            ordering: false,
            columnDefs: [
                { width: "60", targets: 0 },
                { width: "300", targets: 1 },
                { width: "300", targets: 2 },
                { width: "300", targets: 3 }
            ]
        });

        $("#rw-cnt-bsq").css("cursor", "");
        $("#dv-cnt-bsq").removeClass("dsb-dv");

        $("#dv-loading-data").hide();
        $("#dv-tb-ctzn").show();
    };
}

var capacitadoresVM = new capacitadoresViewModel();
ko.applyBindings(capacitadoresVM);

$(document).ready(function () {
    capacitadoresVM.obtenerCapacitadores();
});