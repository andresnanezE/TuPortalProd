function contratacionViewModel() {
    var self = this;

    self.titleModal = ko.observable();
    self.textModal = ko.observable();

    self.idProspecto = ko.observable();
    self.nombresApellidos = ko.observable();
    self.cedula = ko.observable();
    self.observaciones = ko.observable();
    self.estado = ko.observable();

    self.cedula1 = ko.observable(true);
    self.cedula2 = ko.observable(true);
    self.procuraduria = ko.observable(true);
    self.policia = ko.observable(true);
    self.rut = ko.observable(true);
    self.certificacion = ko.observable(true);
    self.hojavida = ko.observable(true);

    self.notas = ko.observableArray();
    self.solicitud = ko.observableArray();
    self.contrataciones = ko.observableArray();
    self.archivos = ko.observableArray();

    self.informacionPersonal = ko.observable(true);
    self.sarlaft = ko.observable(true);
    self.experienciaComercial = ko.observable(true);
    self.contrato = ko.observable(true);
    self.certificacionTributaria = ko.observable(true);

    self.gestionarContrato = function (id) {

        self.idProspecto(id);

        $.ajax({
            type: 'GET',
            data: { id: id, fist: true },
            url: '/Reclutamiento/ObtenerContrataciones/',
            dataType: 'json',
            success: function (result) {

                var data = result[0];
                self.nombresApellidos(data.NombresApellidos);
                self.cedula(data.NumeroDocumento);
                self.estado(data.Estado.toLowerCase().replace(/\s/g, ''));
                self.archivos(data.Archivos);

                self.informacionPersonal(data.InformacionPersonal);
                self.sarlaft(data.Sarlaft);
                self.experienciaComercial(data.ExperienciaComercial);
                self.contrato(data.Contrato);
                self.certificacionTributaria(data.CertificacionTributaria);

                $('#estado option[value="' + self.estado() + '"]').prop('selected', true);

                if (self.estado() === "aprobado" || self.estado() === "devuelto") {

                    $("#estado").attr("disabled", "disabled");
                    $(".dv-chcks").css({
                        "opacity": "0.6",
                        "pointer-events": "none"
                    });
                }
                else {
                    $("#estado").removeAttr("disabled");
                    $(".dv-chcks").removeAttr("style");
                }


                for (var i = 0; i < result[0].Archivos.length; i++) {

                    if (data.Archivos[i].EstadoArchivo !== "Pendiente") {
                        $("#chk-" + (i + 1)).prop("checked", false);
                    }

                    if (data.Archivos[i].EstadoArchivo === "Verificado") {
                        
                        $("#chk-" + (i + 1)).prop("checked", true);
                        $("#a-chk-" + (i + 1)).show();
                        $("#sp-chk-" + (i + 1)).hide();

                        if (result[0].Archivos[i].NombreOriginal === "ND") {
                            $("#a-chk-" + (i + 1)).hide();
                            $("#sp-chk-" + (i + 1)).show();
                        }
                    }
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

    self.obtenerContrataciones = function () {

        self.clearTable();

        $.ajax({
            type: 'GET',
            data: { id: 0, fist: false },
            url: '/Reclutamiento/ObtenerContrataciones/',
            dataType: 'json',
            success: function (result) {
                self.contrataciones(result);
                self.drawTable();
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.getFiles = function (rutaArchivo, nombreOriginal) {
        window.location.href = '/Digitalizacion/DownloadFiles?rutaArchivo=' + rutaArchivo + '&nombreOriginal=' + nombreOriginal;
    };

    self.datosContrato = function () {

        $.ajax({
            url: '/Reclutamiento/ExportarContrato',
            type: 'POST',
            data: { numeroIdentificacion: self.cedula() },
            success: function (data) {
                window.location.href = '/ArchivoTemporal/Download'
                    + '?fileGuid=' + data.FileGuid
                    + '&filename=' + data.FileName
                    + '&contentType=' + "application/pdf";
            },
            fail: function () {
                self.titleModal("Advertencia");
                self.textModal("No es posible mostrar el reporte en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                $("#mdl-fnsh").modal("show");
            }
        });
    };

    self.datosSolicitud = function () {
        $.ajax({
            url: '/Reclutamiento/DatosSolicitud',
            type: 'POST',
            data: { numeroIdentificacion: self.cedula() },
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
                $("#msg-error").removeClass("hidden");
            }
        });
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
            scrollX: true,
            ordering: false,
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
                { width: "290", targets: 11 }
            ],
            fixedColumns: {
                leftColumns: 1
            }
        });

        $("#rw-cnt-bsq").css("cursor", "");
        $("#dv-cnt-bsq").removeClass("dsb-dv");

        $("#dv-loading-data").hide();
        $("#dv-tb-ctzn").show();

        //self.stateClean(0);
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

    self.actualizarContratacion = function () {

        if (!self.observaciones()) {
            $('#observaciones').addClass('error-vl');
        }
        else {

            var estado = $("#estado option:selected").text();

            if (estado === "Pendiente") {
                self.titleModal("Advertencia");
                self.textModal("Por favor seleccione un estado diferente a Pendiente para continuar con la actualización de gestión de contratación.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                return;
            }
            else if (estado === "Devuelto") {
                if (self.cedula1() && self.cedula2() && self.procuraduria() && self.policia() && self.rut() && self.certificacion() && self.hojavida() && self.informacionPersonal() && self.sarlaft() && self.experienciaComercial() && self.contrato() && self.certificacionTributaria()) {
                    self.titleModal("Advertencia");
                    self.textModal("Para actualizar esta solicitud con estado Devuelto por favor seleccione el(los) archivo(s) y/o el(los) registros(s) a devolución.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                    return;
                }
            }
            else if (estado === "Aprobado") {
                if (!self.cedula1() || !self.cedula2() || !self.procuraduria() || !self.policia() || !self.rut() || !self.certificacion() || !self.hojavida() || !self.informacionPersonal() || !self.sarlaft() || !self.experienciaComercial() || !self.contrato() || !self.certificacionTributaria()) {
                    self.titleModal("Advertencia");
                    self.textModal("Para actualizar esta solicitud con estado Aprobado por favor active todos los archivos y active todos los registros.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                    return;
                }
            }

            var model = {
                NumeroDocumento: self.cedula(),
                IdProspecto: self.idProspecto(),
                Estado: estado,
                File1: self.cedula1(),
                File2: self.cedula2(),
                File3: self.procuraduria(),
                File4: self.policia(),
                File5: self.rut(),
                File6: self.certificacion(),
                File7: self.hojavida(),
                InformacionPersonal: self.informacionPersonal(),
                Sarlaft: self.sarlaft(),
                ExperienciaComercial: self.experienciaComercial(),
                Contrato: self.contrato(),
                CertificacionTributaria: self.certificacionTributaria(),
                Observaciones: self.observaciones()
            };

            $.ajax({
                url: "/Reclutamiento/ActualizarContratacion",
                type: 'POST',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify(model),
                success: function (result) {

                    self.observaciones("");

                    if (result) {
                        self.obtenerContrataciones();
                        $("#mdl-form").modal("hide");
                    }
                },
                error: function () {
                    //self.stateButton(0);
                    //self.titleModal("Registro no exitoso");
                    //self.textModal("El registro no se pudo realizar de forma exitosa, por favor intente realizar el registro mas tarde.");
                    //$("#mdl-error").modal("show");
                    //$("#msg-error").removeClass("hidden");
                }
            });
        }
    };
}

var contratacionVM = new contratacionViewModel();
ko.applyBindings(contratacionVM);

$(document).ready(function () {
    contratacionVM.obtenerContrataciones();
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