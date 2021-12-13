function IndexViewModel() {

    var self = this;
    self.fechaActual = ko.observable(moment(Date.now()).format('ll'));
    self.link = ko.observable();
    self.linkEdit = ko.observable();
    self.cedAsesor = ko.observable();
    self.textModal = ko.observable();
    self.titleModal = ko.observable();
    self.idSolicitud = ko.observable();
    self.stateClean = ko.observable(0);
    self.noAfiliados = ko.observable();
    self.noAfiliadosEdit = ko.observable();
    self.noContrato = ko.observable();
    self.noContratoEdit = ko.observable();
    self.noFormulario = ko.observable();
    self.noFormularioEdit = ko.observable();
    self.cedContratante = ko.observable();
    self.cedContratanteEdit = ko.observable();
    self.observaciones = ko.observable();
    self.solicitudes = ko.observableArray();
    self.archivosPopUp = ko.observableArray();
    self.filesDelete = ko.observableArray();

    self.idSolicitudGestionada = ko.observable();
    self.NumeroFormularioGestionada = ko.observable();
    self.CedulaAsesorGestionada = ko.observable();
    self.idUsuarioSolicitudGestionada = ko.observable();
    self.CedulaContratanteGestionada = ko.observable();
    self.ClaveMedioPagoGestionada = ko.observable();
    self.NumeroContratoGestionada = ko.observable();
    self.NumeroAfiliadosGestionada = ko.observable();
    self.LinkGestionada = ko.observable();
    self.TipoArchivoGestionada = ko.observable();
    self.causales = ko.observableArray();
    self.causalesHabilitadas = ko.observable(false);
    self.causalesSeleccionadas = ko.observableArray();
    self.accionSeleccionada = ko.observable(false);
    self.historicosSolicitudSeleccionada = ko.observableArray();
    self.causalesSolicitudSeleccionada = ko.observableArray();
    self.contentCausales = ko.observable();
    self.numeroReferenciaRequired = ko.observable();
    self.cedRequired = ko.observable();
    self.medioPagoRequired = ko.observable();
    self.tipoContratoRequired = ko.observable();
    self.numeroContratoRequired = ko.observable();
    self.observacionesRequired = ko.observable();
    self.accionRequired = ko.observable();
    self.motivosRechazoRequired = ko.observable();

    //Caracteres no permitidos nombre archivo
    var caracteresEspeciales = ["/", "&", "%", "$", "#", "!", "¡", "?", "¿", "+", "*"];


    self.closeModal = function () {

        self.link("");
        self.noAfiliados("");
        self.noFormulario("");
        self.cedContratante("");
        self.noContrato("");
        $("#sl-tipoContrato").val("");

        $("input:file").val("");
        $("#sl-tipoArchivo").val("");
        $("#dv-msg").addClass("hidden");
        $("#dv-num-contrato").addClass("hidden");
        $("#dv-link").addClass("hidden");
        $("#dv-upload").addClass("hidden");

        $("#mdl-upld").modal("hide");

        self.cedRequired('');
        self.numeroContratoRequired('');
        self.numeroReferenciaRequired('');
    };
    self.closeModalEdit = function () {

        self.filesDelete().forEach(function (value) {
            self.archivosPopUp().push(value);
        });
        self.linkEdit("");
        self.noAfiliadosEdit("");
        self.noFormularioEdit("");
        self.cedContratanteEdit("");
        self.filesDelete([]);

        $("input:file").val("");
        $("#sl-tipoArchivoEdit").val("");
        $("#sl-tipoContratoEdit").val("");
        $("#dv-msgEdit").addClass("hidden");
        $("#dv-linkEdit").addClass("hidden");
        $("#dv-uploadEdit").addClass("hidden");

        $("#mdl-gestion-solicitud").modal("hide");

        self.cedRequired('');
        self.numeroContratoRequired('');
        self.numeroReferenciaRequired('');
    };
    self.closeModalGestionSac = function () {
        self.NumeroContratoGestionada('');
        $("#sl-tipoContratoGestion").val('');
        $("#sl-medioPagoGestion").val('');

        self.cedRequired('');
        self.numeroContratoRequired('');
        self.observaciones('');
        self.causalesSeleccionadas([]);
        self.accionSeleccionada('');
    };
    self.changeFiles = function () {
        var valFileTpye = $("#sl-tipoArchivo").val();
        var fileType = $("#sl-tipoArchivo option:selected").text();

        var files;

        if (fileType === "Excel") {
            files = $("#fileInputEx").get(0).files;
        }
        if (fileType === "Imagen")
            files = $("#fileInputIm").get(0).files;

        var hayCaracteresEspeciales = false;
        $(files).each(function (indexFile, valueFile) {
            $(caracteresEspeciales).each(function (indexCaracter, caracter) {
                if (valueFile.name.includes(caracter))
                    hayCaracteresEspeciales = true;
            });
        });
        if (hayCaracteresEspeciales) {
            document.getElementById("fileInputEx").value = "";
            document.getElementById("fileInputIm").value = "";
            self.titleModal("Cargue no exitoso");
            self.textModal("El nombre del archivo adjunto no debe contener caracteres especiales (/&%$#!¡?+*)");
            $("#mdl-fnsh").modal("show");
        }
    };
    self.changeTipoContrato = function () {

        var valTipoContrato = $("#sl-tipoContrato").val();

        if (valTipoContrato !== "") {
            if (valTipoContrato === "fam-inc" || valTipoContrato === "ap-inc" || valTipoContrato === "ppe-inc") {
                $("#dv-num-contrato").removeClass("hidden");
            } else {
                $("#dv-num-contrato").addClass("hidden");
            }
        }
        else {
            $("#dv-num-contrato").addClass("hidden");
        }
    };
    self.changeTipoContratoEdit = function () {

        var valTipoContrato = $("#sl-tipoContratoEdit").val();

        if (valTipoContrato !== "") {
            if (valTipoContrato === "fam-inc" || valTipoContrato === "ap-inc" || valTipoContrato === "ppe-inc") {
                $("#dv-num-contrato-edit").removeClass("hidden");
            } else {
                $("#dv-num-contrato-edit").addClass("hidden");
            }
        }
        else {
            $("#dv-num-contrato-edit").addClass("hidden");
        }
    };
    self.changeTipoContratoGestionSac = function () {
        var valTipoContrato = $("#sl-tipoContratoGestion").val();

        if (valTipoContrato !== "") {
            if (valTipoContrato === "fam-inc" || valTipoContrato === "ap-inc" || valTipoContrato === "ppe-inc") {
                $("#dv-num-contrato-gestion").removeClass("hidden");
            } else {
                $("#dv-num-contrato-gestion").addClass("hidden");
            }
        }
        else {
            $("#dv-num-contrato-gestion").addClass("hidden");
        }
    };
    self.changeAccionSac = function () {
        var accion = self.accionSeleccionada();
        if (accion === "radicar") {
            $("#dv-medio-pago-gestion").removeClass('hidden');
        } else {
            $("#dv-medio-pago-gestion").addClass('hidden');
        }
    };
    self.changeTipoArchivo = function () {

        var valFileTpye = $("#sl-tipoArchivo").val();
        var fileType = $("#sl-tipoArchivo option:selected").text();

        if (valFileTpye !== "") {

            self.link("");
            $("input:file").val("");

            $("#dv-upload").removeClass("hidden");

            if (fileType === "Excel") {
                $("#fileInputIm").hide();
                $("#fileInputEx").show();
                $("#dv-link").removeClass("hidden");
            }
            if (fileType === "Imagen") {
                $("#fileInputIm").show();
                $("#fileInputEx").hide();
                $("#dv-link").addClass("hidden");
            }
        }
        else {
            $("#dv-link").addClass("hidden");
            $("#dv-upload").addClass("hidden");

        }
    };
    self.changeTipoArchivoEdit = function () {

        var valFileTpyeEdit = $("#sl-tipoArchivoEdit").val();
        var fileTypeEdit = $("#sl-tipoArchivoEdit option:selected").text();

        if (valFileTpyeEdit !== "") {

            self.linkEdit("");
            $("input:file").val("");

            $("#dv-uploadEdit").removeClass("hidden");

            if (fileTypeEdit === "Excel") {
                $("#fileInputImEdit").hide();
                $("#fileInputExEdit").show();
                $("#dv-linkEdit").removeClass("hidden");
            }
            if (fileTypeEdit === "Imagen") {
                $("#fileInputImEdit").show();
                $("#fileInputExEdit").hide();
                $("#dv-linkEdit").addClass("hidden");
            }
        }
        else {
            $("#dv-linkEdit").addClass("hidden");
            $("#dv-uploadEdit").addClass("hidden");

        }
    };

    self.uploadFiles = function (fileData, editar) {
        $.ajax({
            type: "POST",
            url: "/Digitalizacion/UploadFiles",
            dataType: "json",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (result, status, xhr) {

                if (result.length >= 1) {
                    if (editar)
                        self.registerInfoFileEdit(result);
                    else
                        self.registerInfoFile(result);
                }
            },
            error: function (xhr, status, error) {
                console.log("error ", xhr);
                self.titleModal("Cargue no exitoso");
                self.textModal("Error al tratar de cargar los archivos, por favor contáctese con el administrador del sistema.");
                $("#mdl-fnsh").modal("show");
            }
        });
    };

    self.loadFile = function () {

        var valFileTpye = $("#sl-tipoArchivo").val();
        var fileType = $("#sl-tipoArchivo option:selected").text();
        var valTipoContrato = $("#sl-tipoContrato").val();
        var tipoContrato = $("#sl-tipoContrato option:selected").text();
        self.numeroReferenciaRequired('');

        $("input").removeClass("error-vl");

        //$.ajax({
        //    url: "/ReferenciaPago/PaymentData",
        //    type: "POST",
        //    data: {
        //        referencia: self.noFormulario()
        //    },
        //    success: function (paymentData) {
        //        var resultado = JSON.parse(paymentData.Resultado);
        //        var validoNumeroCedula = (resultado.IsInclusion ?
        //            self.cedContratante() == resultado.ContractorDocument :
        //            self.cedContratante() == resultado.IdentificationNumber);

                if (self.noFormulario() === "" ||
                    self.noFormulario() === undefined) {
                    //$('#noFormulario').addClass('error-vl');
                }
                if ((self.cedContratante() !== '' &&
                    self.cedContratante() !== undefined &&
                    self.cedContratante() !== null)
                    //&& !validoNumeroCedula
                ) {
                    //$('#noFormulario').addClass('error-vl');
                    //self.numeroReferenciaRequired('El número de referencia no está relacionado con este número de cédula');
                }
                if (self.cedAsesor() === "" || self.cedAsesor() === undefined) {
                    $('#cedAsesor').addClass('error-vl');
                }
                if (self.cedContratante() === "" || self.cedContratante() === undefined) {
                    $('#cedContratante').addClass('error-vl');
                }
                else if (self.cedContratante().length < 5) {
                    $('#cedContratante').addClass('error-vl');
                    self.cedRequired('Mínimo 5 caracteres');
                }
                else if (self.cedContratante() <= 0) {
                    $('#cedContratante').addClass('error-vl');
                    self.cedRequired('Ingrese un número válido');
                }
                else
                    self.cedRequired('');

                if (self.noAfiliados() === "" || self.noAfiliados() === undefined) {
                    $('#noAfiliados').addClass('error-vl');
                }
                var esInclusion = false;
                if (valTipoContrato === "fam-inc" || valTipoContrato === "ap-inc" || valTipoContrato === "ppe-inc") {
                    esInclusion = true;
                    if (self.noContrato() === "" || self.noContrato() === undefined || self.noContrato().length < 4) {
                        $('#noContrato').addClass('error-vl');
                        self.numeroContratoRequired("El número de contrato debe tener mínimo 4 caracteres");
                    } else {
                        $('#noContrato').removeClass('error-vl');
                        self.numeroContratoRequired('');
                    }
                }

                if (self.noFormulario() && self.cedAsesor() && self.cedContratante() &&
                    self.cedContratante().length >= 5 && self.noAfiliados()
                    && ((esInclusion && self.noContrato() && !(self.noContrato().length < 4)) || (!esInclusion)
                    //&& validoNumeroCedula
                )
                ) {
                    var files;

                    if (fileType === "Excel") {

                        if (self.link() === "" || self.link() === undefined) {
                            $('#link').addClass('error-vl');
                            return false;
                        }

                        files = $("#fileInputEx").get(0).files;
                    }
                    if (fileType === "Imagen")
                        files = $("#fileInputIm").get(0).files;

                    var fileData = new FormData();

                    if (valFileTpye !== "" && valTipoContrato !== "") {
                        if (files.length >= 1) {
                            for (var i = 0; i < files.length; i++) {
                                fileData.append("fileInput", files[i]);
                            }
                            if (esInclusion)
                                $.ajax({
                                    type: 'GET',
                                    url: '/Digitalizacion/VerificarContrato/',
                                    data: { numeroContrato: self.noContrato() },
                                    dataType: 'json',
                                    success: function (result, status, xhr) {
                                        if (result)
                                            self.uploadFiles(fileData, false);
                                        else
                                            self.noExisteContrato();
                                    }
                                    ,
                                    error: function (jqXHR, textStatus, errorThrown) {
                                        console.log("error1 ", jqXHR);
                                    }
                                });
                            else
                                self.uploadFiles(fileData, false);

                        }
                        else {

                            $("#dv-msg").removeClass("hidden");

                            if (fileType === "Excel")
                                $("#tx-upd").text("Por favor seleccione un archivo excel.");
                            if (fileType === "Imagen")
                                $("#tx-upd").text("Por favor seleccione una o unas imagenes.");
                        }
                    }
                    else {
                        $("#dv-msg").removeClass("hidden");

                        if (valFileTpye === "")
                            $("#tx-upd").text("Por favor seleccione un tipo de archivo.");
                        else if (valTipoContrato === "")
                            $("#tx-upd").text("Por favor seleccione un tipo de contrato.");
                        else if (esInclusion && !self.noContrato())
                            $("#tx-upd").text("Por favor proporcione un número de contrato.");
                    }
                }
        //    }
        //});


    };

    self.prvArch = ko.observable();
    self.deleteFiles = function (data) {
        self.archivosPopUp.remove(data);
        self.filesDelete.push(data);
    };

    self.EditFile = function () {

        var valFileTpyeEdit = $("#sl-tipoArchivoEdit").val();
        var fileTypeEdit = $("#sl-tipoArchivoEdit option:selected").text();
        var valTipoContrato = $("#sl-tipoContratoEdit").val();
        var tipoContrato = $("#sl-tipoContratoEdit option:selected").text();
        self.numeroReferenciaRequired('');

        $("input").removeClass("error-vl");

        //$.ajax({
        //    url: "/ReferenciaPago/PaymentData",
        //    type: "POST",
        //    data: {
        //        referencia: self.noFormularioEdit()
        //    },
        //    success: function (paymentData) {
        //        var resultado = JSON.parse(paymentData.Resultado);
        //        var validoNumeroCedula = (resultado.IsInclusion ?
        //            self.cedContratanteEdit() == resultado.ContractorDocument :
        //            self.cedContratanteEdit() == resultado.IdentificationNumber);

                if ((self.cedContratanteEdit() !== '' &&
                    self.cedContratanteEdit() !== undefined &&
                    self.cedContratanteEdit() !== null)
                    //&& !validoNumeroCedula
                ) {
                    //$("#noFormularioEdit").addClass('error-vl');
                    //self.numeroReferenciaRequired('El número de referencia no está relacionado con este número de cédula');
                }
                if (self.noFormularioEdit() === "" || self.noFormularioEdit() === undefined) {
                    //$('#noFormularioEdit').addClass('error-vl');
                }
                if (self.cedAsesor() === "" || self.cedAsesor() === undefined) {
                    $('#cedAsesorEdit').addClass('error-vl');
                }
                if (self.cedContratanteEdit() === "" || self.cedContratanteEdit() === undefined) {
                    $('#cedContratanteEdit').addClass('error-vl');
                }
                else if (self.cedContratanteEdit().length < 5) {
                    $('#cedContratanteEdit').addClass('error-vl');
                    self.cedRequired('Mínimo 5 caracteres');
                }
                else if (self.cedContratanteEdit() <= 0) {
                    $('#cedContratanteEdit').addClass('error-vl');
                    self.cedRequired('Ingrese un número válido');
                }
                else
                    self.cedRequired('');
                if (self.noAfiliadosEdit() === "" || self.noAfiliadosEdit() === undefined) {
                    $('#noAfiliadosEdit').addClass('error-vl');
                }

                var esInclusion = false;
                if (valTipoContrato === "fam-inc" || valTipoContrato === "ap-inc" || valTipoContrato === "ppe-inc") {
                    esInclusion = true;
                    if (self.noContratoEdit() === "" || self.noContratoEdit() === undefined || self.noContratoEdit().length < 4) {
                        $('#noContratoEdit').addClass('error-vl');
                        self.numeroContratoRequired("El número de contrato debe tener mínimo 4 caracteres");
                    } else {
                        $('#noContratoEdit').removeClass('error-vl');
                        self.numeroContratoRequired("");
                    }
                }

                if (
                    self.noFormularioEdit() !== "" && self.noFormularioEdit() !== undefined
                    && self.cedAsesor() !== "" && self.cedAsesor() !== undefined
                    && self.cedContratanteEdit() !== "" && self.cedContratanteEdit() !== undefined
                    && !(self.cedContratanteEdit().length < 5)
                    && self.noAfiliadosEdit() !== "" && self.noAfiliadosEdit() !== undefined
                    && ((esInclusion && self.noContratoEdit() !== "" && self.noContratoEdit() !== undefined && !(self.noContratoEdit().length < 4)) || (!esInclusion))
                    //&& validoNumeroCedula
                ) {

                    var filesEdit;

                    if (fileTypeEdit === "Excel") {

                        if (self.linkEdit() === "" || self.linkEdit() === undefined) {
                            $('#linkEdit').addClass('error-vl');
                            return false;
                        }

                        filesEdit = $("#fileInputExEdit").get(0).files;
                    }
                    if (fileTypeEdit === "Imagen")
                        filesEdit = $("#fileInputImEdit").get(0).files;

                    var fileDataEdit = new FormData();

                    if (valFileTpyeEdit !== "" && valTipoContrato !== "") {
                        if (filesEdit.length >= 1) {
                            for (var i = 0; i < filesEdit.length; i++) {
                                fileDataEdit.append("fileInput", filesEdit[i]);
                            }

                            if (esInclusion)
                                $.ajax({
                                    type: 'GET',
                                    url: '/Digitalizacion/VerificarContrato/',
                                    data: { numeroContrato: self.noContratoEdit() },
                                    dataType: 'json',
                                    success: function (result, status, xhr) {

                                        if (result)
                                            self.uploadFiles(fileDataEdit, true);
                                        else
                                            self.noExisteContrato();
                                    }
                                    ,
                                    error: function (jqXHR, textStatus, errorThrown) {
                                        console.log("error1 ", jqXHR);
                                    }
                                });
                            else
                                self.uploadFiles(fileDataEdit, true);
                        }
                        else {

                            $("#dv-msgEdit").removeClass("hidden");

                            if (fileTypeEdit === "Excel")
                                $("#tx-updEdit").text("Por favor seleccione un archivo excel.");
                            if (fileTypeEdit === "Imagen")
                                $("#tx-updEdit").text("Por favor seleccione una o unas imagenes.");
                        }
                    }
                    else {
                        $("#dv-msgEdit").removeClass("hidden");
                        $("#tx-updEdit").text("Por favor seleccione un tipo de archivo.");
                        if (valTipoContrato === "")
                            $("#tx-updEdit").text("Por favor seleccione un tipo de contrato.");
                        else if (esInclusion && !self.noContratoEdit())
                            $("#tx-updEdit").text("Por favor proporcione un número de contrato.");
                    }
                }
        //    }
        //});
    };

    self.registerInfoFile = function (fileInfo) {

        var valTipoContrato = $("#sl-tipoContrato").val();
        if (valTipoContrato === "fam-nuevo" || valTipoContrato === "ap-nuevo" || valTipoContrato === "ppe-nuevo")
            self.noContrato('');
        var fileType = $("#sl-tipoArchivo option:selected").text();

        $("#mdl-upld").css("cursor", "wait");

        $("#fileButton").attr("disabled", true);
        $("#close").attr("disabled", true);

        var model = {
            NumeroFormulario: self.noFormulario(),
            CedulaAsesor: self.cedAsesor(),
            CedulaContratante: self.cedContratante(),
            NumeroAfiliados: self.noAfiliados(),
            TipoArchivo: fileType,
            Link: self.link(),
            FileInfo: fileInfo,
            TipoContrato: valTipoContrato,
            NumeroContrato: self.noContrato()
        };
        var modelString = JSON.stringify(model);

        $.ajax({
            url: "/Digitalizacion/RegistrarSolicitud",
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            data: modelString,
            success: function (result, status, xhr) {

                $("#close").attr("disabled", false);
                $("#fileButton").attr("disabled", false);
                $("#mdl-upld").css("cursor", "default");

                if (result.Exitoso) {

                    self.titleModal("Cargue exitoso");
                    self.textModal("El registro de digitalización y cargue de los archivos se realizó de forma exitosa.");
                    $("#mdl-fnsh-accept").modal("show");

                    self.closeModal();
                    self.obtenerSolicitudesDigitalizacion();
                } else {
                    self.titleModal("Cargue Invalido");
                    self.textModal(result.Mensaje);
                    $("#mdl-fnsh-accept").modal("show");

                    self.closeModal();
                }
            },
            error: function (xhr, status, error) {

                console.log("error1 ", xhr);
                $("#close").attr("disabled", false);
                $("#fileButton").attr("disabled", false);
                $("#mdl-upld").css("cursor", "default");
                self.titleModal("Cargue no exitoso");
                self.textModal("El registro del nuevo cargue no se pudo realizar, por favor contáctese con el administrador del sistema.");
                $("#mdl-fnsh").modal("show");
            }
        });

    };

    self.registerInfoFileEdit = function (fileInfoEdit) {

        var fileTypeEdit = $("#sl-tipoArchivoEdit option:selected").text();

        $("#mdl-upldEdit").css("cursor", "wait");
        $("#saveButtonEdit").attr("disabled", true);
        $("#closeEdit").attr("disabled", true);
        var valTipoContrato = $("#sl-tipoContratoEdit").val();
        if (valTipoContrato === "fam-nuevo" || valTipoContrato === "ap-nuevo" || valTipoContrato === "ppe-nuevo")
            self.noContratoEdit();
        var model = {
            Id: self.idSolicitud(),
            NumeroFormulario: self.noFormularioEdit(),
            CedulaAsesor: self.cedAsesor(),
            CedulaContratante: self.cedContratanteEdit(),
            NumeroAfiliados: self.noAfiliadosEdit(),
            TipoArchivo: fileTypeEdit,
            Link: self.linkEdit(),
            FileInfo: fileInfoEdit,
            FileInfoEliminar: self.filesDelete(),
            //Observaciones: self.observaciones(),
            TipoContrato: valTipoContrato,
            NumeroContrato: self.noContratoEdit()
        };

        $.ajax({
            url: "/Digitalizacion/EditarSolicitud",
            type: 'POST',
            dataType: 'json',
            contentType: "application/json",
            data: JSON.stringify(model),
            success: function (result, status, xhr) {

                $("#closeEdit").attr("disabled", false);
                $("#saveButtonEdit").attr("disabled", false);
                $("#mdl-upldEdit").css("cursor", "default");

                if (result >= 1) {

                    self.titleModal("Cargue exitoso");
                    self.textModal("El registro de digitalización y cargue de los archivos se realizó de forma exitosa.");
                    $("#mdl-fnsh").modal("show");

                    self.closeModalEdit();
                    self.obtenerSolicitudesDigitalizacion();
                }
            },
            error: function (xhr, status, error) {

                console.log("error1 ", xhr);
                $("#closeEdit").attr("disabled", false);
                $("#saveButtonEdit").attr("disabled", false);
                $("#mdl-upldEdit").css("cursor", "default");
                self.titleModal("Cargue no exitoso");
                self.textModal("El registro del nuevo cargue no se pudo realizar, por favor contáctese con el administrador del sistema.");
                $("#mdl-fnsh").modal("show");
            }
        });

    };

    self.obtenerCedulaAsesor = function () {

        $.ajax({
            type: 'GET',
            url: '/Digitalizacion/ObtenerCedulaAsesor/',
            dataType: 'json',
            success: function (result, status, xhr) {
                self.cedAsesor(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerSolicitudesDigitalizacion = function () {
        self.clearTable();

        var fInicio = $("#dtIni").val();
        var fechaFin = $("#dtFin").val();

        $.ajax({
            type: 'GET',
            url: '/Digitalizacion/ObtenerSolicitudesDigitalizacion/',
            data: { fechaInicio: fInicio, fechaFin: fechaFin },
            dataType: 'json',
            success: function (result, status, xhr) {
                self.solicitudes(result);
                self.drawTable();
            }
            ,
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("error1 ", jqXHR);
            }
        });
    };

    self.getCausalesRechazo = function () {
        $.ajax({
            type: 'GET',
            url: '/Digitalizacion/ObtenerCausales/',
            data: { idEstado: 3 },
            dataType: 'json',
            success: function (result, status, xhr) {
                $.each(result, function (index, value) {
                    value.Seleccionado = ko.observable(false);
                });
                self.causales(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error1 ", jqXHR);
            }
        });
    };
    self.gestionarSolicitud = function (element) {
        var clave = element.ClaveTipoContrato === null ? "" : new String(element.ClaveTipoContrato).trim();
        self.linkEdit(element.LinkEdit);
        self.noAfiliadosEdit(element.NumeroAfiliados);
        self.noFormularioEdit(element.NumeroFormulario);
        self.cedContratanteEdit(element.CedulaContratante);
        $("#sl-tipoContratoEdit").val(clave);
        self.changeTipoContratoEdit();
        self.noContratoEdit(element.NumeroContrato_Inclusion);
        self.idSolicitud(element.Id);
        self.observaciones(element.Observaciones);
        self.archivosPopUp(element.Archivos);
        self.filesDelete([]);
        $("#mdl-gestion-solicitud").modal();
    };
    self.gestionarSolicitudSac = function (element) {

        var clave = element.ClaveTipoContrato === null ? "" : new String(element.ClaveTipoContrato).trim();

        self.idSolicitudGestionada(element.Id);
        self.NumeroFormularioGestionada(element.NumeroFormulario);
        self.idUsuarioSolicitudGestionada(element.IdUsr);
        self.CedulaAsesorGestionada(element.CedulaAsesor);
        self.CedulaContratanteGestionada(element.CedulaContratante);
        self.LinkGestionada(element.Link);
        $("#sl-tipoContratoGestion").val(clave);
        self.changeTipoContratoGestionSac();
        self.NumeroContratoGestionada(element.NumeroContrato_Inclusion);
        self.ClaveMedioPagoGestionada(element.ClaveMedioPago);
        self.NumeroAfiliadosGestionada(element.NumeroAfiliados);
        self.TipoArchivoGestionada(element.TipoArchivo);
        self.accionSeleccionada();
        self.changeAccionSac();
        self.observaciones();
        $.each(self.causales(), function (index, value) {
            value.Seleccionado(false);
        });

        self.causalesSeleccionadas([]);

        $("#mdl-gestion-solicitud-sac").modal();
    };
    self.accionSeleccionada.subscribe(function (value) {
        if (value === "rechazar")
            self.causalesHabilitadas(true);
        else
            self.causalesHabilitadas(false);
    });
    self.noExisteContrato = function () {
        self.titleModal("Error");
        self.textModal("El contrato no se encuentra activo o no existe");
        $("#mdl-fnsh-accept").modal("show");
    };
    self.diligenciamientoIncompleto = function () {
        self.titleModal("Error");
        self.textModal("Diligenciamiento incompleto");
        $("#mdl-fnsh-accept").modal("show");
    };

    self.gestionarSolicitudDigitalizacion = function (radicar, rechazar, medioPago, tipoContrato) {
        $.ajax({
            type: 'POST',
            url: '/Digitalizacion/GestionarSolicitudDigitalizacion/',
            data: {
                info: {
                    IdUsuario: self.idUsuarioSolicitudGestionada(),
                    Id: self.idSolicitudGestionada(),
                    NumeroFormulario: self.NumeroFormularioGestionada(),
                    CedulaAsesor: self.CedulaAsesorGestionada(),
                    CedulaContratante: self.CedulaContratanteGestionada(),
                    NumeroAfiliados: self.NumeroAfiliadosGestionada(),
                    Link: self.LinkGestionada(),
                    TipoArchivo: self.TipoArchivoGestionada(),
                    Radicar: radicar,
                    Rechazar: rechazar,
                    Causales: self.causalesSeleccionadas(),
                    Observaciones: self.observaciones(),
                    MedioPago: medioPago,
                    TipoContrato: tipoContrato,
                    NumeroContrato: self.NumeroContratoGestionada()
                }
            },
            dataType: 'json',
            success: function (result, status, xhr) {
                $("#mdl-gestion-solicitud-sac").modal('hide');
                self.titleModal("Operación exitosa");
                self.textModal("La solicitud ha sido " + (radicar ? "radicada" : "rechazada"));
                $("#mdl-fnsh-accept").modal("show");

                self.closeModalGestionSac();
                self.obtenerSolicitudesDigitalizacion();
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error1 ", jqXHR);
            }
        });
    };
    self.guardarGestion = function () {

        debugger;

        var radicar = false;
        var rechazar = false;
        var esInclusion = true;
        self.observacionesRequired("");
        self.motivosRechazoRequired("");
        self.medioPagoRequired("");
        self.numeroContratoRequired("");
        self.accionRequired("");
        var tipoContrato = $("#sl-tipoContratoGestion").val();
        var medioPago = $("#sl-medioPagoGestion").val();
        if (tipoContrato === "fam-nuevo" || tipoContrato === "ap-nuevo" || tipoContrato === "ppe-nuevo") {
            esInclusion = false;
            self.NumeroContratoGestionada('');
        }

        if (self.accionSeleccionada() === "rechazar") {
            self.causalesSeleccionadas([]);
            rechazar = true;
            $.each(self.causales(), function (index, value) {
                if (value.Seleccionado()) {
                    var causalNueva = new Object();
                    causalNueva.CausalId = value.IdCausal;
                    causalNueva.Causal = value.Motivo;
                    self.causalesSeleccionadas().push(causalNueva);
                }
            });
        } else if (self.accionSeleccionada() === "radicar") {
            radicar = true;
            self.causalesSeleccionadas([]);
        }

        if (
            (esInclusion && (!self.NumeroContratoGestionada() || self.NumeroContratoGestionada().length < 4)) ||
            (radicar && medioPago === "") ||
            (
                rechazar &&
                (
                    self.observaciones() === undefined ||
                    self.observaciones() === null ||
                    self.observaciones() === "" ||
                    self.causalesSeleccionadas().length === 0
                )
            ) ||
            (!radicar && !rechazar)
        ) {
            if (esInclusion && (!self.NumeroContratoGestionada() || self.NumeroContratoGestionada().length < 4))
                self.numeroContratoRequired("El número de contrato debe tener mínimo 4 caracteres");
            if (radicar && medioPago === "")
                self.medioPagoRequired("* Este campo es obligatorio");
            if (
                rechazar &&
                (self.observaciones() === undefined ||
                    self.observaciones() === null ||
                    self.observaciones() === "")
            ) {
                self.observacionesRequired("* Este campo es obligatorio");
            }
            if (rechazar && self.causalesSeleccionadas().length === 0) {
                self.motivosRechazoRequired("* Este campo es obligatorio");
            }
            if (!radicar && !rechazar) {
                self.accionRequired("* Seleccione una acción");
            }
            self.diligenciamientoIncompleto();
            return;
        }

        if (esInclusion)
            $.ajax({
                type: 'GET',
                url: '/Digitalizacion/VerificarContrato/',
                data: { numeroContrato: self.NumeroContratoGestionada() },
                dataType: 'json',
                success: function (result, status, xhr) {
                    if (result)
                        self.gestionarSolicitudDigitalizacion(radicar, rechazar, medioPago, tipoContrato);
                    else
                        self.noExisteContrato();
                }
                ,
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("error1 ", jqXHR);
                }
            });
        else
            self.gestionarSolicitudDigitalizacion(radicar, rechazar, medioPago, tipoContrato);

    };

    self.getFiles = function (rutaArchivo, nombreOriginal) {
        window.location.href = '/Digitalizacion/DownloadFiles?rutaArchivo=' + rutaArchivo + '&nombreOriginal=' + nombreOriginal;
    };

    self.drawTable = function () {

        if (isDirector) {
            $('#tb-ctzn').DataTable({
                fixedColumns: true,
                columnDefs: [
                    { width: 200, targets: 12 }
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
        } else if (isSac) {
            $('#tb-ctzn').DataTable({
                fixedColumns: true,
                columnDefs: [
                    { width: 200, targets: 13 }
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
        } else {
            $('#tb-ctzn').DataTable({
                fixedColumns: true,
                columnDefs: [
                    {
                        width: 200
                        , targets: 13
                    }
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
        }

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

        var dtNow = moment().format('YYYY/MM/DD');

        $('#dtIni').val(dtNow);
        $('#dtFin').val(dtNow);

        $('#dtIni,#dtFin').datepicker('update');

        reporteVM.obtenerSolicitudesDigitalizacion();
    };



    self.verCausalesSolicitud = function (element) {
        $.ajax({
            type: 'GET',
            url: '/Digitalizacion/ObtenerCausalesSolicitud/',
            data: {
                idSolicitud: element.Id
            },
            dataType: 'json',
            success: function (result, status, xhr) {
                if (result !== null && result.length) {
                    self.causalesSolicitudSeleccionada(result);
                    var content = "";
                    $.each(result, function (index, value) {
                        content += "<tr><td>" + value.Motivo + "</td></tr>";
                    });
                    self.contentCausales(content);
                } else {
                    self.contentCausales("<tr><td>No hay causales de rechazo</td></tr>");
                }
                $("#mdl-causales-rechazo").modal();
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error1 ", jqXHR);
            }
        });
    };

    self.verHistorialSolicitud = function (element) {
        self.historicosSolicitudSeleccionada([]);
        $.ajax({
            type: 'GET',
            url: '/Digitalizacion/ObtenerHistoricoSolicitud/',
            data: {
                idSolicitud: element.Id
            },
            dataType: 'json',
            success: function (result, status, xhr) {
                if (result !== null) {
                    //var actual = new Object();
                    //actual.Fecha = element.FechaRegistro;
                    //actual.Estado = element.Estado;
                    $.ajax({
                        type: 'GET',
                        url: '/Digitalizacion/ObtenerCausalesSolicitud/',
                        data: {
                            idSolicitud: element.Id
                        },
                        dataType: 'json',
                        success: function (causalesResult, status, xhr) {
                            //actual.Causales = causalesResult;
                            //result.unshift(actual);
                            $.each(result, function (index, value) {
                                value.CausalesStr = "";
                                $.each(value.Causales, function (index, value2) {
                                    value.CausalesStr += "- " + value2.Motivo + "<br/>";
                                });
                                var observaciones = "</br><i>Observaciones:</i> " +
                                    (value.Observaciones === null || value.Observaciones === undefined || value.Observaciones === "" ? '---' : value.Observaciones);
                                value.CausalesStr += observaciones;
                                //if (value.Analista === undefined)
                                //    value.Analista = 'N/A';
                            });
                            self.historicosSolicitudSeleccionada(result);
                            $("#mdl-historial-solicitud").modal();
                        }, error: function (jqXHR, textStatus, errorThrown) {
                            console.log("error1 ", jqXHR);
                        }
                    });
                }
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error1 ", jqXHR);
            }
        });

    };


}

var reporteVM = new IndexViewModel();
ko.applyBindings(reporteVM);

var id = 0;

$(function () {

    var dtNow = moment().format('YYYY/MM/DD');

    $('#dtIni').val(dtNow);
    $('#dtFin').val(dtNow);

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
    reporteVM.obtenerCedulaAsesor();
    reporteVM.obtenerSolicitudesDigitalizacion();
    reporteVM.getCausalesRechazo();
    $("#sl-tipoArchivo").val("");
    $("#sl-tipoContratoEdit").val("");
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

$("#dtIni, #dtFin").on('change', function () {

    if (id === 5)
        id = 0;

    if (id === 0) {

        if (reporteVM.stateClean() === 0) {
            reporteVM.obtenerSolicitudesDigitalizacion();
        }
    }

    id += 1;
});

$(document).ajaxSend(function (event, request, settings) {
    $("#ov-loading").show();
});

$(document).ajaxStop(function () {

    window.setTimeout(function () {
        $("#ov-loading").hide();
    }, 1000);
});

function validateNumber(id, idalert, isquery) {
    var id = parseInt($("#" + id).val());
    if (id == 0) {
        $("#" + idalert).text('El número debe ser diferente de cero');
        $("#" + idalert).addClass("alert-form");
        $("#" + idalert).css('color', 'red');
    } else {
        $("#" + idalert).text('');
        $("#" + idalert).removeClass("alert-form");
    }
    switch (isquery) {
        case 1: GetPaymentByReference(id, idalert); break;
    }
}

function GetPaymentByReference(ref, idalert) {
    $.ajax({
        url: "/Digitalizacion/PostPaymentByReference",
        method: "POST",
        data: {
            refPago: ref
        },
        success: function (data) {
            console.log(data);
            if (!data.Exitoso) {
                $("#" + idalert).text('La referencia de pago no existe');
                $("#" + idalert).addClass("alert-form");
                $("#" + idalert).css('color', 'red');
            }
        }
    });
}