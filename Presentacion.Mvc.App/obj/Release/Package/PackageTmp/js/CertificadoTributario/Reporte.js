function ReporteViewModel() {

    var self = this;
    self.envios = ko.observableArray();
    self.stateClean = ko.observable(0);

    self.obtenerReporteEnvios = function () {
        self.clearTable();

        var fInicio = $("#dtIni").val();
        var fechaFin = $("#dtFin").val();

        $.ajax({
            type: 'POST',
            url: '/CertificadoTributario/ObtenerReporteEnvios',
            success: function (result, status, xhr) {
                if (result.Exitoso) {
                    self.envios(result.Result);
                    self.drawTable();
                }
            }
            ,
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("error1 ", jqXHR);
            }
        });
    };

    

    self.drawTable = function () {
        $('#tb-ctzn').DataTable({
            fixedColumns: true,
            columnDefs: [
                { width: 100, targets: 7 }
            ],
            scrollX: true,
            processing: true,
            lengthChange: false,
            dom: 'Bfrtip',
            buttons: [{
                extend: 'excelHtml5'
                , text: '<i class="fa fa-file-excel-o" aria-hidden="true"></i> Descargar Excel'
                , className: 'btn-export-xsl'
                , autoFilter: true
                , titleAttr: 'Descargar'
                , sheetName: 'Data Cargue'
                , html: '<h1>asdasd</h1>'
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
            order: [[0, "desc"]]
        }).rows().recalcHeight().columns.adjust().draw();
        

        $("#rw-cnt-bsq").css("cursor", "");
        $("#dv-cnt-bsq").removeClass("dsb-dv");

        $("#dv-loading-data").hide();
        $("#dv-tb-ctzn").show();

        self.stateClean(0);
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

    self.limpiarFiltros = function () {

        self.stateClean(1);

        //var dtNow = moment().format('YYYY/MM/DD');

        //$('#dtIni').val(dtNow);
        //$('#dtFin').val(dtNow);

        //$('#dtIni,#dtFin').datepicker('update');

        reporteVM.obtenerReporteEnvios();
    };
}





var reporteVM = new ReporteViewModel();
ko.applyBindings(reporteVM);

var id = 0;

//$(function () {

//    var dtNow = moment().format('YYYY/MM/DD');

//    $('#dtIni').val(dtNow);
//    $('#dtFin').val(dtNow);

//    $('#sandbox-container .input-daterange').datepicker({
//        format: "yyyy/mm/dd",
//        todayBtn: "linked",
//        clearBtn: false,
//        language: "es",
//        daysOfWeekHighlighted: "0,6",
//        autoclose: true
//    });
//});

$(document).ready(function () {
    reporteVM.obtenerReporteEnvios();
});

function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select",
        "contextmenu", "drop"].forEach(function (event) {

        if (textbox !== null) {
            textbox.addEventListener(event, function () {
                if (inputFilter(this.value)) {
                    this.oldValue = this.value;
                    this.oldSelectionStart = this.selectionStart;
                    this.oldSelectionEnd = this.selectionEnd;
                } else if (this.hasOwnProperty("oldValue")) {
                    this.value = this.oldValue;
                    this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                } else {
                    this.value = "";
                }
            });
        }
    });
}

//---------INPUT VALIDATIONS---------//
setInputFilter(document.getElementById("noFormulario"), function (value) {
    return /^-?\d*$/.test(value);
});

setInputFilter(document.getElementById("cedAsesor"), function (value) {
    return /^-?\d*$/.test(value);
});

setInputFilter(document.getElementById("cedContratante"), function (value) {
    return /^-?\d*$/.test(value);
});

setInputFilter(document.getElementById("noAfiliados"), function (value) {
    return /^-?\d*$/.test(value);
});

$("#noFormulario").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#cedAsesor").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#cedContratante").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#noAfiliados").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#link").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("input:file").change(function () {
    $("#tx-upd").text("");
});

//---------INPUT VALIDATIONS---------//

$(document).ajaxSend(function (event, request, settings) {
    $("#ov-loading").show();
});

$(document).ajaxStop(function () {

    window.setTimeout(function () {
        $("#ov-loading").hide();
    }, 1000);
});