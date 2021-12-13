function reclutadoresViewModel() {

    var self = this;

    self.id = ko.observable();
    self.idUsr = ko.observable();
    self.nombres = ko.observable();
    self.apellidos = ko.observable();
    self.reclutador = ko.observableArray();
    self.reclutadores = ko.observableArray();

    self.directores = ko.observableArray();

    self.ciudadSelected = ko.observableArray();
    self.ciudades = ko.observableArray();

    self.tipo = ko.observable();
    self.textModal = ko.observable();
    self.titleModal = ko.observable();
    self.titleModal1 = ko.observable();

    self.ciudadesChanged = function () {

        if (self.ciudadSelected() !== undefined) {
            $('#ciudades').removeClass('error-vl');
        }
        else {
            $('#ciudades').addClass('error-vl');
        }
    };

    self.clearForm = function () {
        self.id(undefined);
        self.idUsr(undefined);
        self.nombres(undefined);
        self.apellidos(undefined);
        $('#ciudades')[0].selectedIndex = 0;
        $(':checkbox').prop('checked', false);
    };

    self.gestionarAdicion = function () {

        self.clearForm();
        self.obtenerDirectores();
        self.obtenerSucursales();

        self.tipo("Create");
        self.titleModal1("Agregar Reclutador");
        $("#mdl-form").modal("show");
    };

    self.gestionarEliminacion = function (id) {

        self.id(id);
        $("#mdl-delete").modal("show");
    };

    self.gestionarActualizacion = function (id) {

        self.id(id);
        self.tipo("Update");
        self.titleModal1("Actualizar Reclutador");

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerReclutadorPorId/',
            data: { id: id },
            dataType: 'json',
            success: function (result) {

                if (result.length >= 1) {

                    self.reclutador(result);

                    self.obtenerSucursales();
                    self.obtenerDirectores();

                    self.idUsr(result[0].IdUsuario);
                    self.nombres(result[0].Nombres);
                    self.apellidos(result[0].Apellidos);
                }

                $("#mdl-form").modal("show");

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });

    };

    self.eliminarReclutador = function () {

        $.ajax({
            url: '/Reclutamiento/GestionarReclutamiento?id=' + self.id() + '&idUsr=&nombres=&apellidos=&ciudadId=&directores=&tipo=Delete',
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            success: function (data) {

                if (data === true) {

                    $("#mdl-delete").modal("hide");

                    self.titleModal("Advertencia");
                    self.textModal("Se elimino el registro de forma exitosa.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");

                    self.obtenerReclutadores();
                }

                if (data === "Error" || data === false) {
                    self.titleModal("Advertencia");
                    self.textModal("No es posible eliminar el reclutador en este momento ya que se encuentra asignado a un proceso de prospecto a asesor.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                }

            },
            error: function () {
                self.titleModal("Advertencia");
                self.textModal("No es posible eliminar el reclutador en este momento ya que se encuentra asignado a un proceso de prospecto a asesor.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
            }
        });

    };

    self.actualizarReclutador = function () {

        if (self.validarForm()) {

            $(".error-vl").removeClass("error-vl");

            var idsUsr = [];
            $(':checkbox:checked').each(function (i) {
                idsUsr[i] = $(this).attr("id");
            });

            if (idsUsr.length === 0) {
                self.titleModal("Advertencia");
                self.textModal("Para realizar la actualización de un reclutador por favor seleccione uno o más directores para tener una asociación.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                return;
            }

            $.ajax({
                url: '/Reclutamiento/GestionarReclutamiento?id=' + self.id() + '&idUsr=' + self.idUsr() + '&nombres=' + self.nombres() + '&apellidos=' + self.apellidos() + '&ciudadId=' + self.ciudadSelected() + '&directores=' + idsUsr + '&tipo=Update',
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

                        self.obtenerReclutadores();
                    }

                    if (data === "Error" || data === false) {
                        self.titleModal("Advertencia");
                        self.textModal("No es posible actualizar el reclutador en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                        $("#mdl-error").modal("show");
                        $("#msg-error").removeClass("hidden");
                    }

                },
                error: function () {
                    self.titleModal("Advertencia");
                    self.textModal("No es posible actualizar el reclutador en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                }
            });
        }
    };

    self.validarForm = function () {

        if (self.idUsr() === undefined || self.idUsr() === "") {
            $("#idUsr").addClass("error-vl");
        }

        if (self.nombres() === undefined || self.nombres() === "") {
            $("#nombre").addClass("error-vl");
        }

        if (self.apellidos() === undefined || self.apellidos() === "") {
            $("#apellidos").addClass("error-vl");
        }

        if (self.ciudadSelected() === undefined) {
            $("#ciudades").addClass("error-vl");
        }

        if (self.nombres() && self.ciudadSelected())
            return true;

        return false;
    };

    self.agregarReclutador = function () {

        if (self.validarForm()) {

            $(".error-vl").removeClass("error-vl");

            var idsUsr = [];
            $(':checkbox:checked').each(function (i) {
                idsUsr[i] = $(this).attr("id");
            });

            if (idsUsr.length === 0) {
                self.titleModal("Advertencia");
                self.textModal("Para realizar la creación de un reclutador por favor seleccione uno o más directores para tener una asociación.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                return;
            }

            $.ajax({
                url: '/Reclutamiento/GestionarReclutamiento?id=0&idUsr=' + self.idUsr() + '&nombres=' + self.nombres() + '&apellidos=' + self.apellidos() + '&ciudadId=' + self.ciudadSelected() + '&directores=' + idsUsr + '&tipo=Create',
                type: 'POST',
                dataType: 'json',
                contentType: "application/json",
                success: function (data) {

                    if (data) {

                        $("#mdl-form").modal("hide");

                        self.titleModal("Advertencia");
                        self.textModal("Se agrego el reclutador de forma exitosa.");
                        $("#mdl-error").modal("show");
                        $("#msg-error").removeClass("hidden");

                        self.obtenerReclutadores();
                    }

                    if (data === "Error" || data === false) {
                        self.titleModal("Advertencia");
                        self.textModal("No es posible agregar el reclutador en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                        $("#mdl-error").modal("show");
                        $("#msg-error").removeClass("hidden");
                    }

                },
                error: function () {
                    self.titleModal("Advertencia");
                    self.textModal("No es posible agregar el reclutador en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                }
            });

        }

    };

    self.obtenerReclutadores = function () {

        self.clearForm();
        self.clearTable();

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerReclutadores/',
            data: { id: null },
            dataType: 'json',
            success: function (result) {

                self.reclutadores(result);
                self.drawTable();

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerDirectores = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerDirectores/',
            data: { id: null },
            dataType: 'json',
            success: function (result) {
                self.directores(result);

                if (self.tipo() === 'Update') {
                    for (var i = 0; i < self.reclutador().length; i++) {
                        $("#" + self.reclutador()[i].DirectoresAsignados).prop('checked', 'checked');
                    }
                }


            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerSucursales = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerSucursales/',
            data: { id: null },
            dataType: 'json',
            success: function (result) {

                if (self.tipo() === 'Update') {

                    window.setTimeout(function () {
                        self.ciudadSelected(self.reclutador()[0].Ciudad);
                        //$('#ciudades option[value="' + parseInt() + '"]').prop('selected', true);
                    }, 500);
                }

                self.ciudades(result);

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
        self.nombres(undefined);

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
                { width: "500", targets: 1 },
                { width: "500", targets: 2 },
                { width: "500", targets: 2 },
                { width: "500", targets: 2 }
            ]
        });

        $("#rw-cnt-bsq").css("cursor", "");
        $("#dv-cnt-bsq").removeClass("dsb-dv");

        $("#dv-loading-data").hide();
        $("#dv-tb-ctzn").show();
    };
}

var reclutadoresVM = new reclutadoresViewModel();
ko.applyBindings(reclutadoresVM);

$(document).ready(function () {
    reclutadoresVM.obtenerReclutadores();
});