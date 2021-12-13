function ReportesGeneralViewModel() {
    var self = this;

    //Productos
    self.Productos = ko.observableArray([]);

    //Estado Reserva
    self.EstadosReserva = ko.observableArray([]);

    //Estado Cotización
    self.EstadoCotizacion = ko.observableArray([]);

    //Canales
    self.Canales = ko.observableArray([]);

    //Ciudades
    self.Ciudades = ko.observableArray([]);

    //Sectores
    self.Sectores = ko.observableArray([]);

    //Directores
    self.Directores = ko.observableArray([]);

    self.asesores = ko.observableArray([]);

    self.fechaInicio = ko.observable();
    self.fechaFin = ko.observable();
    self.reporteGeneral = ko.observableArray([]);

    var getIdsJoin = function (controlId) {
        let array = $("#" + controlId).val();
        if (array !== undefined && array !== null) {
            return array.join(',');
        }
        return null;
    };

    self.queryData = ko.observableArray([]);

    //Reporte
    self.reporteGeneral = ko.observableArray([]);


    self.obtenerDatosReporte = function () {

        $("#dv-loading-data").show();

        var filtros = {
            estadosCotizacionId: getIdsJoin("m-select-e-cotizacion"),
            productosId: getIdsJoin("m-select-productos"),
            estadosId: getIdsJoin("m-select-e-reserva"),
            ciudadesId: reporte == "Gerente Regional" ? ciudad : getIdsJoin("m-select-ciudades"),
            canalesId: getIdsJoin("m-select-canales"),
            sectoresId: getIdsJoin("m-select-sectores"),
            directoresId: getIdsJoin("m-select-directores"),
            asesoresId: ["Asesor", "Director"].includes(reporte) ? getIdsJoin("m-select-asesores") : $("#tx-asesor").val(),
            fechaInicio: self.fechaInicio(),
            fechaFin: self.fechaFin(),
            descripcionRol: reporte
        };

        $.ajax({
            type: 'POST',
            url: '../Reportes/GetData',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(filtros),
            success: function (data) {

                if (table !== undefined) {
                    table.clear().draw();
                    $("#tb-rpt").dataTable().fnDestroy();
                }
                self.reporteGeneral(data);

                self.iniciarTabla();

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
                $("#dv-loading-data").hide();
            }
        });
    };

    self.iniciarTabla = function () {

        var urlR = window.location.pathname;

        var options =
        {
            dom: 'Bfrtip',
            buttons: [
                {
                    extend: 'excel',
                    text: '<i class="fa fa-files-o"></i> Exportar a Excel',
                    titleAttr: 'Excel',
                    className: 'btn btn-success btnCl',
                    filename: function () {
                        return 'Reporte_' + reporte.replace(" ","_") + "_" + moment().format('DD_MM_YYYY_HH_mm');
                    },
                    attr: {
                        id:  'export-excel-pricing'
                    },
                    action: function (e, dt, button, config) {

                        if (urlR === "/Reportes/ReportePricing")
                            registerAnalyticExportExcelPricing();
                        if (urlR === "/Reportes/Index")
                            registerAnalyticExportExcelDirector();
                        if (urlR === "/Reportes/ReporteAsesor")
                            registerAnalyticExportExcelAsesor();
                        if (urlR === "/Reportes/ReporteGerenteRegional")
                            registerAnalyticExportExcelGerenteRegional();
                        if (urlR === "/Reportes/ReporteGerenteNacional")
                            registerAnalyticExportExcelGerenteNacional();

                        $.fn.dataTable.ext.buttons.excelHtml5.action.call(dt.button(this), e, dt, button, config);
                    }
                }
            ],
            processing: true,
            bFilter: false,
            paging: true,
            searching: false,
            scrollCollapse: true,
            retrieve: true,
            scrollY: 400,
            scrollX: true,
            responsive: true,
            language: {
                "lengthMenu": "Listar _MENU_",
                "zeroRecords": "No se han encontrado resultados para la búsqueda",
                "info": "Mostrando página _PAGE_ de _PAGES_",
                "infoEmpty": "No hay registros disponibles",
                "infoFiltered": "(filtered from _MAX_ total records)",
                "sSearch": "Buscar:",
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

        };

        switch (reporte) {
            case "Asesor":
                options.columnDefs = [
                    {
                        "targets": [1, 6, 7, 8, 9, 10, 11, 12, 15, 16, 17, 18, 19],
                        "visible": false
                    },
                    { width: "60", targets: 0 },
                    { width: "60", targets: 2 },
                    { width: "15", targets: 3 },
                    { width: "125", targets: 4 },
                    { width: "100", targets: 5 },
                    { width: "80", targets: 13 },
                    { width: "80", targets: 14 },
                    { width: "90", targets: 20 },
                    { width: "90", targets: 21 },
                    { width: "90", targets: 22 },
                    { width: "130", targets: 23 }
                ];
                options.initComplete = function () {
                    $("#th-hdr-rsva").attr('colspan', 6);
                    $("#tr-hdr-1").append("<th colspan='5' class='hdr-ctzn'>COTIZACIÓN</th >");
                    $("#dv-loading-data").hide();
                };
                break;
            case "Director":
                options.columnDefs = [
                    {
                        "targets": [1, 7, 8, 9, 10, 11, 12, 13, 16, 17, 18, 19, 20],
                        "visible": false
                    },
                    { width: "60", targets: 0 },
                    { width: "125", targets: 2 },
                    { width: "60", targets: 3 },
                    { width: "15", targets: 4 },
                    { width: "125", targets: 5 },
                    { width: "100", targets: 6 },
                    { width: "80", targets: 14 },
                    { width: "80", targets: 15 },
                    { width: "90", targets: 21 },
                    { width: "90", targets: 22 },
                    { width: "90", targets: 23 },
                    { width: "130", targets: 24 }
                ];
                options.initComplete = function () {
                    $("#th-hdr-rsva").attr('colspan', 7);
                    $("#tr-hdr-1").append("<th colspan='5' class='hdr-ctzn'>COTIZACIÓN</th >");
                    $("#dv-loading-data").hide();
                };
                break;
            case "Pricing":
            case "Gerente Nacional":
                options.columnDefs = [
                    {
                        "targets": [7, 8, 9, 10, 11, 12, 16, 17, 18, 19, 20],
                        "visible": false
                    },
                    { width: "60", targets: 0 },
                    { width: "125", targets: 1 },
                    { width: "125", targets: 2 },
                    { width: "60", targets: 3 },
                    { width: "15", targets: 4 },
                    { width: "125", targets: 5 },
                    { width: "100", targets: 6 },
                    { width: "80", targets: 13 },
                    { width: "80", targets: 14 },
                    { width: "80", targets: 15 },
                    { width: "90", targets: 21 },
                    { width: "90", targets: 22 },
                    { width: "90", targets: 23 },
                    { width: "130", targets: 24 }
                ];
                options.initComplete = function () {
                    $("#th-hdr-rsva").attr('colspan', 9);
                    $("#th-hdr-ctzn").attr('colspan', 5);
                    $("#dv-loading-data").hide();
                };
                break;
            case "Gerente Regional":
                options.columnDefs = [
                    {
                        "targets": [7, 8, 9, 10, 11, 12, 13, 16, 17, 18, 19, 20],
                        "visible": false
                    },
                    { width: "60", targets: 0 },
                    { width: "125", targets: 1 },
                    { width: "125", targets: 2 },
                    { width: "60", targets: 3 },
                    { width: "15", targets: 4 },
                    { width: "125", targets: 5 },
                    { width: "100", targets: 6 },
                    { width: "80", targets: 14 },
                    { width: "80", targets: 15 },
                    { width: "90", targets: 21 },
                    { width: "90", targets: 22 },
                    { width: "90", targets: 23 },
                    { width: "130", targets: 24 }
                ];
                options.initComplete = function () {
                    $("#th-hdr-rsva").attr('colspan', 9);
                    $("#tr-hdr-1").append("<th colspan='5' class='hdr-ctzn'>COTIZACIÓN</th >");
                    $("#dv-loading-data").hide();
                };
                break;
            default:
                break;
        }

        table = $("#tb-rpt").DataTable(options);
        table.buttons().container().appendTo('#tableActions');
    }

    self.obtenerListados = function (nombreListado) {

        $.ajax({
            type: 'GET',
            url: '../Reportes/ObtenerListadosMultiselect/',
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
                    case "Canales":
                        self.Canales(data);
                        break;
                    case "Ciudades":
                        self.Ciudades(data);
                        break;
                    case "SectorEconomico":
                        self.Sectores(data);
                        break;
                    case "Directores":
                        self.Directores(data);
                        break;
                    case "Asesores":
                        self.asesores(data);
                        break;
                    case "DirectoresRegion":
                        self.Directores(data);
                        break;
                    default:
                }

            }, error: function (jqXHR, textStatus, errorThrown) {

                console.log("error");
            }
        });
    };

}

