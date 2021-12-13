function IndexViewModel() {
    var self = this;
    self.table = ko.observable();

    self.Productos = ko.observableArray([]);
    self.EstadosReserva = ko.observableArray([]);
    self.EstadoCotizacion = ko.observableArray([]);

    self.queryData = ko.observableArray([]);
    self.misCotizaciones = ko.observableArray([]);

    self.verificarReservasAVencer = function (data) {

        self.queryData(Enumerable.From(data)
            .Where(function (x) { return x.ValidarVencida === true; })
            .ToArray());

        if (self.queryData().length >= 1)
            $("#mdl-reservas").modal({ backdrop: "static" });
    };

    self.obtenerCotizaciones = function (tipo) {

        if (tipo === "Inicial") {

            var dateIni = moment().format('YYYY/MM/DD');
            var dateEnd = moment().add(2, 'months').format('YYYY/MM/DD');

            self.clearTable();

            $.get('../MisReservas/ObtenerMisCotizaciones/?estadoCotizacion=' + ''
                + '&producto=' + ''
                + '&fechaInicio=' + dateIni
                + '&fechaFin=' + dateEnd
                + '&estadoReserva=Vigente', function (data) {

                    self.verificarReservasAVencer(data.LstMisCotizacionesRol);

                    self.misCotizaciones(data.LstMisCotizacionesRol);

                    self.drawTable();

                }, 'json');

        }
        else {

            var fechaInicio = $("#dtIni").val();
            var fechaFin = $("#dtFin").val();
            var estadoCotizacion = $("select#estadoCotizacion").val().trim() !== "" ? $("#estadoCotizacion option:selected").text() : "";
            var productos = $("select#productos").val();
            var estadoReserva = $("select#estadoReserva").val().trim() !== "" ? $("#estadoReserva option:selected").text() : "Vigente";

            self.clearTable();

            $.get('../MisReservas/ObtenerMisCotizaciones/?estadoCotizacion=' + estadoCotizacion
                + '&producto=' + productos
                + '&fechaInicio=' + fechaInicio
                + '&fechaFin=' + fechaFin
                + '&producto=' + productos
                + '&estadoReserva=' + estadoReserva, function (data) {

                    self.misCotizaciones(data.LstMisCotizacionesRol);

                    self.drawTable();

                }, 'json');

        }
    };

    self.visualizarPDF = function (IdCotizacion) {

        //Google Analytics - Botón Cotizar Ahora Página Mis Reservas
        var dataGA = {
            "Category": "Mis Reservas",
            "Action": "Visualizar",
            "Label": "Visualización del pdf de la cotización de una reserva"
        };

        registerAnalytic(dataGA);

        var strWindowFeatures = "location=yes,height=320,width=620,scrollbars=yes,status=yes";
        var url = '/CotizarReserva/VisualizarCotizacion?cotID=' + IdCotizacion;
        window.open(url, "_blank", strWindowFeatures);
    };

    self.obtenerListados = function (nombreListado) {

        $.ajax({
            type: 'GET',
            url: '../BloqueoNit/ObtenerListadosMisCotizaciones/',
            data: { nombreListado: nombreListado },
            dataType: 'json',
            success: function (data) {

                switch (nombreListado) {
                    case "EstadosReserva":
                        self.EstadosReserva(data);
                        break;
                    case "EstadosCotizacion":
                        self.EstadoCotizacion(data);
                        break;
                    case "Productos":
                        self.Productos(data);
                        break;
                    default:
                }

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

        var table = $("#tb-ctzn").DataTable();
        table.clear().draw();

        $("#tb-ctzn").dataTable().fnDestroy();
    };

    self.drawTable = function () {

        $('#tb-ctzn').DataTable({
            processing: true,
            select: false,
            scrollY: "300px",
            scrollX: true,
            scrollCollapse: true,
            columnDefs: [
                { width: "60", targets: 0 },
                { width: "120", targets: 1 },
                { width: "120", targets: 2 },
                { width: "15", targets: 3 },
                { width: "125", targets: 4 },
                { width: "100", targets: 5 },
                { width: "115", targets: 6 },
                { width: "130", targets: 7 },
                { width: "250", targets: 8 },
                { width: "290", targets: 9 },
                { width: "290", targets: 10 },
                { width: "130", targets: 11 }
            ],
            fixedColumns: {
                leftColumns: 6
            },
            "lengthChange": false,
            initComplete: function () {

                $(".dv-search div").remove();
                $("#tb-ctzn_filter").detach().appendTo('.dv-search'); 

                $("#tb-ctzn_filter label").css("width", "100%");
                $("#tb-ctzn_filter label input")
                    .addClass("form-control ipt-fm")
                    .attr("placeholder", "Ingrese un parámetro de búsqueda");


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
            order: [[ 6, "desc" ]]
        });

        $("#rw-cnt-bsq").css("cursor", "");
        $("#dv-cnt-bsq").removeClass("dsb-dv");

        $("#dv-loading-data").hide();
        $("#dv-tb-ctzn").show();
    };

    self.limpiarFiltros = function () {

        var dt1 = moment().format('YYYY/MM/DD');
        var dt2 = moment().add(2, 'months').format('YYYY/MM/DD');

        $('#dtIni').val(dt1);
        $('#dtFin').val(dt2);

        self.obtenerCotizaciones("Inicial");
        $('.slc-ct-rs').prop('selectedIndex', 0);

    };

    self.cotizarAhora = function (IdCotizacion) {

        //Google Analytics - Botón Cotizar Ahora Página Mis Reservas
        var dataGA = {
            "Category": "Mis Reservas",
            "Action": "Cotizar",
            "Label": "Realizar cotización de la reserva de un NIT"
        };

        registerAnalytic(dataGA);

        window.location.href = '/CotizarReserva/Index?cotID=' + IdCotizacion;

    };
}

var reporteVM = new IndexViewModel();
ko.applyBindings(reporteVM);

moment.locale('es');

var id = 0;
var ii = 0;

$(function () {

    var dt1 = moment().format('YYYY/MM/DD');
    var dt2 = moment().add(2, 'months').format('YYYY/MM/DD');

    $('#dtIni').val(dt1);
    $('#dtFin').val(dt2);

    $('#sandbox-container .input-daterange').datepicker({
        format: "yyyy/mm/dd",
        todayBtn: "linked",
        clearBtn: false,
        language: "es",
        daysOfWeekHighlighted: "0,6",
        autoclose: true
    });
});

$(document).ready(function () {

    $("#search-tbl-ctzn").prop("disabled", true);

    reporteVM.misCotizaciones(null);

    reporteVM.obtenerListados("Productos");
    reporteVM.obtenerListados("EstadosReserva");
    reporteVM.obtenerListados("EstadosCotizacion");

    reporteVM.obtenerCotizaciones("Inicial");

});

$("#dtIni, #dtFin").on('change', function () {

    if (id === 5)
        id = 0;

    if (id === 0) {

        reporteVM.obtenerCotizaciones();
    }

    id += 1;
});