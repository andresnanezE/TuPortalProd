function sucursalesViewModel() {

    var self = this;

    self.id = ko.observable();
    self.nombre = ko.observable();
    self.activo = ko.observable(true);
    self.ciudades = ko.observableArray();

    self.tipo = ko.observable();
    self.textModal = ko.observable();
    self.titleModal = ko.observable();
    self.titleModal1 = ko.observable();

    self.gestionarAdicion = function () {

        self.id(undefined);
        self.nombre(undefined);

        self.tipo("Create");
        self.titleModal1("Agregar Sucursal");
        $("#mdl-form").modal("show");
    };

    self.gestionarEliminacion = function (id) {

        self.id(id);
        $("#mdl-delete").modal("show");
    };

    self.gestionarActualizacion = function (id) {

        self.id(id);
        self.tipo("Update");
        self.titleModal1("Actualizar Sucursal");

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerSucursales/',
            data: { id: id },
            dataType: 'json',
            success: function (result) {

                self.nombre(result[0].Nombre);
                self.activo(result[0].Activo);

                $("#mdl-form").modal("show");

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });

    };

    self.eliminarSucursal = function () {

        $.ajax({
            url: '/Reclutamiento/GestionarSucursal?id=' + self.id() + '&nombre=&activo=true&tipo=Delete',
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

                    self.obtenerSucursales();
                }

            },
            error: function () {
                self.titleModal("Advertencia");
                self.textModal("No es posible eliminar la sucursal en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
            }
        });

    };

    self.actualizarSucursal = function () {

        $.ajax({
            url: '/Reclutamiento/GestionarSucursal?id=' + self.id() + '&nombre=' + self.nombre() + '&activo=' + self.activo() + '&tipo=Update',
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

                    self.obtenerSucursales();
                }

            },
            error: function () {
                self.titleModal("Advertencia");
                self.textModal("No es posible actualizar la sucursal en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
            }
        });

    };

    self.agregarSucursal = function () {

        if (self.nombre() === undefined || self.nombre() === '') {
            $("#nombre").addClass("error-vl");
            return;
        }


        $.ajax({
            url: '/Reclutamiento/GestionarSucursal?id=0&nombre=' + self.nombre() + '&activo=' + self.activo() + '&tipo=Create',
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {

                console.log("Edata ", data);

                if (data === true) {

                    $("#mdl-form").modal("hide");

                    self.titleModal("Advertencia");
                    self.textModal("Se agrego la sucursal de forma exitosa.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");

                    self.obtenerSucursales();
                }

                if (data === "Error") {
                    self.titleModal("Advertencia");
                    self.textModal("No es posible agregar la sucursal porque ya se encuentra registrada, por favor ingrese otro nombre.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                }

            },
            error: function () {
                self.titleModal("Advertencia");
                self.textModal("No es posible agregar la sucursal en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
            }
        });

    };

    self.obtenerSucursales = function () {

        self.clearTable();

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerSucursales/',
            data: { id: null },
            dataType: 'json',
            success: function (result) {

                self.ciudades(result);
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
        self.nombre(undefined);

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
                { width: "800", targets: 1 },
                { width: "800", targets: 2 }
            ]
        });

        $("#rw-cnt-bsq").css("cursor", "");
        $("#dv-cnt-bsq").removeClass("dsb-dv");

        $("#dv-loading-data").hide();
        $("#dv-tb-ctzn").show();
    };
}

var sucursalesVM = new sucursalesViewModel();
ko.applyBindings(sucursalesVM);

$(document).ready(function () {
    sucursalesVM.obtenerSucursales();
});