function reporteDirectorViewModel() {

    var self = this;

    self.titleModal = ko.observable();
    self.textModal = ko.observable();

    self.fechaInicio = ko.observable(moment().subtract(1, 'month').format('YYYY/MM/DD'));
    self.fechaFin = ko.observable(moment().format('YYYY/MM/DD'));

    self.generarReporte = function () {

        $.ajax({
            url: '/Reclutamiento/ExportarReporteDirector',
            type: 'POST',
            data: { fechaInicio: self.fechaInicio(), fechaFin: self.fechaFin() },
            success: function (data) {
                window.location.href = '/ArchivoTemporal/Download'
                    + '?fileGuid=' + data.FileGuid
                    + '&filename=' + data.FileName
                    + '&contentType=' + "application/pdf";
            },
            fail: function () {
                self.titleModal("Advertencia");
                self.textModal("No es posible mostrar el reporte en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                $("#mdl-error").modal("show");
            }
        });

    };
}

var reporteDirectorVM = new reporteDirectorViewModel();
ko.applyBindings(reporteDirectorVM);

$(document).ready(function () {
    $('.input-group.date').datepicker({
        format: "yyyy/mm/dd",
        language: "es",
        autoclose: true,
        todayHighlight: true
    });
});