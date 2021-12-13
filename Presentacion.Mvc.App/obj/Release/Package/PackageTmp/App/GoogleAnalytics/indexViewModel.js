$(function () {

    //Link Modificar Usuario -- Página Usuario
    $(".modify-user").click(function () {

        console.log("modify");

        var id = $(this).attr('name');

        var dataGA = {
            "Category": "Administración usuarios",
            "Action": "Modificar",
            "Label": "Actualizacion datos de usuario"
        };

        registerAnalytic(dataGA);

        window.setTimeout(function () {
            window.location.href = "/Usuario/Modificar?USUARIOID=" + id;
        }, 1000);
        
    });

    //Link Activar Usuario -- Página Usuario
    $(".active-user").click(function () {

        console.log("modify");

        var id = $(this).attr('name');

        var dataGA = {
            "Category": "Administración usuarios",
            "Action": "Activar",
            "Label": "Habilitar usuario en aplicación"
        };

        registerAnalytic(dataGA);

        window.setTimeout(function () {
            window.location.href = "/Usuario/Modificar?USUARIOID=" + id;
        }, 1000);

    });

    //Link Desactivar Usuario -- Página Usuario
    $(".deactive-user").click(function () {

        console.log("modify");

        var id = $(this).attr('name');

        var dataGA = {
            "Category": "Administración usuarios",
            "Action": "Desactivar",
            "Label": "Deshabilitar usuario en aplicación"
        };

        registerAnalytic(dataGA);

        window.setTimeout(function () {
            window.location.href = "/Usuario/Modificar?USUARIOID=" + id;
        }, 1000);

    });

    //Link Regístrate -- Página Inicial
    $('#registrarteLogin').click(function () {

        var dataGA = {
            "Category": "Inicio Portal",
            "Action": "Redirección",
            "Label": "Redirección formulario para nuevo usuario"
        };

        registerAnalytic(dataGA);
    });

    //Link Olvidaste Contraseña -- Página Inicial
    $('#olvidasteLogin').click(function () {

        var dataGA = {
            "Category": "Inicio Portal",
            "Action": "Restaurar",
            "Label": "Redirección restaurar contraseña"
        };

        registerAnalytic(dataGA);
    });

    //Botón ver todas las noticias
    $('#viewAllNews').click(function () {

        var dataGA = {
            "Category": "Noticias",
            "Action": "Visualizar",
            "Label": "Ver todas las noticias"
        };

        registerAnalytic(dataGA);
    });

    //Botón Consultar NIT -- Página Reservar NIT
    $('#btn-consulta-nit').click(function () {

        var nit = $("#input-nit").val();

        if (nit !== "") {

            var dataGA = {
                "Category": "Consulta NIT",
                "Action": "Búsqueda",
                "Label": "Consulta sobre un NIT para verificar datos"
            };

            registerAnalytic(dataGA);
        }
    });

    //Botón Reservar Nit -- Página Reservar NIT
    $('#btn-reservar-cliente').click(function () {

        var dataGA = {
                "Category": "Consulta NIT",
                "Action": "Reservar",
                "Label": "Reserva del NIT ingresado previo a la consulta"
        };
        registerAnalytic(dataGA);
    });

    //Botón Liberar Reserva de NIT -- Página Gestionar Reserva
    $('#btn-liberar-reserva').click(function () {

        var dataGA = {
            "Category": "Mis Reservas",
            "Action": "Liberar",
            "Label": "Liberación de la reserva"
        };
        registerAnalytic(dataGA);
    });

    //Botón Renovar Reserva de NIT -- Página Gestionar Reserva
    $('#btn-renovar-reserva').click(function () {

        var dataGA = {
            "Category": "Mis Reservas",
            "Action": "Renovar",
            "Label": "Renovación de la reserva"
        };
        registerAnalytic(dataGA);
    });

    //Botón Reconsiderar Cotización -- Página Gestionar Reserva
    $('#btn-reconsiderar').click(function () {

        var dataGA = {
            "Category": "Mis Reservas",
            "Action": "Reconsiderar",
            "Label": "Reconsideración de una reserva"
        };
        registerAnalytic(dataGA);
    });

    //Botón Descargar Informe Consolidado -- Página Reporte Bajas
    $('[name="bjs-informe-consolidado"]').click(function () {

        var dataGA = {
            "Category": "Reporte Bajas",
            "Action": "Descarga",
            "Label": "Descarga informe consolidado"
        };
        registerAnalytic(dataGA);
    });

    //Botón Descargar Informe Detallado -- Página Reporte Bajas
    $('[name="bjs-informe-detallado"]').click(function () {

        var dataGA = {
            "Category": "Reporte Bajas",
            "Action": "Descarga",
            "Label": "Descarga informe detallado"
        };
        registerAnalytic(dataGA);
    });

    //Botón Descargar Informe Consolidado -- Página Detalle de Reactivaciones
    $('[name="rct-informe-consolidado"]').click(function () {

        var dataGA = {
            "Category": "Reporte Reactivaciones",
            "Action": "Descarga",
            "Label": "Descarga informe consolidado"
        };
        registerAnalytic(dataGA);
    });

    //Botón Descargar Informe Detallado -- Página Detalle de Reactivaciones
    $('[name="rct-informe-detallado"]').click(function () {

        var dataGA = {
            "Category": "Reporte Reactivaciones",
            "Action": "Descarga",
            "Label": "Descarga informe detallado"
        };
        registerAnalytic(dataGA);
    });

    //Botón Consultar Detalle Afiliaciones -- Página Detalle de Afiliaciones
    $('#buscar-afiliaciones').click(function () {

        var dataGA = {
            "Category": "Reporte Afiliaciones",
            "Action": "Consulta",
            "Label": "Visualización detalle de afiliaciones"
        };
        registerAnalytic(dataGA);
    });

    //Botón Buscar Contratos -- Página Consulta de Contratos
    $('[name="buscar-contratos"]').click(function () {

        var dataGA = {
            "Category": "Contratos",
            "Action": "Consulta",
            "Label": "Visualización información de contratos"
        };
        registerAnalytic(dataGA);
    });

    //Botón Buscar Inclusiones -- Página Validación de Inclusiones
    $('[name="buscar-inclusiones"]').click(function () {

        var dataGA = {
            "Category": "Validar inclusión",
            "Action": "Consulta",
            "Label": "Visualización información de inclusiones"
        };
        registerAnalytic(dataGA);
    });

    //Botón Generar Reporte Gerente Nacional-- Página Reporte Gerente Nacional
    $('#cns-rpt-gr-nacional').click(function () {

        var dataGA = {
            "Category": "Reporte Cotización",
            "Action": "Consulta",
            "Label": "Visualización reporte GERENTE NACIONAL"
        };
        registerAnalytic(dataGA);
    });

    //Botón Generar Reporte Gerente Regional-- Página Reporte Gerente Regional
    $('#cns-rpt-gr-regional').click(function () {

        var dataGA = {
            "Category": "Reporte Cotización",
            "Action": "Consulta",
            "Label": "Visualización reporte GERENTE REGIONAL"
        };
        registerAnalytic(dataGA);
    });

    //Botón Generar Reporte Pricing-- Página Reporte Pricing
    $('#cns-rpt-pricing').click(function () {

        var dataGA = {
            "Category": "Reporte Cotización",
            "Action": "Consulta",
            "Label": "Visualización reporte PRICING"
        };
        registerAnalytic(dataGA);
    });

    //Botón Generar Reporte Director-- Página Reporte Director
    $('#cns-rpt-director').click(function () {

        var dataGA = {
            "Category": "Reporte Cotización",
            "Action": "Consulta",
            "Label": "Visualización reporte DIRECTOR"
        };
        registerAnalytic(dataGA);
    });

    //Botón Generar Reporte Asesor-- Página Reporte Asesor
    $('#cns-rpt-asesor').click(function () {

        var dataGA = {
            "Category": "Reporte Cotización",
            "Action": "Consulta",
            "Label": "Visualización reporte ASESOR"
        };
        registerAnalytic(dataGA);
    });

});