var reporteGeneralVM = new ReportesGeneralViewModel();
ko.applyBindings(reporteGeneralVM);

var ii = 0;
$(document).ready(function () {

    reporteGeneralVM.obtenerListados("Productos");
    reporteGeneralVM.obtenerListados("EstadosReserva");
    reporteGeneralVM.obtenerListados("EstadosCotizacion");
    reporteGeneralVM.obtenerListados("Canales");
    reporteGeneralVM.obtenerListados("SectorEconomico");

    switch (reporte) {
        case "Director":
            reporteGeneralVM.obtenerListados("Asesores");
            break;
        case "Pricing":
        case "Gerente Nacional":
            reporteGeneralVM.obtenerListados("Directores");
            reporteGeneralVM.obtenerListados("Ciudades");
            break;
        case "Gerente Regional":
            reporteGeneralVM.obtenerListados("DirectoresRegion");
            break;
        default:
            break;
    }

    reporteGeneralVM.fechaInicio(start.format(formatDate));
    reporteGeneralVM.fechaFin(end.format(formatDate));

    // #region dateranger
    moment.locale('es');
    function cb(start, end) {
        reporteGeneralVM.fechaInicio(start.format(formatDate));
        reporteGeneralVM.fechaFin(end.format(formatDate));
        $('#reportrange span').html(start.format('DD-MM-YYYY') + ' - ' + end.format('DD-MM-YYYY'));
    }

    $('#reportrange').daterangepicker({
        locale: {
            "applyLabel": "Aplicar",
            "cancelLabel": "Cancelar",
            "customRangeLabel": "Rango Personalizado",
            "weekLabel": "S",
            "daysOfWeek": [
                "Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sá"
            ],
            "monthNames": [
                "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Deciembre"
            ],
            "firstDay": 1
        },
        startDate: start,
        endDate: end,
        ranges: {
            'Hoy': [moment(), moment()],
            'Ayer': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Los últimos 7 Dias': [moment().subtract(6, 'days'), moment()],
            'Últimos 30 días': [moment().subtract(29, 'days'), moment()],
            'Este mes': [moment().startOf('month'), moment().endOf('month')],
            'El mes pasado': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);

    cb(start, end);
    // #endregion dateranger

    //Limpiar filtros
    $('#btn-clear').on('click', function () {
        $("#m-select-e-reserva option:selected").prop("selected", false);
        $("#m-select-e-reserva").multiselect('refresh');
        $("#m-select-e-cotizacion option:selected").prop("selected", false);
        $("#m-select-e-cotizacion").multiselect('refresh');
        $("#m-select-productos option:selected").prop("selected", false);
        $("#m-select-productos").multiselect('refresh');
        $("#m-select-canales option:selected").prop("selected", false);
        $("#m-select-canales").multiselect('refresh');
        $("#m-select-sectores option:selected").prop("selected", false);
        $("#m-select-sectores").multiselect('refresh');
        $("#m-select-directores option:selected").prop("selected", false);
        $("#m-select-directores").multiselect('refresh');
        $("#m-select-asesores option:selected").prop("selected", false);
        $("#m-select-asesores").multiselect('refresh');
        $("#tx-asesor").val('');
    });

});



