function solicitudesViewModel() {
    var self = this;

    self.titleModal = ko.observable();
    self.textModal = ko.observable();

    self.fFechaInicio = ko.observable();
    self.fFechaFin = ko.observable();
    self.fCedula = ko.observable("");
    self.directorId = ko.observable("");
    self.directores = ko.observableArray();
    self.directorSelected = ko.observable();


    self.idProspecto = ko.observable();
    self.nombresApellidos = ko.observable();
    self.cedula = ko.observable();
    self.tipoIdentificacion = ko.observable();
    self.observaciones = ko.observable();
    self.estado = ko.observable();
    self.proceso = ko.observable();
    self.archivos = ko.observableArray();

    self.cedula1 = ko.observable(true);
    self.cedula2 = ko.observable(true);
    self.procuraduria = ko.observable(true);
    self.policia = ko.observable(true);
    self.rut = ko.observable(true);
    self.certificacion = ko.observable(true);
    self.hojavida = ko.observable(true);

    self.informacionPersonal = ko.observable(true);
    self.sarlaft = ko.observable(true);
    self.experienciaComercial = ko.observable(true);
    self.contrato = ko.observable(true);
    self.certificacionTributaria = ko.observable(true);

    self.notas = ko.observableArray();
    self.solicitud = ko.observableArray();
    self.solicitudes = ko.observableArray();

    self.limpiarFiltros = function () {

        $('#proceso option[value=""]').prop('selected', true);
        $('#estado option[value=""]').prop('selected', true);
        $('#directores').prop('selectedIndex', 0);
        self.fCedula("");

        var dtNow = moment().subtract(2, 'months').format('YYYY/MM/DD');
        var dtFin = moment().format('YYYY/MM/DD');
        self.fFechaInicio(dtNow);
        self.fFechaFin(dtFin);

        self.obtenerSolicitudes("Inicial");
    };

    self.obtenerDirectores = function (referidoId) {
        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerDirectoresPorReferido/',
            data: { referidoId: 0 },
            dataType: 'json',
            success: function (result) {
                self.directores(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.directorChanged = function () {

        if (self.directorSelected() !== undefined)
            self.directorId(self.directorSelected().id_usr);
        else
            self.directorId("");
    };

    self.gestionarSolicitud = function (id) {

        var estadoOp = '';
        self.idProspecto(id);

        $.ajax({
            type: 'GET',
            data: { id: id, first: true, proceso: '', estado: '', director: '', fechaInicio: self.fFechaInicio(), fechaFin: self.fFechaFin(), numeroDocumento: '' },
            url: '/Reclutamiento/ObtenerSolicitudes/',
            dataType: 'json',
            success: function (result) {

                self.nombresApellidos(result[0].NombresApellidos);
                self.cedula(result[0].NumeroDocumento);
                self.tipoIdentificacion(result[0].TipoDocumento);
                self.archivos(result[0].Archivos);

                self.estado(result[0].Estado.toLowerCase());
                self.proceso(result[0].Proceso.toLowerCase());

                if (self.estado() === "aprobado" || self.estado() === "devuelto") {

                    if (self.proceso() === "capacitación")
                        $("#estadomdl").removeAttr("disabled");
                    else
                        $("#estadomdl").attr("disabled", "disabled");

                    $(".dv-chcks").css({
                        "opacity": "0.6",
                        "pointer-events": "none"
                    });
                }

                $('#estadomdl option[value="' + self.estado() + '"]').prop('selected', true);

                for (var i = 0; i < result[0].Archivos.length; i++) {

                    if (result[0].Archivos[i].EstadoArchivo !== "Pendiente") {
                        $("#chk-" + (i + 1)).prop("checked", false);
                    }

                    if (result[0].Archivos[i].EstadoArchivo === "Pendiente") {

                        //if (result[0].Archivos[i].NombreOriginal !== "ND")
                        //    $("#chk-" + (i + 1)).prop("checked", true);
                        //else {
                        //    $("#a-chk-" + (i + 1)).replaceWith("<span>Sin Cargue</span>");
                        //}
                        $("#chk-" + (i + 1)).prop("checked", true);
                        $("#a-chk-" + (i + 1)).show();
                        $("#sp-chk-" + (i + 1)).hide();

                        if (result[0].Archivos[i].NombreOriginal === "ND") {
                            $("#a-chk-" + (i + 1)).hide();
                            $("#sp-chk-" + (i + 1)).show();
                        }
                    }
                }

                self.cedula1(result[0].Archivos[0].EstadoArchivo === "Pendiente" ? true : false);
                self.cedula2(result[0].Archivos[1].EstadoArchivo === "Pendiente" ? true : false);
                self.procuraduria(result[0].Archivos[2].EstadoArchivo === "Pendiente" ? true : false);
                self.policia(result[0].Archivos[3].EstadoArchivo === "Pendiente" ? true : false);
                self.rut(result[0].Archivos[4].EstadoArchivo === "Pendiente" && result[0].Archivos[5].nombreOriginal !== "ND"  ? true : false);
                self.certificacion(result[0].Archivos[5].EstadoArchivo === "Pendiente" && result[0].Archivos[5].nombreOriginal !== "ND" ? true : false);
                self.hojavida(result[0].Archivos[6].EstadoArchivo === "Pendiente" ? true : false);

                self.obtenerNotas();

                $("#mdl-form").modal("show");

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.generarExcel = function () {

        var createXLSLFormatObj = [];

        var xlsHeader = ["Id", "Tipo de documento", "Número de documento", "Apellidos-Nombres", "Estado", "Proceso", "Correo electrónico", "Teléfono", "Ciudad de vinculación", "Gestionado Por", "Director", "Fecha de registro", "Archivos"];

        createXLSLFormatObj.push(xlsHeader);
        $.each(self.solicitudes(), function (index, value) {
            
            var innerRowData = [];
            $.each(value, function (ind, val) {

                var archivos = "";

                if (ind !== "EstadoArchivo" && ind !== "Archivos" && ind !== "IdUsrDirector" && ind !== "LogUsr")
                    innerRowData.push(val);

                if (ind === "Archivos") {

                    for (var i = 0; i < val.length; i++) {

                        if (i === val.length -1)
                            archivos += val[i].NombreOriginal;
                        else
                            archivos += val[i].NombreOriginal + " - ";
                    }

                    innerRowData.push(archivos);
                }
            });
            createXLSLFormatObj.push(innerRowData);
        });

        var filename = "Reporte.xlsx";
        var ws_name = "Hoja Reporte";

        var wb = XLSX.utils.book_new(),
            ws = XLSX.utils.aoa_to_sheet(createXLSLFormatObj);

        XLSX.utils.book_append_sheet(wb, ws, ws_name);
        XLSX.writeFile(wb, filename);
    };

    self.gestionarContrato = function (id) {

        var estadoOp = '';
        self.idProspecto(id);
        $.ajax({
            type: 'GET',
            data: { id: id },
            url: '/Reclutamiento/ObtenerContrato/',
            dataType: 'json',
            success: function (result) {

                self.nombresApellidos(result[0].NombresApellidos);
                self.cedula(result[0].NumeroDocumento);
                self.tipoIdentificacion(result[0].TipoDocumento);
                
                self.informacionPersonal(result[0].InformacionPersonal);
                self.sarlaft(result[0].Sarlaft);
                self.experienciaComercial(result[0].ExperienciaComercial);
                self.contrato(result[0].Contrato);
                self.certificacionTributaria(result[0].CertificacionTributaria);
                self.estado(result[0].Estado.toLowerCase());
                self.proceso(result[0].Proceso.toLowerCase());

                if (self.estado() === "aprobado" || self.estado() === "devuelto") {

                    $("#estadomdl").attr("disabled", "disabled");
                    $(".dv-chcks").css({
                        "opacity": "0.6",
                        "pointer-events": "none"
                    });
                }

                $('#estadomdl option[value="' + self.estado() + '"]').prop('selected', true);

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

    self.obtenerSolicitudes = function (estado) {

        self.clearTable();

        if (estado === "Inicial") {

            $.ajax({
                type: 'GET',
                data: { id: 0, first: false, proceso: '', estado: '', director: self.directorId(), fechaInicio: self.fFechaInicio(), fechaFin: self.fFechaFin(), numeroDocumento: self.fCedula() },
                url: '/Reclutamiento/ObtenerSolicitudes/',
                dataType: 'json',
                success: function (result) {

                    self.solicitudes(result);
                    self.drawTable();
                }, error: function (jqXHR, textStatus, errorThrown) {
                    console.log("error");
                }
            });
        }
        else {

            var proceso = $("#proceso option:selected").val();
            var estadoP = $("#estado option:selected").val();

            $.ajax({
                type: 'GET',
                data: { id: 0, first: false, proceso: proceso, estado: estadoP, director: self.directorId(), fechaInicio: self.fFechaInicio(), fechaFin: self.fFechaFin(), numeroDocumento: self.fCedula() },
                url: '/Reclutamiento/ObtenerSolicitudes/',
                dataType: 'json',
                success: function (result) {

                    self.solicitudes(result);
                    self.drawTable();
                }, error: function (jqXHR, textStatus, errorThrown) {
                    console.log("error");
                }
            });
        }
    };

    self.getFiles = function (rutaArchivo, nombreOriginal) {
        window.open('/Digitalizacion/DownloadFiles?rutaArchivo=' + rutaArchivo + '&nombreOriginal=' + nombreOriginal, '_blank');
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

    self.informeGestion = function () {

        $.ajax({
            url: '/Reclutamiento/ExportarInformeGestion/',
            type: 'POST',
            data: { fechaInicio: self.fFechaInicio(), fechaFin: self.fFechaFin(), director: self.directorId, usuario: self.fCedula() },
            success: function (data) {

                if (data === "Error") {
                    self.titleModal("Advertencia");
                    self.textModal("No es posible mostrar el reporte en este momento. Inténtalo nuevamente, si el problema persiste, comunícate con el área de tecnología.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                }
                else {
                    window.location.href = '/ArchivoTemporal/Download'
                    + '?fileGuid=' + data.FileGuid
                    + '&filename=' + data.FileName
                    + '&contentType=' + "application/pdf";
                }
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
            buttons: false,
            initComplete: function () {

                $(".dt-buttons").remove();

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
                { width: "500", targets: 1 },
                { width: "500", targets: 2 }
            ]
            ,fixedColumns: {
                leftColumns: 1
            }
        });

        $("#rw-cnt-bsq").css("cursor", "");
        $("#dv-cnt-bsq").removeClass("dsb-dv");

        $("#dv-loading-data").hide();
        $("#dv-tb-ctzn").show();
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

    self.actualizarSolicitud = function () {

        if (self.observaciones() === undefined || self.observaciones() === "") {
            $('#observaciones').addClass('error-vl');
            return;
        }
        else {
            $('#observaciones').removeClass('error-vl');
            var estado = $("#estadomdl option:selected").text();

            if (estado === "Pendiente") {
                self.titleModal("Advertencia");
                self.textModal("Por favor seleccione un estado diferente a Pendiente para continuar con la actualización de gestión de solicitud.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                return;
            }
            else if (estado === "Devuelto") {
                if (self.cedula1() && self.cedula2() && self.procuraduria() && self.policia() && self.rut() && self.certificacion() && self.hojavida()) {
                    self.titleModal("Advertencia");
                    self.textModal("Para actualizar esta solicitud con estado Devuelto por favor seleccione el(los) archivo(s) a devolución.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                    return;
                }
            }
            else if (estado === "Aprobado") {
                if (!self.cedula1() || !self.cedula2() || !self.procuraduria() || !self.policia() || !self.rut() || !self.certificacion() || !self.hojavida()) {
                    self.titleModal("Advertencia");
                    self.textModal("Para actualizar esta solicitud con estado Aprobado por favor active todos los archivos.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                    return;
                }
            }
            //else if (estado === "Aprobado") {
            //    if (!self.cedula1() || !self.cedula2() || !self.procuraduria() || !self.policia() || !self.rut() || !self.certificacion() || !self.hojavida()) {
            //        self.titleModal("Advertencia");
            //        self.textModal("Para actualizar esta solicitud con estado Aprobado por favor active todos los archivos.");
            //        $("#mdl-error").modal("show");
            //        $("#msg-error").removeClass("hidden");
            //        return;
            //    }
            //}

            var model = {
                NumeroDocumento: self.cedula(),
                idProspecto: self.idProspecto(),
                file1: self.cedula1(),
                file2: self.cedula2(),
                file3: self.procuraduria(),
                file4: self.policia(),
                file5: self.rut(),
                file6: self.certificacion(),
                file7: self.hojavida(),
                estado: estado,
                observaciones: self.observaciones()
            };

            $.ajax({
                url: "/Reclutamiento/ActualizarSolicitud",
                type: 'POST',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify(model),
                success: function (result) {

                    self.observaciones("");

                    if (result) {
                        self.obtenerSolicitudes();
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

    self.actualizarSolicitudCapacitacion = function () {

        if (self.observaciones() === undefined || self.observaciones() === "") {
            $('#observaciones').addClass('error-vl');
        }
        else {

            $('#observaciones').removeClass('error-vl');
            var estado = $("#estadomdl option:selected").text();

            if (estado === "Devuelto") {
                self.titleModal("Advertencia");
                self.textModal("Por favor seleccione un estado diferente a devuelto para reactivar el proceso de capacitación.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                return;
            }

            var model = {
                idProspecto: self.idProspecto(),
                estado: estado,
                observaciones: self.observaciones()
            };

            $.ajax({
                url: "/Reclutamiento/ActualizarCapacitacionReclutador",
                type: 'POST',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify(model),
                success: function (result) {

                    if (result === true) {
                        $("#mdl-form").modal("hide");
                        self.obtenerSolicitudes();
                        self.titleModal("Advertencia");
                        self.textModal("Se actualizo el estado de la capacitación de forma exitosa.");
                        $("#mdl-error").modal("show");
                        $("#msg-error").removeClass("hidden");
                    }
                    else {
                        self.stateButton(0);
                        self.titleModal("Registro no exitoso");
                        self.textModal("El registro no se pudo realizar de forma exitosa, por favor intente realizar la actualización mas tarde.");
                        $("#mdl-error").modal("show");
                        $("#msg-error").removeClass("hidden");
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

    self.actualizarContrato = function () {

        if (!self.observaciones()) {
            $('#observaciones').addClass('error-vl');
        }
        else {
            var estado = $("#estadomdl option:selected").text();

            if (estado === "Pendiente") {
                self.titleModal("Advertencia");
                self.textModal("Por favor seleccione un estado diferente a Pendiente para continuar con la actualización de gestión de solicitud.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                return;
            }
            else if (estado === "Devuelto") {
                if (self.informacionPersonal() && self.sarlaft() && self.experienciaComercial() && self.contrato() && self.certificacionTributaria()) {
                    self.titleModal("Advertencia");
                    self.textModal("Para actualizar esta solicitud con estado Devuelto por favor seleccione el(los) registros(s) a devolución.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                    return;
                }
            }
            else if (estado === "Aprobado") {
                if (!self.informacionPersonal() || !self.sarlaft() || !self.experienciaComercial() || !self.contrato() || !self.certificacionTributaria()) {
                    self.titleModal("Advertencia");
                    self.textModal("Para actualizar esta solicitud con estado Aprobado por favor active todos los registros.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                    return;
                }
            }

            var model = {
                IdProspecto: self.idProspecto(),
                NumeroDocumento: self.cedula(),
                InformacionPersonal: self.informacionPersonal(),
                Sarlaft: self.sarlaft(),
                ExperienciaComercial: self.experienciaComercial(),
                Contrato: self.contrato(),
                CertificacionTributaria: self.certificacionTributaria(),
                Estado: estado,
                Observaciones: self.observaciones()
            };

            $.ajax({
                url: "/Reclutamiento/ActualizarSolicitudContrato",
                type: 'POST',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify(model),
                success: function (result) {

                    self.observaciones("");

                    if (result) {
                        self.obtenerSolicitudes();
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

var solicitudesVM = new solicitudesViewModel();
ko.applyBindings(solicitudesVM);

$(document).ready(function () {

    var dtNow = moment().subtract(2, 'months').format('YYYY/MM/DD');
    var dtFin = moment().format('YYYY/MM/DD');
    solicitudesVM.fFechaInicio(dtNow);
    solicitudesVM.fFechaFin(dtFin);

    $('.input-group.date').datepicker({
        format: "yyyy/mm/dd",
        language: "es",
        autoclose: true,
        todayHighlight: true
    });

    solicitudesVM.obtenerDirectores();
    solicitudesVM.obtenerSolicitudes("Inicial");
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