//Document Ready ADRES -- Página Contratos FOSYGA
function registerAnalyticADRES() {

    var dataGA = {
        "Category": "ADRES",
        "Action": "Consulta",
        "Label": "Visualización información ADRES"
    };
    registerAnalytic(dataGA);

}

//Document Ready Geo-Zonificador -- Página GeoZonificador
function registerAnalyticGeoZonificador() {

    var dataGA = {
        "Category": "Geo-Zonificador",
        "Action": "Consulta",
        "Label": "Visualización información de geo-zonificadores"
    };
    registerAnalytic(dataGA);

}

//Document Ready Manual Usuario -- Página Manual de Usuario
function registerAnalyticUserManual() {

    var dataGA = {
        "Category": "Ayuda Usuario",
        "Action": "Consulta",
        "Label": "Visualización del manual de usuario"
    };
    registerAnalytic(dataGA);

}

//Botón Exportar Reporte Gerente Nacional-- Página Reporte Gerente Nacional
function registerAnalyticExportExcelGerenteNacional() {

    var dataGA = {
        "Category": "Reporte Cotización",
        "Action": "Descarga",
        "Label": "Descargar reporte GERENTE NACIONAL"
    };
    registerAnalytic(dataGA);

}

//Botón Exportar Reporte Gerente Regional-- Página Reporte Gerente Regional
function registerAnalyticExportExcelGerenteRegional() {

    var dataGA = {
        "Category": "Reporte Cotización",
        "Action": "Descarga",
        "Label": "Descargar reporte GERENTE REGIONAL"
    };
    registerAnalytic(dataGA);

}

//Botón Exportar Reporte Pricing-- Página Reporte Pricing
function registerAnalyticExportExcelPricing() {

    var dataGA = {
        "Category": "Reporte Cotización",
        "Action": "Descarga",
        "Label": "Descargar reporte PRICING"
    };
    registerAnalytic(dataGA);

}

//Botón Exportar Reporte Director-- Página Reporte Director
function registerAnalyticExportExcelDirector() {

    var dataGA = {
        "Category": "Reporte Cotización",
        "Action": "Descarga",
        "Label": "Descargar reporte DIRECTOR"
    };
    registerAnalytic(dataGA);

}

//Botón Exportar Reporte Asesor-- Página Reporte Asesor
function registerAnalyticExportExcelAsesor() {

    var dataGA = {
        "Category": "Reporte Cotización",
        "Action": "Descarga",
        "Label": "Descargar reporte ASESOR"
    };
    registerAnalytic(dataGA);

}

function registerAnalytic(dataGA) {

    $.ajax({
        type: 'POST',
        url: '../GoogleAnalytics/RegistrarEventoGoogleAnalytic/',
        data: { data: dataGA },
        dataType: 'json',
        success: function (data) {
        }, error: function (jqXHR, textStatus, errorThrown) {}
    });
}