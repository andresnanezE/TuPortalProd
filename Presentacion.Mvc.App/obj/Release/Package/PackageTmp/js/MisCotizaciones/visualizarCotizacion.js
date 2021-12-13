(function () {
    'use strict';

    angular.module('portalComercialApp', ['angular-loading-bar', 'cfp.loadingBar']).controller('visualizarCotizacionController', visualizarCotizacionController);

    function visualizarCotizacionController($scope, $http, $window, cfpLoadingBar) {

        $scope.InfReporteCotizacion = null;
        $scope.InfSedesCotizacion = null;


        $scope.getObtenerInfSedesCotizacion = function () {
            $http({
                url: '../BloqueoNit/ObtenerInfSedesCotizacion/',
                method: "GET"
            }).then(function (res) {

                var dCiudad;
                var dataC = res.data;

                for (var i = 0; i < dataC.length; i++) {

                    dCiudad = dataC[i].NombreCiudad;

                    if (dCiudad === 'BOGOTA'
                        || dCiudad === 'BOGOTA D.C.'
                        || dCiudad === 'CHIA'
                        || dCiudad === 'SOACHA'
                        || dCiudad === 'CAJICA') {
                        $(".hd-a").removeClass("hidden");
                        break;
                    }
                }

                $scope.InfSedesCotizacion = res.data;
            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.getObtenerInfReporteCotizacion = function () {
            $http({
                url: '../BloqueoNit/ObtenerInfReporteCotizacion/',
                method: "GET"
            }).then(function (res) {
                $scope.InfReporteCotizacion = res.data;
                $scope.getObtenerInfSedesCotizacion();
            }).catch(function (res) {
                console.log(res.message);
            });
        };        

        //Calculo Cotización
        $scope.valorPropuestaEconomica = function () {        
            var valorTotal = 0;
            var valorMensual = 0;
            var valorMensualIva = 0;

            if ($scope.InfSedesCotizacion !== null && $scope.InfSedesCotizacion.length > 0) {

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

            if ($scope.InfSedesCotizacion !== null && $scope.InfSedesCotizacion.length > 0) {
                for (var i = 0; i < $scope.InfSedesCotizacion.length; i++) {
                    valorTotal += $scope.InfSedesCotizacion[i].Valor;
                }

                valorMensual = valorTotal / 12;
                valorMensualIva = valorMensual * 5 / 100;
                valorTotal = valorMensual + valorMensualIva;
            }

            return valorTotal;
        };

        /*****LOAD*****/
        $scope.getObtenerInfReporteCotizacion();
    }
})();