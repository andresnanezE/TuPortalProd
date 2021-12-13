function capacitacionViewModel() {
    var self = this;

    self.titleModal = ko.observable();
    self.textModal = ko.observable();

    self.estadoId = ko.observable();
    self.capacitadorId = ko.observable();

    self.idProspecto = ko.observable();
    self.nombresApellidos = ko.observable();
    self.tipoIdentificacion = ko.observable();
    self.cedula = ko.observable();
    self.estado = ko.observable();
    self.estadoCapacitacion = ko.observable();
    self.capacitador = ko.observable();
    self.fechaInicio = ko.observable("");
    self.fechaFin = ko.observable("");
    self.fechaCapacitacion = ko.observable();
    self.observaciones = ko.observable();

    self.notas = ko.observableArray();
    self.capacitadores = ko.observableArray();
    self.capacitaciones = ko.observableArray();

    self.obtenerCapacitaciones = function () {

        self.clearTable();

        $.ajax({
            type: 'GET',
            data: { id: 0, fist: false },
            url: '/Reclutamiento/ObtenerCapacitaciones/',
            dataType: 'json',
            success: function (result) {

                self.capacitaciones(result);
                self.drawTable();
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.gestionarCapacitacion = function (id) {

        self.idProspecto(id);

        $.ajax({
            type: 'GET',
            data: { id: id, fist: true },
            url: '/Reclutamiento/ObtenerCapacitaciones/',
            dataType: 'json',
            success: function (result) {
                self.nombresApellidos(result[0].ApellidosNombres);
                self.cedula(result[0].NumeroDocumento);
                self.estadoCapacitacion(result[0].EstadoCapacitacion);
                self.estado(result[0].Estado.toLowerCase());
                self.tipoIdentificacion(result[0].TipoIdentificacion);
                self.fechaCapacitacion(moment(result[0].FechaCapacitacion).format('MM/DD/YYYY, h:mm a'));

                $('#estado option[value="' + self.estado() + '"]').prop('selected', true);

                if (self.estadoCapacitacion() === "Devuelto") {
                    $('#estado').attr("disabled", "disabled");
                    $('#observaciones').attr("disabled", "disabled");
                    $('#capacitador').attr("disabled", "disabled");
                    $('#dtIni').attr("disabled", "disabled");
                    $('#dtFin').attr("disabled", "disabled");
                    $('.input-group.date').css({
                        "pointer-events": "none",
                        "cursor": "not-allowed"
                    });
                    $('#estado option[value="' + self.estadoCapacitacion() + '"]').prop('selected', true);
                }

                self.capacitadorId(result[0].IdReclutamientoCapacitador);

                if (self.estado() !== "pendiente") {
                    self.fechaInicio(moment(result[0].FechaInicio).format('MM/DD/YYYY'));
                    self.fechaFin(moment(result[0].FechaFin).format('MM/DD/YYYY'));
                }
                self.obtenerNotas();

                $("#mdl-form").modal("show");

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerNotas = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerNotasReclutamiento/',
            data: { numeroDocumento: self.cedula() },
            dataType: 'json',
            success: function (result, status, xhr) {
                self.notas(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerCapacitadores = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerCapacitadores/',
            dataType: 'json',
            success: function (result, status, xhr) {
                self.capacitadores(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.actualizarCapacitacion = function () {

        var estado = $("#estado option:selected").text();

        if (estado === "Pendiente") {
            self.titleModal("Advertencia");
            self.textModal("Por favor seleccione un estado diferente a Pendiente para continuar con la actualización de gestión de capacitación.");
            $("#mdl-error").modal("show");
            $("#msg-error").removeClass("hidden");
            return;
        }

        if (estado === "Aprobado") {
            if (self.capacitadorId() === undefined) {
                $('#capacitador').addClass('error-vl');
                return;
            }
            if (self.fechaInicio() === "" || self.fechaFin() === "") {
                self.titleModal("Advertencia");
                self.textModal("Por favor seleccione las fechas para la capacitación.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                return;
            }
            if (self.observaciones() === undefined || self.observaciones() === "") {
                $('#observaciones').addClass('error-vl');
                return;
            }

            if (self.fechaFin() < self.fechaInicio()) {
                self.titleModal("Advertencia");
                self.textModal("La fecha cierre de capacitación debe ser menor que la fecha inicio de capacitación.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                return;
            }
        }

        if (estado === "Devuelto") {
            if (self.observaciones() === undefined || self.observaciones() === "") {
                $('#observaciones').addClass('error-vl');
                return;
            }
            if (self.capacitadorId() === undefined)
                self.capacitadorId(0);
        }

        var model = {
            NumeroDocumento: self.cedula(),
            idProspecto: self.idProspecto(),
            estado: estado,
            capacitadorId: self.capacitadorId(),
            fechaInicio: self.fechaInicio(),
            fechaFin: self.fechaFin(),
            observaciones: self.observaciones()
        };

        $.ajax({
            url: "/Reclutamiento/ActualizarCapacitacion",
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(model),
            success: function (result) {

                self.observaciones("");
                if (result) {
                    self.obtenerCapacitaciones();
                    $("#mdl-form").modal("hide");
                }
            },
            error: function () {
                self.titleModal("Actualización no exitoso");
                self.textModal("La operación no se pudo realizar de forma exitosa, por favor contactese con el administrador del sistema.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
            }
        });
    };

    self.clearTable = function () {

        $("#search-tbl-ctzn").val("");

        $("#dv-tb-ctzn").hide();
        $("#dv-loading-data").show();

        $("#rw-cnt-bsq").css("cursor", "wait");
        $("#dv-cnt-bsq").addClass("dsb-dv");

        var table = $("#tb-ctzn").DataTable();
        table.clear().draw();

        $("#tb-ctzn").dataTable().fnDestroy();
    };

    self.drawTable = function () {

        $('#tb-ctzn').DataTable({
            processing: true,
            "lengthChange": false,
            dom: 'Bfrtip',
            scrollY: "300px",
            scrollX: true,
            ordering: false,
            scrollCollapse: true,
            columnDefs: [
                { width: "60", targets: 0 },
                { width: "100", targets: 1 },
                { width: "200", targets: 2 },
                { width: "200", targets: 3 },
                { width: "120", targets: 4 },
                { width: "120", targets: 5 },
                { width: "120", targets: 6 },
                { width: "120", targets: 7 },
                { width: "120", targets: 8 },
                { width: "120", targets: 9 },
                { width: "120", targets: 10 }
            ],
            fixedColumns: {
                leftColumns: 1
            },
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
            }

        });

        var table = $("#tb-ctzn").DataTable();
        table.columns.adjust().draw();

        $("#rw-cnt-bsq").css("cursor", "");
        $("#dv-cnt-bsq").removeClass("dsb-dv");

        $("#dv-loading-data").hide();
        $("#dv-tb-ctzn").show();
    };
    
    self.estadoChanged = function () {

        var estado = $("#estado").val();

        if (estado === "") {
            $('#estado').addClass('error-vl');
        }
        else {
            $('#estado').removeClass('error-vl');
        }
    };

    self.capacitadorChanged = function () {
        if (self.capacitadorId() === undefined) {
            $('#capacitador').removeClass('error-vl');
        }
        else {
            $('#capacitador').addClass('error-vl');
        }
    };
}

var capacitacionVM = new capacitacionViewModel();
ko.applyBindings(capacitacionVM);

$(document).ready(function () {

    $('.input-group.date').datepicker({
        format: "yyyy/mm/dd",
        language: "es",
        autoclose: true,
        todayHighlight: true
    });

    capacitacionVM.obtenerCapacitadores();
    capacitacionVM.obtenerCapacitaciones();

});

$("#observaciones").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#dtIni").change(function () {
    
    var val = $(this).val();

    if (val !== "Sin definir")
        $(this).removeClass("error-vl");

});