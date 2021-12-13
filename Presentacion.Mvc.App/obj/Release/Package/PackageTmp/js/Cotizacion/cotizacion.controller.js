(function () {
    'use strict';

    angular.module('portalComercialApp', ['angular-loading-bar', 'cfp.loadingBar']).controller('cotizacionController', cotizacionController);

    function cotizacionController($scope, $http, $window) {

        //******========= Eventos HTTP =========******

        //Http Get
        $scope.getObtenerListados = function (nombreListado) {

            $http({
                url: '../BloqueoNit/ObtenerListados/',
                method: "GET",
                params: { nombreListado: nombreListado }
            }).then(function (res) {
   
                switch (nombreListado) {
                    case "SectorEconomico":
                        $scope.lstSectorEconomico = res.data;
                        break;
                    case "TipoAP":
                        $scope.lstTipoAP = res.data;
                        break;
                    case "CiudadesFactor":
                        $scope.lstCiudadesFactor = res.data;
                        break;
                    case "TipoRiesgo":
                        $scope.lstTipoRiesgo = res.data;
                        break;
                    default:
                }
            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.getObtenerIdCotizacion = function () {
            $http({
                url: '../BloqueoNit/ObtenerIdCotizacion/',
                method: "GET"
            }).then(function (res) {
                $scope.idCotizacion = res.data;
            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.getObtenerInformacionReserva = function () {
            $http({
                url: '../BloqueoNit/ObtenerInformacionReserva/',
                method: "GET"
            }).then(function (res) {
                $scope.lstInformacionReserva = res.data;
            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.getObtenerNombreDirector = function () {
            $http({
                url: '../BloqueoNit/ObtenerNombreDirector/',
                method: "GET"
            }).then(function (res) {
                $scope.nombreDirector = res.data;
            }).catch(function (res) {
                console.log(res.message);
            });
        };

        //Llenar la data de la cotización que se envia por email.
        $scope.getObtenerInfReporteCotizacion = function () {
            $http({
                url: '../BloqueoNit/ObtenerInfReporteCotizacion/',
                method: "GET"
            }).then(function (res) {
                $scope.InfReporteCotizacion = res.data;
                if ($scope.InfReporteCotizacion.length <= 0) {

                    $scope.motrarAlerta(1, "Se guardo correctamente la cotización.");
                    $scope.motrarAlerta(2, "Error al enviar el email, no se genero la plantilla de envio.");
                    $scope.motrarAlerta(2, "El asesor no tiene asignado un director.");

                    setTimeout(function () {
                        $scope.redirectMisReservas();
                    }, 10000);

                } else {
                    $scope.getObtenerInfSedesCotizacion();
                }

            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.getObtenerInfSedesCotizacion = function () {
            $http({
                url: '../BloqueoNit/ObtenerInfSedesCotizacion/',
                method: "GET"
            }).then(function (res) {
                $scope.InfSedesCotizacion = res.data;
                $scope.pdf("#reporteCotizacion");
            }).catch(function (res) {
                console.log(res.message);
            });
        };

        //Http Post
        $scope.guardarCotizacion = function () {
            $scope.objguardarCotizacion.Id_Cotizacion = parseInt($scope.idCotizacion);
            $scope.objguardarCotizacion.Id_Sector = parseInt($scope.cotizacion.idSectorEconomico);
            $scope.objguardarCotizacion.Id_TipoAP = parseInt($scope.cotizacion.idTipoAP);
            $scope.objguardarCotizacion.ValorTarifa = 0;
            $scope.objguardarCotizacion.ListadoSedes = $scope.lstCotizacionSedes;

            var nombreSector = $scope.lstSectorEconomico.filter(function (x) { return x.Id_sector === parseInt($scope.cotizacion.idSectorEconomico); });
            var nombreTipoAP = $scope.lstTipoAP.filter(function (x) { return x.Id_area === parseInt($scope.cotizacion.idTipoAP); });

            $scope.objguardarCotizacion.SectorEconomico = nombreSector[0].Descripcion;
            $scope.objguardarCotizacion.TipoAP = nombreTipoAP[0].Tipo_area;

            $http({
                url: "../BloqueoNit/GuardarCotizacion/"
                , dataType: 'json'
                , method: "POST"
                , data: JSON.stringify($scope.objguardarCotizacion)
                , headers: {
                    "Content-Type": "application/json"
                }
            }).then(successHandler, errorHandler);
            function successHandler(res) {

                if (res.data !== null) {

                    //Respuesta de Actualizar Cotización
                    if (res.data.RespCotizacion === true && res.data.RespCotizacionSedes === true) {
                        //Crear El Reporte de la Cotización
                        $scope.getObtenerInfReporteCotizacion();
                    }
                    else {
                        //Respuesta de Crear Cotización
                        if (res.data.RespCotizacion === true) {
                            $scope.motrarAlerta(1, res.data.MensajeCotizacion);
                        } else {
                            $scope.motrarAlerta(2, res.data.MensajeCotizacion);
                        }
                        //Respuesta de Crear Sedes
                        if (res.data.RespCotizacionSedes === true) {
                            $scope.motrarAlerta(1, res.data.MensajeCotizacionSedes);
                        }
                        else {
                            $scope.motrarAlerta(2, res.data.MensajeCotizacionSedes);
                        }
                    }
                }
            }
            function errorHandler(res) {
                console.log(response);
            }
        };
        $scope.generarPdf = function (inf) {
            kendo.drawing.drawDOM($(inf.sectionExportar), {
                forcePageBreak: inf.forcePageBreak,
                paperSize: inf.formatoPapel,
                multiPage: inf.multipagina,
                margin: { left: "0cm", top: "1cm", right: "0cm", bottom: "0cm" }
            })
                .then(function (group) {
                    return kendo.drawing.exportPDF(group);
                })
                .done(function (data) {
                    $scope.objEnvioEmailCotizacion.PlantillaCotizacion.Base64 = data;
                    $scope.objEnvioEmailCotizacion.PlantillaCotizacion.FileName = inf.fileName;
                    $scope.objEnvioEmailCotizacion.Nit = $scope.InfReporteCotizacion[0].Nit;
                    $scope.objEnvioEmailCotizacion.NombreEmpresa = $scope.InfReporteCotizacion[0].NombreEmpresa;
                    $scope.enviarEmailCotizacion();
                });
        };
        $scope.enviarEmailCotizacion = function () {
            $http({
                url: "../BloqueoNit/EnviarEmailCotizacion/"
                , dataType: 'json'
                , method: "POST"
                , data: JSON.stringify($scope.objEnvioEmailCotizacion)
                , headers: {
                    "Content-Type": "application/json"
                }
            }).then(successHandler, errorHandler);
            function successHandler(res) {
                if (res.data !== null) {

                    console.log(res.data);

                    $scope.motrarAceptarMsgConfirmacion = true;

                    if (res.data.Respuesta === true) {
                        var options = { "backdrop": "static", keyboard: true };
                        $('#modalCreadaCotizacion').modal(options);
                        $('#modalCreadaCotizacion').modal('show');
                    } else {
                        $scope.motrarAceptarMsgConfirmacion = false;
                        $scope.motrarReporte = false;
                        $scope.redirectMisReservas();
                        $scope.motrarAlerta(2, res.data.Mensaje);
                    }
                }
            }
            function errorHandler(res) {
                console.log(response);
            }
        };

        //******========= Funciones =========******
        $scope.declaracionVariables = function () {
            $scope.idCotizacion = null;
            $scope.lstSectorEconomico = null;
            $scope.lstTipoAP = null;
            $scope.lstCiudadesFactor = null;
            $scope.lstTipoRiesgo = null;
            $scope.lstCotizacionSedes = [];
            $scope.lstInformacionReserva = [];
            $scope.InfReporteCotizacion = null;
            $scope.InfSedesCotizacion = [];
            $scope.motrarReporte = false;
            $scope.motrarAceptarMsgConfirmacion = false;
            $scope.cotizacion = null;
            $scope.validacionInfo = "Certifico que la información proporcionada es veraz y exacta.";
            $scope.objCotizacionSedes = {
                Id: null,
                Id_Cotizacion: null,
                Id_Ciudad: null,
                Id_TipoRiesgo: null,
                NombreSede: null,
                NoPersonalPermanente: null,
                NoPersonalVisitantes: null,
                FechaCreacion: null,
                FechaModificacion: null,
                Valor: null,
                NombreCiudad: null,
                NombreTipoRiesgo: null
            };
            $scope.objguardarCotizacion = {
                Id_Cotizacion: null,
                Id_Sector: null,
                Id_TipoAP: null,
                SectorEconomico: null,
                TipoAP: null,
                ValorTarifa: null,
                ListadoSedes: []
            };
            $scope.objEnvioEmailCotizacion = {
                PlantillaCotizacion: {
                    ContentType: null,
                    Base64: null,
                    FileName: null
                },
                Nit: null,
                NombreEmpresa: null
            };
        };

        //Sección Sedes
        $scope.agregarSede = function () {
            $scope.objCotizacionSedes.Id = $scope.lstCotizacionSedes.length + 1;
            $scope.objCotizacionSedes.NombreSede = "Sede " + ($scope.lstCotizacionSedes.length + 1).toString();
            $scope.objCotizacionSedes.Id_Cotizacion = parseInt($scope.idCotizacion);

            $scope.lstCotizacionSedes.push($scope.objCotizacionSedes);
            $scope.objCotizacionSedes = {
                Id: null,
                Id_Cotizacion: null,
                Id_Ciudad: null,
                Id_TipoRiesgo: null,
                NombreSede: null,
                NoPersonalPermanente: null,
                NoPersonalVisitantes: null
            };
        };
        $scope.eliminarSede = function (id) {
            $scope.lstCotizacionSedes.splice(id, 1);
        };

        $scope.valideKey = function (evt) {
            var code = evt.which ? evt.which : evt.keyCode;
            if (code >= 48 && code <= 57) {
                return true;
            } else {
                event.preventDefault();
            }
        };
        $scope.motrarAlerta = function (tipoMsg, Msg) {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": true,
                "progressBar": true,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "10000",
                "hideDuration": "10000",
                "timeOut": "10000",
                "extendedTimeOut": "10000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };

            if (tipoMsg === 1) {
                toastr.success(Msg);
            } else if (tipoMsg === 2) {
                toastr.error(Msg);
            }
        };
        $scope.redirectMisReservas = function () {
            $scope.motrarReporte = false;
            $window.location.href = '/BloqueoNit/MisCotizaciones';
        };

        //Calculo Cotización
        $scope.valorPropuestaEconomica = function () {
            var valorTotal = 0;
            var valorMensual = 0;
            var valorMensualIva = 0;

            if ($scope.InfSedesCotizacion.length > 0) {
                for (var i = 0; i < $scope.InfSedesCotizacion.length; i++) {
                    valorTotal += $scope.InfSedesCotizacion[i].Valor;
                }

                valorMensual = valorTotal / 12;
            }

            return valorMensual;
        };
        $scope.valorPropuestaEconomicaIva = function () {
            var valorTotal = 0;
            var valorMensual = 0;
            var valorMensualIva = 0;

            if ($scope.InfSedesCotizacion.length > 0) {
                for (var i = 0; i < $scope.InfSedesCotizacion.length; i++) {
                    valorTotal += $scope.InfSedesCotizacion[i].Valor;
                }

                valorMensual = valorTotal / 12;
                valorMensualIva = valorMensual * 5 / 100;
                valorTotal = valorMensual + valorMensualIva;
            }

            return valorTotal;
        };

        $scope.pdf = function (sectionExportar) {
            //Propiedades para la exportación del PDF.
            var objPropiedadesPDF = {
                sectionExportar: sectionExportar,
                forcePageBreak: ".page-break",
                formatoPapel: "Letter",
                multipagina: true,
                margin: {
                    left: "0",
                    top: "2cm",
                    right: "0",
                    bottom: "0"
                },
                fileName: "Cotizacion_" + $scope.idCotizacion.toString() + ".pdf"
                //urlPDF: "../BloqueoNit/GenerarPDF/"
            };

            //Subir archivo PDF al servidor, se almacena en el proyecto API en la carpeta Upload.
            $scope.generarPdf(objPropiedadesPDF);
        };

        //Abrir Modal Resumen Cotización
        $scope.abrirModalResumen = function () {
            $scope.motrarReporte = true;

            //Obtener Nombres de Sector Económico / Tipo AP
            $scope.nombreSector = $scope.lstSectorEconomico.filter(function (x) { return x.Id_sector === parseInt($scope.cotizacion.idSectorEconomico); });
            $scope.nombreTipoAP = $scope.lstTipoAP.filter(function (x) { return x.Id_area === parseInt($scope.cotizacion.idTipoAP); });

            var options = { "backdrop": "static", keyboard: true };
            $('#modalResumenCotizacion').modal(options);
            $('#modalResumenCotizacion').modal('show');
        };

        //******========= Load =========******
        $scope.declaracionVariables();

        //Información seleccionada en mis reservas
        $scope.getObtenerIdCotizacion();
        $scope.getObtenerNombreDirector();
        $scope.getObtenerInformacionReserva();

        //Obtener Listados
        $scope.getObtenerListados("SectorEconomico");
        $scope.getObtenerListados("TipoAP");
        $scope.getObtenerListados("CiudadesFactor");
        $scope.getObtenerListados("TipoRiesgo");
    }
})();