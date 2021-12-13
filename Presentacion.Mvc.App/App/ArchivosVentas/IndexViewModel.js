function IndexViewModel() {

    var self = this;
    self.archivosBlob = ko.observableArray();
    self.documento = ko.observable();
    self.textModal = ko.observable();

    self.obtenerArchivos = function () {
        self.archivosBlob.removeAll();
        self.clearTable();
        $("#dv-loading-data").show();
        if (self.documento() === undefined || self.documento() === '') {
            self.textModal("Por favor ingrese un número de documento.");
            $("#dv-loading-data").hide();
            $("#mdl-fnsh").modal("show");
            return;
        }

        $.ajax({
            type: "POST",
            url: "/ArchivosVentas/GetArchivosVentas",
            data:
            {
                "documento": self.documento()
            },
            dataType: "json",
            success: function (datos) {
                $("#dv-loading-data").hide();
                if (datos == undefined || datos.length === 0) {
                    self.textModal("No hay documentos listados con el documento consultado.");
                    $("#mdl-fnsh").modal("show");
                }
                else {

                    self.archivosBlob(datos);
                    self.drawTable();
                }
            }
        });
    };

    self.clearTable = function () {

        $("#dv-tb-ctzn").hide();
        $("#dv-loading-data").show();

        var table = $("#tb-ctzn").DataTable();
        table.clear().draw();

        $("#tb-ctzn").dataTable().fnDestroy();
    };

    self.descargarArchivo = function (fileName) {
        window.location.href = "/ArchivosVentas/DownloadFile?name=" + fileName;
    };

    self.drawTable = function () {

        $('#tb-ctzn').DataTable({
            fixedColumns: true,
            columnDefs: [
                { width: 960, targets: 0 }
            ],
            buttons: [],
            scrollX: true,
            processing: true,
            searching: false,
            lengthChange: false,
            dom: 'Bfrtip',
            initComplete: function () {
                $(".dv-search div").remove();
                $("#dv-loading-data").hide();
                $("#dv-tb-ctzn").fadeIn();
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

        $("#dv-loading-data").hide();
        $("#dv-tb-ctzn").show();
    };
}

var reporteVM = new IndexViewModel();
ko.applyBindings(reporteVM);

$(document).ready(function () {
    $("#dv-loading-data").hide();
});