function procesoRegistroViewModel() {

    var self = this;

    self.ip = ko.observable();

    self.update = ko.observable(false);
    self.estadoOpInt = ko.observable(0);
    self.estadoDeclaro = ko.observable(0);
    self.estadoForm = ko.observable(0);
    self.estadoContrato = ko.observable(0);

    self.step1 = ko.observable(false);
    self.step2 = ko.observable(false);
    self.step3 = ko.observable(false);
    self.step4 = ko.observable(false);
    self.step5 = ko.observable(false);

    self.textModal = ko.observable();
    self.titleModal = ko.observable();
    self.estadoCargue = ko.observable();
    self.estadoProspecto = ko.observable();
    self.proceso = ko.observable();
    self.correoElectronico = ko.observable();
    self.numeroDocumento = ko.observable(localStorage.getItem("nDoc"));
    self.telefono = ko.observable('333');
    self.ciudad = ko.observable('ciudad');
    self.dia = ko.observable(moment().format('D'));
    self.mes = ko.observable(moment().format('M'));
    self.anio = ko.observable(moment().format('YYYY'));

    self.fechaNacimiento = ko.observable();

    self.paisId = ko.observable();
    self.epsId = ko.observable();
    self.pensionId = ko.observable();
    self.profesionId = ko.observable();
    self.nivelEducativoId = ko.observable();
    self.direccion = ko.observable();
    self.barrio = ko.observable();
    self.terminosCondiciones = ko.observable(false);
    self.declaro = ko.observable(false);

    self.recursos = ko.observable();
    self.prSlft1 = ko.observable(false);
    self.prSlft2 = ko.observable(false);
    self.prSlft3 = ko.observable(false);
    self.prSlft4 = ko.observable(false);
    self.prSlft5 = ko.observable(false);
    self.prSlft6 = ko.observable(false);

    self.anios = ko.observable(0);
    self.primerSector = ko.observable();
    self.segundoSector = ko.observable();

    self.paises = ko.observableArray();
    self.eps = ko.observableArray();
    self.pensiones = ko.observableArray();
    self.nivelEducativo = ko.observableArray();
    self.profesion = ko.observableArray();
    self.notas = ko.observableArray();
    self.notasCapacitacion = ko.observableArray();
    self.archivosPendientes = ko.observableArray();
    self.estadoArchivos = ko.observableArray();
    self.nombresApellidos = ko.observableArray();
    self.dataModify = ko.observableArray();
    self.procesoRegistro = ko.observableArray();
    self.representanteLegal = ko.observableArray();

    self.pr1 = ko.observable();
    self.pr2 = ko.observable();
    self.pr3 = ko.observable();
    self.pr4 = ko.observable();
    self.pr5 = ko.observable();
    self.pr6 = ko.observable();
    self.pr7 = ko.observable();
    self.pr8 = ko.observable();
    self.pr9 = ko.observable();
    self.pr10 = ko.observable();
    self.pr11 = ko.observable();
    self.pr12 = ko.observable();
    self.pr13 = ko.observable();
    self.pr14 = ko.observable();

    self.hijo1 = ko.observable("");
    self.hijo2 = ko.observable("");
    self.hijo3 = ko.observable("");

    self.loadFile = function () {

        var files;
        var fileData = new FormData();
        var vArchivo = self.validarArchivos();

        if (self.estadoCargue() >= 1) {

            for (var a = 1; a < 8; a++) {
                if ($("#file" + a).attr("disabled") === undefined) {
                    if ($("#file" + a).val() === "") {
                        $("#dv-crtf").hide();
                        self.titleModal("Error cargue");
                        self.textModal("Por favor adjunte nuevamente el/los archivo(s) solicitado(s) para seguir con el proceso de vinculación a Emermédica.");
                        $("#mdl-fnsh").modal("show");
                        return;
                    }
                }
            }
            vArchivo = true;
        }

        if (vArchivo) {

            if (self.estadoCargue() >= 1) {

                for (var b = 0; b < self.archivosPendientes().length; b++) {

                    files = $("#" + self.archivosPendientes()[b].Input).get(0).files;

                    for (var c = 0; c < files.length; c++) {
                        fileData.append(self.archivosPendientes()[b].Input, files[c]);
                    }
                }

            } else {

                for (var d = 1; d < 8; d++) {

                    files = $("#file" + d).get(0).files;

                    for (var e = 0; e < files.length; e++) {
                        fileData.append("file" + d, files[e]);
                    }
                }
            }

            if (self.proceso() === "Contratación" && self.estadoProspecto() === "Devuelto")
                self.estadoCargue(7);

            $.ajax({
                type: "POST",
                url: "/Reclutamiento/UploadFiles?nd=" + self.numeroDocumento() + "&ea=" + self.estadoCargue(),
                dataType: "json",
                contentType: false,
                processData: false,
                data: fileData,
                success: function (result, status, xhr) {
                    if (result) {
                        $("#dv-crtf").hide();
                        self.titleModal("Cargue exitoso");
                        self.textModal("Se cargaron los archivos solicitados de manera exitosa, por favor estar pendiente del correo electrónico registrado.");
                        $("#mdl-fnsh").modal("show");

                        procesoRegistroVM.obtenerNotas();
                        procesoRegistroVM.reiniciarElementos();
                        procesoRegistroVM.obtenerEstadoArchivos();

                        $('input[type=file]').val("");
                        $('input[type=file]').prev('label').text('Seleccione un archivo');
                    }
                },
                error: function (xhr, status, error) {
                    $("#dv-crtf").hide();
                    self.titleModal("Error cargue");
                    self.textModal("Error al tratar de cargar los archivos, por favor inténtelo mas tarde.");
                    $("#mdl-fnsh").modal("show");
                }
            });

        } else {
            $("#dv-crtf").hide();
            self.titleModal("Error cargue");
            self.textModal("Por favor adjunte todos los archivos solicitados para realizar el proceso de vinculación a Emermédica.");
            $("#mdl-fnsh").modal("show");
        }
    };

    self.loadContrato = function () {

        var files;
        var fileData = new FormData();
        self.estadoCargue(8);

        if (self.update())
            self.estadoCargue(9);

        var fLength = $("#file8").get(0).files.length;

        if (fLength >= 1) {

            files = $("#file8").get(0).files;

            for (var e = 0; e < files.length; e++) {
                fileData.append("file8", files[e]);
            }

            $.ajax({
                type: "POST",
                url: "/Reclutamiento/UploadContrato?nd=" + self.numeroDocumento() + "&ea=" + self.estadoCargue(),
                dataType: "json",
                contentType: false,
                processData: false,
                data: fileData,
                success: function (result, status, xhr) {
                    if (result) {

                        self.update(false);

                        $("#dv-crtf").hide();
                        self.titleModal("Cargue exitoso");
                        self.textModal("Se cargo el contrato de manera exitosa, se le notificará la continuidad del proceso en el correo electrónico registrado.");
                        $("#mdl-fnsh").modal("show");

                        procesoRegistroVM.obtenerNotas();
                        procesoRegistroVM.reiniciarElementos();
                        procesoRegistroVM.obtenerEstadoArchivos();

                        $('input[type=file]').val("");
                        $('input[type=file]').prev('label').text('Seleccione un archivo');
                    }
                },
                error: function (xhr, status, error) {
                    $("#dv-crtf").hide();
                    self.titleModal("Error cargue");
                    self.textModal("Error al tratar de cargar los archivos, por favor inténtelo mas tarde.");
                    $("#mdl-fnsh").modal("show");
                }
            });

        } else {
            $("#dv-crtf").hide();
            self.titleModal("Error cargue");
            self.textModal("Por favor adjunte todos los archivos solicitados para realizar el proceso de vinculación a Emermédica.");
            $("#mdl-fnsh").modal("show");
        }
    };

    self.verifyArray = function (obj) {
        var size = 0, key;
        for (key in obj) {
            if (obj.hasOwnProperty(key)) size++;
        }
        return size;
    };

    self.obtenerProceso = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerProcesoReclutamiento/',
            data: { numeroDocumento: self.numeroDocumento() },
            dataType: 'json',
            success: function (result, status, xhr) {

                if (result.length >= 1) {

                    if (result[0].EstadoArchivos >= 1) {

                        $(".ft-step").removeClass("ft-step");
                        $("#sp-st-1").addClass("ft-step");

                        self.obtenerEstadoArchivos();
                        self.step1(true);
                        self.step2(false);
                        self.step3(false);
                        self.step4(false);
                        self.step5(false);

                        return;
                    }

                    if (result[0].InformacionPersonal === false
                        || result[0].Sarlaft === false
                        || result[0].ExperienciaComercial === false
                        || result[0].Contrato === false
                        || result[0].CertificacionTributaria === false) {

                        $(".ft-step").removeClass("ft-step");
                        $("#sp-st-2").addClass("ft-step");

                        self.update(true);
                        self.formRegistro();
                    }
                }
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerNotas = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerNotasReclutamiento/',
            data: { numeroDocumento: self.numeroDocumento() },
            dataType: 'json',
            success: function (result, status, xhr) {

                self.proceso(result[0].Proceso);
                self.estadoProspecto(result[0].EstadoProspecto);

                var size = self.verifyArray(self.dataModify());

                if (self.proceso() === "Gestión de contrato" && self.estadoProspecto() === "Devuelto")
                    self.proceso("Completar Registro");

                //PASO 1
                if (self.proceso() === "Pendiente" || self.proceso() === "Envio de documentos" || self.proceso() === "Gestión de contrato") {

                    $(".ft-step").removeClass("ft-step");
                    $("#sp-st-1").addClass("ft-step");

                    procesoRegistroVM.obtenerEstadoArchivos();

                    if (self.proceso() === "Gestión de contrato" && self.estadoProspecto() === "") {
                        self.HabilitarCargueContrato();
                    }
                    else if (size >= 1) {
                        self.HabilitarCargueContrato();
                    }

                    self.step1(true);
                    self.step2(false);
                    self.step3(false);
                    self.step4(false);
                    self.step5(false);
                }

                //PASO 2
                if (self.proceso() === "Completar Registro") {
                    $(".ft-step").removeClass("ft-step");
                    $("#sp-st-2").addClass("ft-step");
                    self.formRegistro();
                }

                //PASO 3
                if (self.proceso() === "Capacitación") {
                    $(".ft-step").removeClass("ft-step");
                    $("#sp-st-3").addClass("ft-step");
                    self.obtenerNotaCapacitacion();
                    self.step1(false);
                    self.step2(false);
                    self.step3(true);
                }

                if (self.proceso() === "Contratación" && self.estadoProspecto() === "Pendiente")
                {
                    self.obtenerNotaCapacitacion();
                    self.obtenerProceso();

                    if (self.update()) {

                        $(".ft-step").removeClass("ft-step");
                        $("#sp-st-1").addClass("ft-step");

                        self.HabilitarCargueContrato();
                        self.step1(true);
                        self.step2(false);
                        self.step3(false);
                        self.step4(false);
                        self.step5(false);
                        return;
                    }

                    if (!self.update()) {
                        $(".ft-step").removeClass("ft-step");
                        $("#sp-st-3").addClass("ft-step");
                        self.step1(false);
                        self.step2(false);
                        self.step3(true);
                        self.estadoProspecto("APROBADO");
                    }
                }

                //PASO 4
                if (self.proceso() === "Contratación" && self.estadoProspecto() === "Devuelto") {
                    self.obtenerProceso();
                }

                //PASO FINAL
                if (self.proceso() === "Contratación" && self.estadoProspecto() === "Aprobado") {

                    $(".ft-step").removeClass("ft-step");
                    $("#sp-st-4").addClass("ft-step");

                    self.step1(false);
                    self.step2(false);
                    self.step3(false);
                    self.step4(false);
                    self.step5(true);
                }
                self.correoElectronico(result[0].CorreoElectronico);
                self.nombresApellidos(result[0].Nombres + ' ' + result[0].Apellidos);
                self.telefono(result[0].Telefono);
                self.ciudad(result[0].Ciudad);
                self.notas(result);

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.formRegistro = function () {

        self.obtenerPaises();
        self.obtenerEps();
        self.obtenerPensiones();
        self.obtenerNivelEducativo();
        self.obtenerProfesiones();

        self.step1(false);
        self.step2(false);
        self.step3(false);
        self.step4(true);
        $('#fuenteIngresos').bootstrapToggle(
            {
                on: 'Si',
                off: 'No',
                size: 'mini'
            });

        var dtNow = moment().subtract(18, 'years').format('YYYY/MM/DD');
        procesoRegistroVM.fechaNacimiento(dtNow);

        $('.input-group.date').datepicker({
            format: "yyyy/mm/dd",
            language: "es",
            autoclose: true,
            todayHighlight: true
        });
    };

    self.HabilitarCargueContrato = function () {

        window.setTimeout(function () {

            $("#dv-btn-load-files").remove();

            $("#dv-file8").show();
            $("#file8").removeAttr("disabled");
            $("#btn-load-contrato").removeAttr("disabled");
            $("#dv-btn-load-contrato").removeClass("dv-disabled-btn").show();

            if (self.estadoContrato() === 0) {
                self.obtenerReporte();
                self.estadoContrato(1);
            }

        }, 100);
    };

    self.ObtenerInformacionCompleta = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerInfoCompletaReclutamiento/',
            data: { numeroDocumento: self.numeroDocumento() },
            dataType: 'json',
            success: function (result, status, xhr) {

                self.dataModify(result[0]);

                //FORM1
                self.fechaNacimiento(moment(self.dataModify().FechaNacimiento).format('YYYY/MM/DD'));
                self.paisId(self.dataModify().IdPais);
                $("#estrato").val(self.dataModify().Estrato);
                self.direccion(self.dataModify().Direccion);
                self.barrio(self.dataModify().Barrio);
                self.epsId(self.dataModify().IdEps);
                self.pensionId(self.dataModify().IdReclutamientoPensiones);
                $("#estadoCivil").val(self.dataModify().EstadoCivil);
                self.nivelEducativoId(self.dataModify().IdNivelEducativo);
                self.dataModify().FuentesIngresos === true ? $('#fuenteIngresos').bootstrapToggle('on') : $('#fuenteIngresos').bootstrapToggle('off');
                self.terminosCondiciones(self.dataModify().TerminosCondicionesI);

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerPaises = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerPaisesReclutamiento/',
            dataType: 'json',
            success: function (result, status, xhr) {
                self.paises(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerEps = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerEpsReclutamiento/',
            dataType: 'json',
            success: function (result, status, xhr) {
                self.eps(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerRepresentanteLegal = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerRepresentanteLegal/',
            dataType: 'json',
            success: function (result) {
                self.representanteLegal(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerPensiones = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerPensionesReclutamiento/',
            dataType: 'json',
            success: function (result, status, xhr) {
                self.pensiones(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerNivelEducativo = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerNivelEducativoReclutamiento/',
            dataType: 'json',
            success: function (result, status, xhr) {
                self.nivelEducativo(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerProfesiones = function (state) {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerProfesionesReclutamiento/',
            dataType: 'json',
            success: function (result, status, xhr) {
                self.profesion(result);

                window.setTimeout(function () {
                    if (self.estadoProspecto() === "Devuelto" || self.update())
                        self.ObtenerInformacionCompleta();
                }, 1500);

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.validarArchivos = function () {

        var fLength;

        for (var i = 1; i < 8; i++) {

            console.log("file" + i);

            fLength = $("#file" + i).get(0).files.length;

            if (i !== 5 && i !== 6)
                if (fLength === 0)
                return false;
        }

        return true;
    };

    self.obtenerEstadoArchivos = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerEstadoArchivo/',
            data: { numeroDocumento: self.numeroDocumento() },
            dataType: 'json',
            success: function (result, status, xhr) {

                self.estadoArchivos(result);
                self.estadoCargue(result.length);

                if (result.length === 0) {
                    $('input[type=file]').val("");
                    $('input[type=file]').removeAttr("disabled");
                    $(".dv-disabled").removeClass("dv-disabled");
                    $("#btn-load-files").removeAttr("disabled");
                    $("#dv-btn-load-files").removeClass("dv-disabled-btn");
                }

                for (var i = 0; i < result.length; i++) {

                    if (result[i].EstadoArchivo === "Recargar") {

                        self.archivosPendientes().push(result[i]);

                        $("#" + result[i].Input).removeAttr("disabled");
                        $("#" + result[i].Input).removeClass("cursor-not");
                        $("#dv-" + result[i].Input).removeClass("dv-disabled");
                        
                        $("#btn-load-files").removeAttr("disabled");
                        $("#dv-btn-load-files").removeClass("dv-disabled-btn");
                    }
                }

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerNotaCapacitacion = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerNotasCapacitacion/',
            data: { numeroDocumento: self.numeroDocumento() },
            dataType: 'json',
            success: function (result, status, xhr) {

                self.notasCapacitacion(result);

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.validarIngresoProspecto = function (validate) {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ValidarIngresoProspecto/',
            data: { numeroDocumento: self.numeroDocumento(), validate: validate },
            dataType: 'json',
            success: function (result, status, xhr) {

                if (result)
                    window.location.href = URI + 'Reclutamiento/Index';

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.reiniciarElementos = function () {

        for (var i = 1; i < 9; i++) {

            $("#file" + i).attr("disabled", "disabled");
            $("#file" + i).addClass("cursor-not");
            $("#dv-file" + i).addClass("dv-disabled");

            $("#btn-load-files").removeAttr("data-bind");
            $("#btn-load-files").attr("disabled", "disabled");
            $("#dv-btn-load-files").addClass("dv-disabled-btn");

            $("#btn-load-contrato").hide();
        }
    };

    self.archivo = function (obj, event) {

        var id = event.currentTarget.id;

        var t = $("#" + id).val();
        var labelText = 'ARCHIVO : ' + t.substr(12, t.length);
        $("#" + id).prev('label').text(labelText);
    };

    self.anterior = function () {

        if (self.estadoForm() === 2)
            self.estadoForm(1);

        if (self.estadoForm() === 4)
            self.estadoForm(3);

        if (self.estadoForm() === 5) {

            $("#dv-form-4").fadeOut(function () {
                $("#bagde-rw-2").addClass("hidden");
                $("#bagde-rw-1").removeClass("hidden");
                $("#dv-form-3").fadeIn();
            });
            self.estadoForm(4);
        }

        if (self.estadoForm() === 3) {

            $("#dv-form-3").fadeOut(function () {
                $("#enviar").hide();
                $("#siguiente").show();
                $("#dv-form-2").fadeIn();
            });

            $("#st-3").removeClass("step-ac");
            $("#bd-3").addClass("badge-step").removeClass("badge-step-ac");

            self.estadoForm(2);
        }

        if (self.estadoForm() === 1) {

            $("#dv-form-2").fadeOut(function () {
                $("#dv-form-1").fadeIn();
            });

            $("#st-2").removeClass("step-ac");
            $("#bd-2").addClass("badge-step").removeClass("badge-step-ac");

            self.estadoForm(0);
        }

        if (self.estadoForm() === 7) {

            $("#dv-form-5").fadeOut(function () {
                $("#st-5").removeClass("step-ac");
                $("#bd-5").removeClass("badge-step-ac").addClass("badge-step");
                $("#dv-form-4").fadeIn();
            });
            self.estadoForm(5);
        }
    };

    self.siguiente = function () {

        var chk = false;

        if (self.estadoForm() === 1)
            self.estadoForm(2);

        if (self.estadoForm() === 3)
            self.estadoForm(4);

        if (self.estadoForm() === 5)
            self.estadoForm(6);

        if (self.estadoForm() === 0) {

            var estrato = $("#estrato").val();
            var estadoCivil = $("#estadoCivil").val();

            if (self.fechaNacimiento() === null || self.fechaNacimiento() === undefined)
                $('#fechaNacimiento').addClass('error-vl');

            if (self.paisId() === null || self.paisId() === undefined)
                $('#pais').addClass('error-vl');

            if (estrato === "")
                $('#estrato').addClass('error-vl');

            if (self.direccion() === "" || self.direccion() === undefined)
                $('#direccion').addClass('error-vl');

            if (self.barrio() === "" || self.barrio() === undefined)
                $('#barrio').addClass('error-vl');

            if (self.epsId() === null || self.epsId() === undefined)
                $('#eps').addClass('error-vl');

            if (self.pensionId() === null || self.pensionId() === undefined)
                $('#pensiones').addClass('error-vl');

            if (estadoCivil === "")
                $('#estadoCivil').addClass('error-vl');

            if (self.nivelEducativoId() === null || self.nivelEducativoId() === undefined)
                $('#nivelEducativo').addClass('error-vl');

            if (self.fechaNacimiento() && self.paisId() && estrato && self.direccion() && self.barrio() && self.epsId() && self.pensionId() && estadoCivil && self.nivelEducativoId()) {

                $("#dv-form-1").fadeOut(function () {
                    $("#dv-form-2").fadeIn();
                });

                $('.slf-tgl').bootstrapToggle({
                    on: 'Si',
                    off: 'No',
                    size: 'mini',
                    width: 45
                });

                if (self.estadoProspecto() === "Devuelto" || self.update()) {
                    self.profesionId(self.dataModify().IdNivelEducativoSarlaft);
                    self.recursos(self.dataModify().DescripcionRecursos);
                    self.dataModify().ManejaRecursosPublicos === true ? $('#prSlft1').bootstrapToggle('on') : $('#prSlft1').bootstrapToggle('off');
                    self.dataModify().EsServidorPublico === true ? $('#prSlft2').bootstrapToggle('on') : $('#prSlft2').bootstrapToggle('off');
                    self.dataModify().OstentaAlgunGradoDePoderPublico === true ? $('#prSlft3').bootstrapToggle('on') : $('#prSlft3').bootstrapToggle('off');
                    self.dataModify().TieneCondicionPersonaExpuestaPoliticamente === true ? $('#prSlft4').bootstrapToggle('on') : $('#prSlft4').bootstrapToggle('off');
                    self.dataModify().RealizaOperacionesInternacionales === true ? $('#prSlft5').bootstrapToggle('on') : $('#prSlft5').bootstrapToggle('off');

                    if (self.dataModify().RealizaOperacionesInternacionales) {
                        $("#opInternacionales").show();
                        $('#opInternacionales option[value="' + self.dataModify().Ooperaciones + '"]').prop('selected', true);
                    }

                    self.dataModify().GozaReconocimientoPublico === true ? $('#prSlft6').bootstrapToggle('on') : $('#prSlft6').bootstrapToggle('off');
                }

                $("#st-2").addClass("step-ac");
                $("#bd-2").addClass("badge-step-ac").removeClass("badge-step");

                $('#prSlft5').change(function () {

                    chk = $(this).prop('checked');

                    if (chk) {
                        self.estadoOpInt(1);
                        $("#opInternacionales").show();
                    }
                    else {
                        self.estadoOpInt(0);
                        $("#opInternacionales").hide();
                    }

                });

                self.estadoForm(1);
            }
        }

        if (self.estadoForm() === 2) {

            if (self.estadoOpInt() === 1)
                if ($("#opInternacionales").val() === "") {
                    $('#opInternacionales').addClass('error-vl');
                    return;
                }

            if (self.recursos() === "" || self.recursos() === undefined)
                $('#recursos').addClass('error-vl');

            if (self.profesionId() === null || self.profesionId() === undefined)
                $('#profesion').addClass('error-vl');

            if (self.recursos() && self.profesionId()) {

                $("#dv-form-2").fadeOut(function () {
                    $("#dv-form-3").fadeIn();
                });

                if (self.estadoProspecto() === "Devuelto" || self.update()) {
                    self.anios(self.dataModify().AniosExperiencia);
                    $("input[name='rg1'][value='" + self.dataModify().PrimerSector + "']").prop('checked', true);
                    $("input[name='rg2'][value='" + self.dataModify().SegundoSector + "']").prop('checked', true);
                }

                $("#st-3").addClass("step-ac");
                $("#bd-3").addClass("badge-step-ac").removeClass("badge-step");

                self.estadoForm(3);
            }
        }

        if (self.estadoForm() === 4) {

            if (self.anios() === 0) {
                self.titleModal("Advertencia");
                self.textModal("Por favor ingrese un año de experiencia comercial diferente a cero (0).");
                $("#mdl-fnsh").modal("show");
                return;
            }

            var rg1 = $("input[name='rg1']:checked").val();
            var rg2 = $("input[name='rg2']:checked").val();

            if (rg1 === "otro1") {

                if (procesoRegistroVM.primerSector() === undefined || procesoRegistroVM.primerSector() === "") {
                    $('#fr-otro1').addClass('error-vl');
                    return;
                }
            }

            if (rg2 === "otro2") {

                if (procesoRegistroVM.segundoSector() === undefined || procesoRegistroVM.segundoSector() === "") {
                    $('#fr-otro2').addClass('error-vl');
                    return;
                }
            }

            $("#dv-form-3").fadeOut(function () {

                self.obtenerRepresentanteLegal();

                $("#bagde-rw-1").addClass("hidden");
                $("#bagde-rw-2").removeClass("hidden");

                $('#dv-contr').scrollTop(0);

                $("#dv-form-4").fadeIn();

                if (self.estadoDeclaro() === 0)
                    $(".btn-sig").addClass("dv-rw-declaro");
                else
                    $(".btn-sig").removeClass("dv-rw-declaro");
            });

            $('#dv-contr').bind('scroll', function () {
                if ($(this).scrollTop() + $(this).outerHeight() >= $(this)[0].scrollHeight) {
                    $("#dv-rw-declaro").removeClass("dv-rw-declaro");

                    if (self.declaro())
                        $(".btn-sig").removeClass("dv-rw-declaro");
                    else
                        $(".btn-sig").addClass("dv-rw-declaro");
                }
                else {
                    $(".btn-sig").addClass("dv-rw-declaro");
                    $("#dv-rw-declaro").addClass("dv-rw-declaro");
                }
            });

            $('#declaro').change(function () {
                if (this.checked) {
                    self.guardarTrazaDeclaro();

                    if (self.estadoDeclaro() === 1)
                        $(".btn-sig").removeClass("dv-rw-declaro");
                }
                else {
                    $(".btn-sig").addClass("dv-rw-declaro");
                }
            });

            self.estadoForm(5);
        }

        if (self.estadoForm() === 6) {

            $("#dv-form-4").fadeOut(function () {
                $("#st-5").addClass("step-ac");
                $("#bd-5").addClass("badge-step-ac").removeClass("badge-step");
                $("#dv-form-5").fadeIn();

                if (self.estadoProspecto() === "Devuelto" || self.update()) {

                    self.pr1(self.dataModify().Pregunta1 === true ? "1": "0");
                    self.dataModify().Pregunta1 === true ? $("input[name='rgp1'][value='1']").prop('checked', true) : $("input[name='rgp1'][value='0']").prop('checked', true);
                    self.pr2(self.dataModify().Pregunta2 === true ? "1" : "0");
                    self.dataModify().Pregunta2 === true ? $("input[name='rgp2'][value='1']").prop('checked', true) : $("input[name='rgp2'][value='0']").prop('checked', true);
                    self.pr3(self.dataModify().Pregunta3 === true ? "1" : "0");
                    self.dataModify().Pregunta3 === true ? $("input[name='rgp3'][value='1']").prop('checked', true) : $("input[name='rgp3'][value='0']").prop('checked', true);
                    self.pr4(self.dataModify().Pregunta4 === true ? "1" : "0");
                    self.dataModify().Pregunta4 === true ? $("input[name='rgp4'][value='1']").prop('checked', true) : $("input[name='rgp4'][value='0']").prop('checked', true);
                    self.pr5(self.dataModify().Pregunta5 === true ? "1" : "0");
                    self.dataModify().Pregunta5 === true ? $("input[name='rgp5'][value='1']").prop('checked', true) : $("input[name='rgp5'][value='0']").prop('checked', true);
                    self.pr6(self.dataModify().Pregunta6 === true ? "1" : "0");
                    self.dataModify().Pregunta6 === true ? $("input[name='rgp6'][value='1']").prop('checked', true) : $("input[name='rgp6'][value='0']").prop('checked', true);
                    self.pr7(self.dataModify().Pregunta7 === true ? "1" : "0");
                    self.dataModify().Pregunta7 === true ? $("input[name='rgp7'][value='1']").prop('checked', true) : $("input[name='rgp7'][value='0']").prop('checked', true);
                    self.pr8(self.dataModify().Pregunta8 === true ? "1" : "0");
                    self.dataModify().Pregunta8 === true ? $("input[name='rgp8'][value='1']").prop('checked', true) : $("input[name='rgp8'][value='0']").prop('checked', true);
                    self.pr9(self.dataModify().Pregunta9 === true ? "1" : "0");
                    self.dataModify().Pregunta9 === true ? $("input[name='rgp9'][value='1']").prop('checked', true) : $("input[name='rgp9'][value='0']").prop('checked', true);
                    self.pr10(self.dataModify().Pregunta10 === true ? "1" : "0");
                    self.dataModify().Pregunta10 === true ? $("input[name='rgp10'][value='1']").prop('checked', true) : $("input[name='rgp10'][value='0']").prop('checked', true);
                    self.pr11(self.dataModify().PreguntaD1 === true ? "1" : "0");
                    self.dataModify().PreguntaD1 === true ? $("input[name='rgp11'][value='1']").prop('checked', true) : $("input[name='rgp11'][value='0']").prop('checked', true);
                    self.pr12(self.dataModify().PreguntaD2 === true ? "1" : "0");
                    self.dataModify().PreguntaD2 === true ? $("input[name='rgp12'][value='1']").prop('checked', true) : $("input[name='rgp12'][value='0']").prop('checked', true);
                    self.pr13(self.dataModify().PreguntaD3 === true ? "1" : "0");
                    self.dataModify().PreguntaD3 === true ? $("input[name='rgp13'][value='1']").prop('checked', true) : $("input[name='rgp13'][value='0']").prop('checked', true);
                    self.pr14(self.dataModify().PreguntaD4 === true ? "1" : "0");
                    self.dataModify().PreguntaD4 === true ? $("input[name='rgp14'][value='1']").prop('checked', true) : $("input[name='rgp14'][value='0']").prop('checked', true);

                    self.hijo1(self.dataModify().NumeroHijos1);
                    self.hijo2(self.dataModify().NumeroHijos2);
                    self.hijo3(self.dataModify().NumeroHijos3);
                }
            });

            self.estadoForm(7);
        }
    };

    self.obtenerReporte = function () {

        $.ajax({
            url: '/Reclutamiento/ExportarContrato',
            type: 'POST',
            data: { numeroIdentificacion: self.numeroDocumento() },
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

    self.guardarTrazaDeclaro = function () {

        if (self.estadoDeclaro() === 0) {
            $.ajax({
                url: "/Reclutamiento/GuardarTrazaDeclaro?numeroDocumento=" + self.numeroDocumento() + "&ip=" + self.ip(),
                type: 'POST',
                dataType: 'json',
                contentType: "application/json",
                success: function (result) {
                    if (result) {
                        self.estadoDeclaro(1);
                        $(".btn-sig").removeClass("dv-rw-declaro");
                    }
                },
                error: function () {
                    console.log("Error");
                }
            });
        }
    };

    self.paisesChanged = function () {

        if (self.paisId() !== undefined && self.paisId() !== null)
            $('#pais').removeClass('error-vl');
        else
            $('#pais').addClass('error-vl');
    };

    self.estratoChanged = function () {

        var estratoId = $("#estrato").val();

        if (estratoId !== "" && estratoId !== null)
            $('#estrato').removeClass('error-vl');
        else
            $('#estrato').addClass('error-vl');
    };

    self.epsChanged = function () {

        if (self.epsId() !== undefined && self.epsId() !== null)
            $('#eps').removeClass('error-vl');
        else
            $('#eps').addClass('error-vl');
    };

    self.pensionesChanged = function () {

        if (self.pensionId() !== undefined && self.pensionId() !== null)
            $('#pensiones').removeClass('error-vl');
        else
            $('#pensiones').addClass('error-vl');
    };

    self.estadoCivilChanged = function () {

        var estadoCivilId = $("#estadoCivil").val();

        if (estadoCivilId !== "" && estadoCivilId !== null)
            $('#estadoCivil').removeClass('error-vl');
        else
            $('#estadoCivil').addClass('error-vl');
    };

    self.operacionesInternacionalesChanged = function () {

        var operacionesInterId = $("#opInternacionales").val();

        if (operacionesInterId !== "" && operacionesInterId !== null)
            $('#opInternacionales').removeClass('error-vl');
        else
            $('#opInternacionales').addClass('error-vl');
    };

    self.nivelEducativoChanged = function () {

        if (self.nivelEducativoId() !== undefined && self.nivelEducativoId() !== null)
            $('#nivelEducativo').removeClass('error-vl');
        else
            $('#nivelEducativo').addClass('error-vl');
    };

    self.profesionChanged = function () {

        if (self.profesionId() !== undefined && self.profesionId() !== null)
            $('#profesion').removeClass('error-vl');
        else
            $('#profesion').addClass('error-vl');
    };

    self.confirmarEnvio = function () {

        $("#mdl-confirm").modal('hide');

        var primerSector = '';
        var segundoSector = '';

        if ($("input[name='rg1']:checked").val() === "otro1")
            primerSector = self.primerSector();
        else
            primerSector = $("input[name='rg1']:checked").val();

        if ($("input[name='rg2']:checked").val() === "otro2")
            segundoSector = self.segundoSector();
        else
            segundoSector = $("input[name='rg2']:checked").val();

        var model = {
            NumeroDocumento: self.numeroDocumento()
            , FechaNacimiento: self.fechaNacimiento()
            , PaisId: self.paisId()
            , Estrato: $("#estrato").val()
            , Direccion: self.direccion()
            , Barrio: self.barrio()
            , EpsId: self.epsId()
            , PensionId: self.pensionId()
            , EstadoCivil: $("#estadoCivil").val()
            , NivelEducativoId: self.nivelEducativoId()
            , TerminosCondiciones: self.terminosCondiciones()
            , FuenteIngresos: $('#fuenteIngresos').prop('checked')
            , ProfesionId: self.profesionId()
            , Recursos: self.recursos()
            , PrSlft1: $("#prSlft1").prop('checked')
            , PrSlft2: $("#prSlft2").prop('checked')
            , PrSlft3: $("#prSlft3").prop('checked')
            , PrSlft4: $("#prSlft4").prop('checked')
            , PrSlft5: $("#prSlft5").prop('checked')
            , PrSlft6: $("#prSlft6").prop('checked')
            , Operaciones: $("#opInternacionales").val()
            , Anios: self.anios()
            , PrimerSector: primerSector
            , SegundoSector: segundoSector
            , Pregunta1: Boolean(parseInt(self.pr1()))
            , Pregunta2: Boolean(parseInt(self.pr2()))
            , Pregunta3: Boolean(parseInt(self.pr3()))
            , Pregunta4: Boolean(parseInt(self.pr4()))
            , Pregunta5: Boolean(parseInt(self.pr5()))
            , Pregunta6: Boolean(parseInt(self.pr6()))
            , Pregunta7: Boolean(parseInt(self.pr7()))
            , Pregunta8: Boolean(parseInt(self.pr8()))
            , Pregunta9: Boolean(parseInt(self.pr9()))
            , Pregunta10: Boolean(parseInt(self.pr10()))
            , Pregunta11: Boolean(parseInt(self.pr11()))
            , Pregunta12: Boolean(parseInt(self.pr12()))
            , Pregunta13: Boolean(parseInt(self.pr13()))
            , Pregunta14: Boolean(parseInt(self.pr14()))
            , Hijo1: self.hijo1()
            , Hijo2: self.hijo2()
            , Hijo3: self.hijo3()
        };

        var url = "";
        if (self.estadoProspecto() === "Devuelto" || self.update()) {
            url = "/Reclutamiento/ActualizarRegistro";
        } else {
            url = "/Reclutamiento/CompletarRegistro";
        } 

        $.ajax({
            url: url,
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(model),
            success: function (result) {

                if (result) {

                    //self.loadCertificado();
                    self.titleModal("¡Se enviaron tus datos correctamente!");
                    $("#msg-success").removeClass("hidden");
                    $("#mdl-complete").modal("show");
                }
                else {
                    $("#dv-crtf").hide();
                    self.titleModal("Registro no exitoso");
                    self.textModal("Ocurrio un error al tratar de completar el registro del usuario, por favor intente mas tarde.");
                    $("#mdl-fnsh").modal("show");
                }
            },
            error: function () {
                $("#dv-crtf").hide();
                self.titleModal("Registro no exitoso");
                self.textModal("Ocurrio un error al tratar de completar el registro del usuario, por favor intente mas tarde.");
                $("#mdl-fnsh").modal("show");
            }
        });
    };

    self.completarRegistro = function () {

        $('.error-vl-hj').removeClass('error-vl-hj');

        if (self.pr1() === undefined)
            $(".radiobtnp1").addClass("radiobtnp-err").focus();

        if (self.pr2() === undefined)
            $(".radiobtnp2").addClass("radiobtnp-err").focus();

        if (self.pr3() === undefined)
            $(".radiobtnp3").addClass("radiobtnp-err").focus();

        if (self.pr4() === undefined)
            $(".radiobtnp4").addClass("radiobtnp-err").focus();

        if (self.pr5() === undefined)
            $(".radiobtnp5").addClass("radiobtnp-err").focus();

        if (self.pr6() === undefined)
            $(".radiobtnp6").addClass("radiobtnp-err").focus();

        if (self.pr7() === undefined)
            $(".radiobtnp7").addClass("radiobtnp-err").focus();

        if (self.pr8() === undefined)
            $(".radiobtnp8").addClass("radiobtnp-err").focus();

        if (self.pr9() === undefined)
            $(".radiobtnp9").addClass("radiobtnp-err").focus();

        if (self.pr10() === undefined)
            $(".radiobtnp10").addClass("radiobtnp-err").focus();

        if (self.pr11() === undefined)
            $(".radiobtnp11").addClass("radiobtnp-err").focus();

        if (self.pr12() === undefined)
            $(".radiobtnp12").addClass("radiobtnp-err").focus();

        if (self.pr13() === undefined)
            $(".radiobtnp13").addClass("radiobtnp-err").focus();

        if (self.pr14() === undefined)
            $(".radiobtnp14").addClass("radiobtnp-err").focus();

        if (self.pr11() === "1" && self.hijo1() === "") {
            $('#frmhijo1').addClass('error-vl-hj').focus();
            return;
        }

        if (self.pr12() === "1" && self.hijo2() === "") {
            $('#frmhijo2').addClass('error-vl-hj').focus();
            return;
        }

        if (self.pr13() === "1" && self.hijo3() === "") {
            $('#frmhijo3').addClass('error-vl-hj').focus();
            return;
        }

        if (self.pr1() && self.pr2() && self.pr3() && self.pr4() && self.pr5() && self.pr6() && self.pr7() && self.pr8() && self.pr9() && self.pr10() && self.pr11() && self.pr12() && self.pr13() && self.pr14())
            $("#mdl-confirm").modal('show');
    };

    self.loadCertificado = function () {

        var file = $("#file-certificado").get(0).files;
        var fileData = new FormData();

        for (var i = 0; i < file.length; i++) {
            fileData.append("fileInput", file[i]);
        }

        $.ajax({
            type: "POST",
            url: "/Reclutamiento/UploadFiles?nd=" + self.numeroDocumento() + "&ea=certf",
            dataType: "json",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (result, status, xhr) {
                console.log("Load ok");
            },
            error: function (xhr, status, error) {
                console.log("error ", xhr);
            }
        });
    };

    self.clearData = function () {
        
        $("#mdl-complete").modal('hide');
        self.obtenerNotas();
    };

    self.getIp = function () {
        $.getJSON("https://api.ipify.org/?format=json", function (e) {
            self.ip(e.ip);
        });
    };
}

var procesoRegistroVM = new procesoRegistroViewModel();
ko.applyBindings(procesoRegistroVM);

$(document).ready(function () {

    window.history.pushState(null, "", window.location.href);
    window.onpopstate = function () {
        window.history.pushState(null, "", window.location.href);
    };

    procesoRegistroVM.getIp();
    procesoRegistroVM.obtenerNotas();

    var hFiles = $("#dv-files").height();
    $("#dv-track").css("height", hFiles);   

    window.setTimeout(function () {

        $("#dv-track-note").css({
            'height': $("#tr-track").height(),
            'overflow': 'auto'
        });

    }, 1000);
});

function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {

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

setInputFilter(document.getElementById("anios"), function (value) {
    return /^-?\d*$/.test(value);
});

function kuBarrio(data, event) {
    var id = event.currentTarget.id;
    var val = $("#" + id).val();

    if (val !== "") {
        $("#" + id).removeClass("error-vl");
    }
}

function kuDireccion(data, event) {
    var id = event.currentTarget.id;
    var val = $("#" + id).val();

    if (val !== "") {
        $("#" + id).removeClass("error-vl");
    }
}

function kuRecursos(data, event) {
    var id = event.currentTarget.id;
    var val = $("#" + id).val();

    if (val !== "") {
        $("#" + id).removeClass("error-vl");
    }
}

function radioGroup1(data, event) {

    var id = event.currentTarget.id;

    if (id === "otro1")
        $("#tr-otro-1").show(500);
    else
        $("#tr-otro-1").hide(500);

}

function radioGroup2(data, event) {

    var id = event.currentTarget.id;

    if (id === "otro2")
        $("#tr-otro-2").show(500);
    else
        $("#tr-otro-2").hide(500);
}

function radioGroup3(data, event) {

    var id = event.currentTarget.id;

    if (id !== "")
        $(".radiobtnp").removeClass("radiobtnp-err");
}