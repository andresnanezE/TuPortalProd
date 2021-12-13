(function () {
    'use strict';

    angular.module('portalComercialApp')
        .factory('cotizadorTarifasService', CotizadorTarifasFactory);

    CotizadorTarifasFactory.$inject = ['$http'];

    function CotizadorTarifasFactory($http) {
        debugger;
        return {
            API: '../CotizadorTarifas/Json',
            getTarifas: getTarifas
        };

        function getTarifas(oTarifaCampana) {
            debugger;
            return $http.get(this.API + '?' + serializeObject(oTarifaCampana));
        }

        function serializeObject(obj) {
            var str = "";
            for (var key in obj) {
                if (str != "") {
                    str += "&";
                }
                str += key + "=" + encodeURIComponent(obj[key]);
            }
            return str;
        }
    }
})();