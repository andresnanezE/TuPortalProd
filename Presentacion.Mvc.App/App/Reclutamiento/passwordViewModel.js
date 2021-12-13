function passwordViewModel() {
    var self = this;
    self.nombres = ko.observable();
    self.apellidos = ko.observable();
    self.contraseniaI = ko.observable("");
    self.contraseniaII = ko.observable("");
    self.titleModal = ko.observable();
    self.textModal = ko.observable();

    self.changePassword = function () {

        var nDoc = self.getUrlParameter('nDoc');
        var rcvr = self.getUrlParameter('rcvr');

        if (self.validatePassword() === false)
            return;

        if (self.contraseniaI() !== self.contraseniaII()) {

            $("#password,#passwordII").addClass("error-vl");

            self.titleModal("Cambio de contraseña");
            self.textModal("La nueva contraseña y la contraseña de confirmación no coinciden. Por favor vuelva a digitar las misma contraseña en ambos cuadros.");
            $("#mdl-error").modal("show");

            return;
        }

        var str = self.contraseniaII();
        if (str.match(/[a-z]/g) && str.match(/[A-Z]/g) && str.match(/[0-9]/g) && str.length >= 8) {

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
                url: "/Reclutamiento/ActualizarContrasenia",
                type: 'POST',
                dataType: 'json',
                contentType: "application/json",
                data: JSON.stringify({ numeroDocumento: nDoc, password: self.contraseniaI(), rcvr: rcvr }),
                success: function (result) {

                    if (result === false || result === "Error") {
                        self.titleModal("Cambio de contraseña");
                        self.textModal("El cambio de contraseña no se pudo realizar de forma exitosa, por favor intente mas tarde.");
                        $("#mdl-error").modal("show");
                        return;
                    }

                    if (result.length >= 1) {
                        self.titleModal("Cambio de contraseña");
                        self.nombres(result[0].Nombres);
                        self.apellidos(result[0].Apellidos);
                        $("#mdl-recovery").modal("show");
                    }

                    $("#cnt-form").css("pointer-events", "");
                },
                error: function () {
                    self.titleModal("Cambio de contraseña");
                    self.textModal("El cambio de contraseña no se pudo realizar de forma exitosa, por favor intente mas tarde.");
                    $("#mdl-error").modal("show");
                }
            });

        }
        else {
            self.titleModal("Cambio de contraseña");
            self.textModal("Por favor tenga en cuenta las políticas de contraseña segura para poder seguir con el proceso de cambio de contraseña.");
            $("#mdl-error").modal("show");
        }
    };

    self.validatePassword = function () {

        if (self.contraseniaI() === "" || self.contraseniaI() === undefined) {
            $("#password").addClass("error-vl");
        }

        if (self.contraseniaII() === "" || self.contraseniaII() === undefined) {
            $("#passwordII").addClass("error-vl");
        }

        if (self.contraseniaI() && self.contraseniaII())
            return true;

        return false;
    };

    self.indexRedirect = function () {

        window.location.href = "/Reclutamiento/Index";
    };

    self.getUrlParameter = function(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
    };
}

var passwordVM = new passwordViewModel();
ko.applyBindings(passwordVM);

$(document).ready(function () {

    window.history.pushState(null, "", window.location.href);
    window.onpopstate = function () {
        window.history.pushState(null, "", window.location.href);
    };

    var hForm = $("#dv-img").height() + 65;
    $("#dv-form").css("height", hForm);

    window.setTimeout(function () {
        $("#password").val("");
        passwordVM.contraseniaI("");
    }, 100);
});

$("#password, #passwordII").keyup(function (event) {
    var val = $(this).val();

    if (val !== "") {
        $(this).removeClass("error-vl");
    }

}).keydown(function (event) {
    if (event.which == 13) {
        event.preventDefault();
    }
});