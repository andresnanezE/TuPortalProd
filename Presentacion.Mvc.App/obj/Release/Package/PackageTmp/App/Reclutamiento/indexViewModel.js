function indexViewModel() {
    var self = this;

    self.ip = ko.observable();
    self.nombres = ko.observable();
    self.apellidos = ko.observable();
    self.noCedula = ko.observable();
    self.correo = ko.observable();
    self.telefono = ko.observable();
    self.contrasenia = ko.observable();
    self.textModal = ko.observable();
    self.textModalEmail = ko.observable();
    self.titleModal = ko.observable();
    self.ciudadId = ko.observable();
    self.tipoIdentificacionId = ko.observable();
    self.referidoId = ko.observable();
    self.directorId = ko.observable();
    self.referidoSelected = ko.observable();
    self.directorSelected = ko.observable();
    self.tipoIdentificacionSelected = ko.observable();

    self.referidos = ko.observableArray();
    self.directores = ko.observableArray();
    self.ciudades = ko.observableArray();
    self.tipoIdentificacion = ko.observableArray();
    self.terminosCondiciones = ko.observable(false);

    self.state = ko.observable(0);
    self.stateButton = ko.observable(0);

    self.obtenerCiudades = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerCiudadesReclutamiento/',
            dataType: 'json',
            success: function (result) {

                self.ciudades(result);

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerTipoIdentificacion = function () {

        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerTipoIdentificacion/',
            dataType: 'json',
            success: function (result) {

                self.tipoIdentificacion(result);

            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerReferidos = function (ciudadId) {
        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerReferidosPorCiudad/',
            data: { ciudadId: ciudadId },
            dataType: 'json',
            success: function (result) {
                self.referidos(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.obtenerDirectores = function (referidoId) {
        $.ajax({
            type: 'GET',
            url: '/Reclutamiento/ObtenerDirectoresPorReferido/',
            data: { referidoId: referidoId },
            dataType: 'json',
            success: function (result) {
                self.directores(result);
            }, error: function (jqXHR, textStatus, errorThrown) {
                console.log("error");
            }
        });
    };

    self.ciudadesChanged = function () {

        if (self.ciudadId() !== undefined) {
            var ciudadId = self.ciudadId().Id;
            self.obtenerReferidos(ciudadId);

            $('#ciudades').removeClass('error-vl');
            $("#referidos").removeAttr("disabled");
        }
        else {
            $('#referidos')[0].selectedIndex = 0;
            $("#referidos").attr("disabled", "true");
        }
    };

    self.referidosChanged = function () {

        if (self.referidoSelected() !== undefined) {
            self.referidoId(self.referidoSelected().Id);
            self.obtenerDirectores(self.referidoId());

            $('#referidos').removeClass('error-vl');
            $("#directores").removeAttr("disabled");
        }
        else {
            $('#directores')[0].selectedIndex = 0;
            $("#directores").attr("disabled", "true");
        }
    };

    self.directorChanged = function () {

        if (self.directorSelected() !== undefined) {
            self.directorId(self.directorSelected().id_usr);
            $('#directores').removeClass('error-vl');
        }
        else {
            self.directorId("");
        }
    };

    self.tipoIdentificacionChanged = function () {

        if (self.tipoIdentificacionSelected() !== undefined) {
            self.tipoIdentificacionId(self.tipoIdentificacionSelected().Id);
            $('#tipoIdentificacion').removeClass('error-vl');
        }
        else
            $('#tipoIdentificacion').addClass('error-vl');
    };

    self.ingresoSolicitud = function (state) {
        
        if(state !== null)
            self.state(state);

        if (self.state() === 0) {

            $("#t-name").hide();
            $("#t-pass").show();
            $("#dv-name").hide();
            $("#dv-pass").show();

            $("#cnt-form").css("pointer-events", "none");

            $("#i-arrow")
                .removeClass("f-right-an")
                .addClass("f-left-an")
                .animate({ right: '85%' }, 800);

            $('#btn-registrar').hide();
            $('#dv-recovery').show();
            $('#btn-ingresar').show();

            $('#sp-verify').fadeOut(500, function () {
                $(this).text('Ir al registro').fadeIn(500);
            });

            $(".fr-hidden").hide(500, function () {
                $("#cnt-form").addClass("col-sm-4").removeClass("col-sm-5");
            });

            $('#tt-form').fadeOut(500, function () {
                $(this).text('Verificar mi solicitud').fadeIn(500);
                $("#cnt-form").css("pointer-events", "");
            });

            self.state(1);
        }
        else {
            $("#cnt-form").addClass("col-sm-5").removeClass("col-sm-4");

            $("#cnt-form").css("pointer-events", "none");

            $("#i-arrow")
                .removeClass("f-left-an")
                .addClass("f-right-an")
                .animate({ right: '10%' }, 1200);

            $('#sp-verify').fadeOut(500, function () {
                $(this).text('Verificar mi solicitud').fadeIn(500);
            });
            
            $('#btn-ingresar').hide();
            $('#btn-registrar').show();
            $('#dv-recovery').hide();
            $('#btn-enviar').hide();
            $('#dv-back').hide();
            $('#password').show();            

            $('#tt-form').fadeOut(500, function () {

                $(".fr-hidden").show(500);

                $("#t-name").show();
                $("#t-pass").hide();
                $("#dv-name").show();
                $("#dv-pass").hide();

                $(this).text('Registro solicitud nuevo Asesor').fadeIn(500);
                $("#cnt-form").css("pointer-events", "");
            });

            self.state(0);
        }
    };

    self.clearData = function (tipo) {

        self.nombres("");
        self.apellidos("");
        self.noCedula("");
        self.correo("");
        self.stateButton(0);
        self.telefono("");
        self.ciudadId("");
        self.referidoId("");
        self.terminosCondiciones(false);
        $('#referidos')[0].selectedIndex = 0;
        $('#ciudades')[0].selectedIndex = 0;
        $('#tipoIdentificacion')[0].selectedIndex = 0;

        if (tipo === 0) {

            self.textModal("");
            self.titleModal("");

            $("#mdl-fnsh").modal("hide");
            $("#msg-error").removeClass("hidden");
            $("#msg-success").removeClass("hidden");

            self.ingresoSolicitud(0);
        }
    };

    self.getIp = function () {
        $.getJSON("https://api.ipify.org/?format=json", function (e) {
            self.ip(e.ip);
        });
    };

    self.registrarUsuario = function () {

        $("#emailMdl").addClass("hidden");

        self.stateButton(1);

        $("#cnt-form").css("pointer-events", "none");

        if (self.validacionInputs()) {

            var testEmail = /^[A-Z0-9._%+-]+@([A-Z0-9-]+\.)+[A-Z]{2,4}$/i;
            if (!testEmail.test($("#email").val())) {
                self.titleModal("Advertencia");
                self.textModal("Por favor ingrese un correo electrónico valido.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                $("#cnt-form").css("pointer-events", "");
                return;
            }

            var response = grecaptcha.getResponse();
            if (response.length === 0) {
                self.titleModal("Advertencia");
                self.textModal("Para continuar con el registro por favor valide el captcha del formulario.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                $("#cnt-form").css("pointer-events", "");
                return;
            }

            if (!self.terminosCondiciones()) {
                self.titleModal("Advertencia");
                self.textModal("Para continuar con el registro por favor acepte la política de protección de datos personales.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                $("#cnt-form").css("pointer-events", "");
                return;
            }

            if (self.stateButton() === 1) {

                var nTipoIdentificacion = $("#tipoIdentificacion option:selected").text();
                var nCiudadVinculacion = $("#ciudades option:selected").text();
                var nGestionado = $("#referidos option:selected").text();

                var model = {
                    IdTipoIdentificacion: self.tipoIdentificacionId()
                    , Nombres: self.nombres()
                    , Apellidos: self.apellidos()
                    , NumeroCedula: self.noCedula()
                    , Correo: self.correo()
                    , Telefono: self.telefono()
                    , IdReclutador: self.referidoId()
                    , IdDirector: self.directorId()
                    , TerminosCondiciones: self.terminosCondiciones()
                    , Ip: self.ip()
                    , TipoIdentificacion: nTipoIdentificacion
                    , CiudadVinculacion: nCiudadVinculacion
                    , Gestionado: nGestionado
                };

                $.ajax({
                    url: "/Reclutamiento/RegistrarUsuario",
                    type: 'POST',
                    dataType: 'json',
                    contentType: "application/json",
                    data: JSON.stringify(model),
                    success: function (result) {

                        var prLg = result.split('$$')[1];
                        result = result.split('$$')[0];

                        if (result === "Success") {

                            self.titleModal("Registro solicitud nuevo corredor comercial");
                            $("#mdl-fnsh").modal("show");
                            $("#msg-success").removeClass("hidden");
                        }

                        if (result === "General") {
                            self.titleModal("Registro no exitoso");
                            self.textModal("El documento de identidad y el correo electrónico ya se encuentran inscritos, lo invitamos a hacer el proceso de recuperación de contraseña en nuestro portal Ser Asesor.");
                            $("#mdl-error").modal("show");
                            $("#msg-error").removeClass("hidden");
                        }

                        if (result === "Restrictivo") {

                            $("#emailMdl").removeClass("hidden");

                            self.titleModal("Información");
                            self.textModal("Un colaborador de Emermedica le contactará para continuar su proceso. En caso de requerir soporte adicional envia un correo a");
                            self.textModalEmail(prLg);
                            $("#mdl-error").modal("show");
                            $("#msg-error").removeClass("hidden");

                            self.clearData(1);
                        }

                        $("#cnt-form").css("pointer-events", "");
                    },
                    error: function () {
                        self.stateButton(0);
                        self.titleModal("Registro no exitoso");
                        self.textModal("El registro no se pudo realizar de forma exitosa, por favor intente realizar el registro mas tarde.");
                        $("#mdl-error").modal("show");
                        $("#msg-error").removeClass("hidden");
                    }
                });
            }
        }
        else {
            self.stateButton(0);
            $("#cnt-form").css("pointer-events", "");
        }
    };

    self.validacionInputs = function () {

        $("input").removeClass("error-vl");

        if (self.nombres() === "" || self.nombres() === undefined) {
            $('#nombres').addClass('error-vl');
        }

        if (self.apellidos() === "" || self.apellidos() === undefined) {
            $('#apellidos').addClass('error-vl');
        }

        if (self.noCedula() === "" || self.noCedula() === undefined) {
            $('#cedula').addClass('error-vl');
        }

        if (self.correo() === "" || self.correo() === undefined) {
            $('#email').addClass('error-vl');
        }

        if (self.correo() === "" || self.correo() === undefined) {
            $('#email').addClass('error-vl');
        }

        if (self.telefono() === "" || self.telefono() === undefined) {
            $('#telefono').addClass('error-vl');
        }

        if (self.ciudadId() === "" || self.ciudadId() === undefined) {
            $('#ciudades').addClass('error-vl');
        }

        if (self.tipoIdentificacionId() === "" || self.tipoIdentificacionId() === undefined) {
            $('#tipoIdentificacion').addClass('error-vl');
        }

        if (self.referidoId() === "" || self.referidoId() === undefined) {
            $('#referidos').addClass('error-vl');
        }

        if (self.directorId() === "" || self.directorId() === undefined) {
            $('#directores').addClass('error-vl');
        }

        if (self.nombres() && self.apellidos() && self.noCedula() && self.correo() && self.telefono() && self.ciudadId() && self.referidoId() && self.directorId())
            return true;

        return false;

    };

    self.validacionLogin = function () {

        if (self.noCedula() === "" || self.noCedula() === undefined) {
            $('#cedula').addClass('error-vl');
        }
        if (self.contrasenia() === "" || self.contrasenia() === undefined) {
            $('#password').addClass('error-vl');
        }

        if (self.noCedula() && self.contrasenia())
            return true;

        return false;

    };

    self.ingresoProspecto = function () {

        if (self.validacionLogin()) {

            var response = grecaptcha.getResponse();
            if (response.length === 0) {
                self.titleModal("Advertencia");
                self.textModal("Para continuar con el inicio de sesión por favor valide el captcha del formulario.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                $("#cnt-form").css("pointer-events", "");
                return;
            }

            $.ajax({
                type: 'GET',
                url: '/Reclutamiento/IngresoProspecto/',
                dataType: 'json',
                data: { numeroDocumento: self.noCedula(), pass: self.contrasenia() },
                success: function (result) {

                    console.log("login", result);

                    if (result) {

                        localStorage.setItem("prLg", true);
                        localStorage.setItem("nDoc", self.noCedula());

                        window.location.href = "/Reclutamiento/ProcesoRegistro?nDoc=" + self.noCedula();
                    }
                    else {

                        self.noCedula("");
                        self.contrasenia("");

                        self.titleModal("Advertencia");
                        self.textModal("Los datos de ingreso son incorrectos, por favor revise las credenciales enviadas al correo registrado.");
                        $("#msg-error").removeClass("hidden");
                        $("#mdl-error").modal("show");
                    }
                }, error: function (jqXHR, textStatus, errorThrown) {
                    console.log("error");
                }
            });
        }
    };

    self.recoveryPassword = function () {

        $("#t-pass").fadeOut();
        $("#password").fadeOut();


        $("#btn-ingresar").hide();
        $("#btn-enviar").show();

        $("#dv-recovery").hide();
        $("#dv-back").show();

        $('#tt-form').fadeOut(500, function () {
            $(this).text('Recuperar o cambiar contraseña').fadeIn(500);
            $("#cnt-form").css("pointer-events", "");
        });

    };

    self.enviar = function () {

        if (self.noCedula() === "" || self.noCedula() === undefined) {
            $("#cedula").addClass("error-vl");
        }
        else {

            var response = grecaptcha.getResponse();
            if (response.length === 0) {
                self.titleModal("Advertencia");
                self.textModal("Para continuar con la recuperación o cambio de la contraseña por favor valide el captcha del formulario.");
                $("#mdl-error").modal("show");
                $("#msg-error").removeClass("hidden");
                $("#cnt-form").css("pointer-events", "");
                return;
            }

            $.ajax({
                url: "/Reclutamiento/RecuperarContrasenia",
                type: 'GET',
                dataType: 'json',
                contentType: "application/json",
                data: { numeroDocumento: self.noCedula() },
                success: function (result) {

                    if (result.length === 0) {
                        self.stateButton(0);
                        self.titleModal("Cambio de contraseña");
                        self.textModal("El usuario con el número de cédula " + self.noCedula() + " no se encuentra registrado, por favor verifique la cédula ingresada nuevamente.");
                        $("#mdl-error").modal("show");
                        $("#msg-error").removeClass("hidden");
                        return;
                    }

                    if (result[0].UrlRecovery !== "Process") {

                        self.stateButton(0);
                        self.titleModal("¡Se envió tu solicitud!");
                        self.nombres(result[0].Nombres);
                        self.apellidos(result[0].Apellidos);
                        self.correo(result[0].CorreoElectronico);
                        $("#mdl-recovery").modal("show");
                    } else {
                        self.stateButton(0);
                        self.titleModal("Cambio de contraseña");
                        self.textModal("Ya existe una solicitud de cambio de contraseña, por favor revise el correo electrónico asociado al número de documento " + self.noCedula() + " o ingrese un nuevo número de documento.");
                        $("#mdl-error").modal("show");
                        $("#msg-error").removeClass("hidden");
                    }

                },
                error: function () {
                    self.stateButton(0);
                    self.titleModal("Cambio de contraseña");
                    self.textModal("El cambio de contraseña no se pudo realizar de forma exitosa, por favor intente realizar la operación mas tarde.");
                    $("#mdl-error").modal("show");
                    $("#msg-error").removeClass("hidden");
                }
            });

        }

    };

    self.back = function () {

        $("#btn-ingresar").show();
        $("#btn-enviar").hide();

        $("#t-pass").show();
        $("#password").show();
        $("#dv-recovery").show();
        $("#dv-back").hide();

        $('#tt-form').fadeOut(500, function () {
            $(this).text('Verificar mi solicitud').fadeIn(500);
            $("#cnt-form").css("pointer-events", "");
        });
    };

    self.recoveryModal = function () {

        window.location.href = "/Reclutamiento/Index";
    };

}

var reporteVM = new indexViewModel();
ko.applyBindings(reporteVM);

$(document).ready(function () {

    window.history.pushState(null, "", window.location.href);
    window.onpopstate = function () {
        window.history.pushState(null, "", window.location.href);
    };

    localStorage.removeItem("prLg");
    localStorage.removeItem("nDoc");

    //var hForm = $("#dv-img").outerHeight();
    $("#dv-form").css("height", "600");

    window.setTimeout(function () {

        var state = getUrlParameter('state');
        if (state === "0") {
            reporteVM.clearData(0);
        }

        reporteVM.getIp();
        reporteVM.obtenerCiudades();
        reporteVM.obtenerTipoIdentificacion();

        $("#dv-loading").fadeOut(function () {
            $(this).remove();
        });
    }, 2500);

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
setInputFilter(document.getElementById("cedula"), function (value) {
    return /^-?\d*$/.test(value);
});

$("#nombres").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#apellidos").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#cedula").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#email").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#telefono").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});

$("#password").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});