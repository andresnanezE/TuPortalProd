var URI = 'http://localhost:15580/';
//var URI = 'http://192.168.92.15:8110/';

$(document).ajaxSend(function (event, request, settings) {

    var excepcionesUrl = settings.url;

    if (excepcionesUrl.includes("ObtenerProfesionesReclutamiento"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ActualizarRegistro"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ObtenerNotasReclutamiento"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("RegistrarUsuario"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("UploadFiles"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("CompletarRegistro"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ActualizarSolicitud"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ActualizarCapacitacion"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ActualizarContratacion"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("RecuperarContrasenia"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ActualizarContrasenia"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ObtenerTipoIdentificacion"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ExportarContrato"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("DatosSolicitud"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ObtenerInfoCompletaReclutamiento"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ObtenerDirectoresPorReferido"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ObtenerSolicitudes"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ObtenerContrato"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ObtenerDirectores"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ObtenerSucursales"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ObtenerReclutadorPorId"))
        $("#ov-loading").show();

    if (excepcionesUrl.includes("ExportarInformeGestion"))
        $("#ov-loading").show();
});

$(document).ajaxStop(function () {

    window.setTimeout(function () {
        $("#ov-loading").hide();
    }, 1000);
});

var getUrlParameter = function getUrlParameter(sParam) {
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