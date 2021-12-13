(function () {
    'use strict';

    angular.module('portalComercialApp', []).controller('GestionarReservaController', GestionarReservaController);

    function GestionarReservaController($scope, $http, $window) {
        $scope.lstInformacionReserva = [];
        $scope.lstSedesCotizacion = [];
        $scope.lstCiudadesFactor = [];
        $scope.idCotizacion = null;
        $scope.objLiberarRenovar = {};
        $scope.notaLiberarRenovar = null;
        $scope.validarLiberarReservar = 0;
        $scope.rolUsuario = rolPrincipal;
        $scope.lstNotas = [];
        $scope.valoresReconsideracionOriginales = [];

        //******========= Eventos HTTP =========******
        $scope.getObtenerIdCotizacion = function () {
            $http({
                url: '../GestionarReserva/ObtenerIdCotizacion/',
                method: "GET"
            }).then(function (res) {
                $scope.idCotizacion = res.data;
                console.log("a", $scope.idCotizacion);
            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.getObtenerInformacionReserva = function () {
            $http({
                url: '../GestionarReserva/ObtenerInformacionReserva/',
                method: "GET"
            }).then(function (res) {
                $scope.lstInformacionReserva = res.data;
            }).catch(function (res) {
                console.log(res.message);
            });
        };

        $scope.getObtenerCiudadesFactor = function () {
            $http({
                url: '../BloqueoNit/ObtenerCiudadesFactor/',
                method: "GET"
            }).then(function (res) {
                debugger;
                $scope.lstCiudadesFactor = res.data;
            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.getObtenerTipoRiesgo = function () {
            $http({
                url: '../BloqueoNit/ObtenerTipoRiesgo/',
                method: "GET"
            }).then(function (res) {
                $scope.lstTipoRiesgo = res.data;
            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.getObtenerInfSedes = function () {
            $http({
                url: '../GestionarReserva/ObtenerInfSedesCotizacion/',
                method: "GET"
            }).then(function (res) {
                $scope.lstSedesCotizacion = res.data;
                $scope.valoresReconsideracionOriginales = $scope.lstSedesCotizacion.map(function (sede) {
                    return {
                        sedeId: sede.Id,
                        valor: sede.ValorReconsideracion,
                        valorReconsideracion: sede.ValorReconsideracion
                    };
                });
            }).catch(function (res) {
                console.log(res.message);
            });
        };

        $scope.getObtenerNotas = function () {
            $http({
                url: '../GestionarReserva/ObtenerNotasCotizacion',
                method: "GET"
            }).then(function (res) {
                $scope.lstNotas = res.data;
            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.guardarData = function () {
            var urlServicio = "";

            $("#btn-ag-rn").attr("disabled", true);

            //1 = Liberar Reserva, 2 = Renovar Reserva
            if ($scope.validarLiberarReservar === 1) {
                urlServicio = "../GestionarReserva/LiberarReserva/";
            } else if ($scope.validarLiberarReservar === 2) {
                urlServicio = "../GestionarReserva/RenovarReservas/";
            }

            $http({
                url: urlServicio
                , dataType: 'json'
                , method: "POST"
                , data: JSON.stringify($scope.objLiberarRenovar)
                , headers: {
                    "Content-Type": "application/json"
                }
            }).then(successHandler, errorHandler);
            function successHandler(res) {
                if (res.data !== null) {
                    if (res.data.Respuesta === true) {
                        $scope.motrarAlerta(1, res.data.Mensaje);
                        setTimeout(function () {
                            $scope.redirectMisReservas();
                            $("#btn-ag-rn").removeAttr("disabled");
                        }, 2000);
                    } else {
                        $scope.motrarAlerta(2, res.data.Mensaje);
                    }
                }
            }
            function errorHandler(res) {
                console.log(response);
            }
        };
        $scope.guardarReconsideracion = function () {

            if ($scope.lstInformacionReserva[0].NumeroReconsideraciones === maxReconsideraciones) {
                $('#m-reconsideracion').modal('hide');
                return $scope.motrarAlerta(2, "La cotización ya ha sido reconsiderada " + maxReconsideraciones+" veces.");
            }

            var reconsideracion = {
                sedes: $scope.lstSedesCotizacion,
                notaLiberarRenovar: $scope.objLiberarRenovar.notaLiberarRenovar
            };

            $http({
                url: "../GestionarReserva/Index"
                , dataType: 'json'
                , method: "POST"
                , data: JSON.stringify(reconsideracion)
                , headers: {
                    "Content-Type": "application/json"
                }
            }).then(successHandler, errorHandler);
            function successHandler(res) {
                if (res.data.Respuesta === true) {
                    $scope.motrarAlerta(1, "Reserva Enviada a reconsideración");
                    setTimeout(function () {
                        $scope.redirectMisReservas();
                    }, 2000);
                }
                else {
                    $scope.motrarAlerta(2, res.data.Mensaje);
                }
                $('#m-reconsideracion').modal('hide');
            }
            function errorHandler(res) {
                console.log(response);
            }
        };
        $scope.aprobarRechazarReconsideracion = function (tipo) {

            if ($scope.lstInformacionReserva[0].EstadoCotizacion === EstadosEnum.RECONSIDERADO
                || $scope.lstInformacionReserva[0].EstadoCotizacion === EstadosEnum.NORECONSIDERADO) {
                $('#m-reconsideracion').modal('hide');
                return $scope.motrarAlerta(2, "La cotización ya ha sido procesada.");
            }

            var aprobacion = {
                aprobar: tipo === 1 ? true : false,
                sedes: $scope.lstSedesCotizacion,
                notaLiberarRenovar: $scope.objLiberarRenovar.notaLiberarRenovar
            };

            $http({
                url: "../GestionarReserva/AprobarRechazarReconsideracion"
                , dataType: 'json'
                , method: "POST"
                , data: JSON.stringify(aprobacion)
                , headers: {
                    "Content-Type": "application/json"
                }
            }).then(successHandler, errorHandler);
            function successHandler(res) {
                if (res.data.Respuesta === true) {
                    $scope.motrarAlerta(1, tipo === 1 ? "Cotización Aprobada" : "Cotización Rechazada");
                    setTimeout(function () {
                        $scope.redirectMisReservas();
                    }, 2000);
                }
                else {
                    $scope.motrarAlerta(2, res.data.Mensaje);
                }
                $('#m-reconsideracion').modal('hide');
            }
            function errorHandler(res) {
                console.log(res);
            }
        };

        //******========= Funciones =========******
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
                "showDuration": "300",
                "hideDuration": "3000",
                "timeOut": "7000",
                "extendedTimeOut": "1000",
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
            $window.location.href = '/MisReservas/';
        };
        $scope.validarReconsideracion = function () {
            for (var i = 0; i < $scope.lstSedesCotizacion.length; i++) {
                if ($scope.lstSedesCotizacion[i].ValorReconsideracion !== $scope.lstSedesCotizacion[i].Valor)
                    return true;
            }
            return false;
        };
        $scope.getTotal = function () {
            var total = 0;
            for (var i = 0; i < $scope.lstSedesCotizacion.length; i++) {
                var sede = $scope.lstSedesCotizacion[i];
                total += sede.ValorReconsideracion;
            }
            return total;
        };

        $scope.permiteReconsiderar = function () {
            if ($scope.lstInformacionReserva.length > 0) {
                var info = $scope.lstInformacionReserva[0];
                if (![EstadosEnum.RECONSIDERADO, EstadosEnum.NOCOTIZADO].includes(info.EstadoCotizacion)) {//Cumple con estado
                    if (![RolEnum.ASESOR, RolEnum.PRICING].includes(rolPrincipal) && EstadosEnum.PENDIENTERECONSIDERAR != info.EstadoCotizacion) { // Es Director/Gerente
                        return info.NumeroReconsideraciones < maxReconsideraciones;
                    }
                    if (RolEnum.PRICING === rolPrincipal) {// Es Pricing
                        return info.NumeroReconsideraciones < maxReconsideraciones && info.EstadoCotizacion === EstadosEnum.PENDIENTERECONSIDERAR;
                    }
                }
            }
            return false;
        }
        $scope.permiteAprobar = function () {
            if ($scope.lstInformacionReserva.length > 0)
                return rolPrincipal === RolEnum.PRICING && $scope.lstInformacionReserva[0].EstadoCotizacion === EstadosEnum.PENDIENTERECONSIDERAR
            else
                return false;
        }
        $scope.permiteVerReconsideracion = function () {
            if ($scope.lstInformacionReserva.length > 0) {
                var info = $scope.lstInformacionReserva[0];
                return rolPrincipal !== RolEnum.ASESOR &&
                    info.EstadoCotizacion !== EstadosEnum.RECONSIDERADO &&
                    info.NumeroReconsideraciones === maxReconsideraciones;
            }
            else
                return false;
        }

        $scope.permiteRenovar = function () {
            if ($scope.lstInformacionReserva.length > 0) {
                console.log($scope.lstInformacionReserva[0].numeroRenovaciones);
                return $scope.lstInformacionReserva[0].numeroRenovaciones < maxRenovaciones;
            }
            else
                return false;
        };
        $scope.abrirModalLiberar = function () {
            var options = { "backdrop": "static", keyboard: true };

            //1 = Liberar Reserva, 2 = Renovar Reserva, 3 = Reconsiderar, 4/5 = aprobar/rechazar
            switch ($scope.validarLiberarReservar) {
                case 1:
                    $('#m-liberacion').modal(options);
                    $('#m-liberacion').modal('show');
                    break;
                case 2:
                    $('#m-renovar').modal(options);
                    $('#m-renovar').modal('show');
                    break;
                case 3:
                    if ($scope.validarReconsideracion()) {
                        $('#m-reconsiderar-message').text("¿Estás seguro de realizar esta acción?");
                        $('#m-reconsideracion').modal(options);
                        $('#m-reconsideracion').modal('show');
                    }
                    else {
                        $scope.motrarAlerta(2, "Debes modificar por lo menos un valor");
                    }
                    break;
                case 4:
                    $('#m-reconsiderar-message').html("Al aprobar se tomarán los valores de la última reconsideración<br/> ¿Estás seguro de aprobar esta cotización?");
                    $('#m-reconsideracion').modal(options);
                    $('#m-reconsideracion').modal('show');
                    break;
                case 5:
                    $('#m-reconsiderar-message').html(" ¿Estás seguro de rechazar la reconsideración?");
                    $('#m-reconsideracion').modal(options);
                    $('#m-reconsideracion').modal('show');
                    break;
            }
        };

        $scope.confirmarModal = function () {
            switch ($scope.validarLiberarReservar) {
                case 3:
                    $scope.guardarReconsideracion();
                    break;
                case 4:
                    $scope.aprobarRechazarReconsideracion(1);
                    break;
                case 5:
                    $scope.aprobarRechazarReconsideracion(2);
                    break;
            }
        };

        $scope.getObtenerIdCotizacion();
        $scope.getObtenerInformacionReserva();
        $scope.getObtenerInfSedes();
        $scope.getObtenerNotas();
    }
})();