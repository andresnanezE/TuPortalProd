(function () {
    'use strict';

    angular.module('portalComercialApp', ['ui.grid', 'ui.grid.pagination', 'angular-loading-bar', 'cfp.loadingBar']).controller('misCotizacionesController', misCotizacionesController);

    function misCotizacionesController($scope, $http, $window, cfpLoadingBar, i18nService, $location) {

        //Cambiar Idioma del Grid a Español.
        i18nService.setCurrentLang('es');

        $scope.mostrarGrid = false;
        $scope.scroll = {
            top: 0,
            left: 0
        };

        $scope.pricyng = null;
        $scope.director = null;
        $scope.asesor = null;
        $scope.lstCotizacionesRol = [];
        $scope.lstCotizaciones = [];

        //******========= Eventos HTTP =========******
        $scope.getObtenerListados = function (nombreListado) {
            $http({
                url: '../BloqueoNit/ObtenerListadosMisCotizaciones/',
                method: "GET",
                params: { nombreListado: nombreListado }
            }).then(function (res) {

                console.log(res);

                switch (nombreListado) {
                    case "EstadosReserva":
                        $scope.lstEstadosReserva = res.data;
                        break;
                    case "EstadosCotizacion":
                        $scope.lstEstadosCotizacion = res.data;
                        break;
                    case "Productos":
                        $scope.lstProductos = res.data;
                        break;
                    default:
                }
            }).catch(function (res) {
                console.log(res.message);
            });
        };
        $scope.getObtenerInfMisCotizaciones = function (estadoReserva) {
            $http({
                url: '../BloqueoNit/ObtenerInfMisCotizaciones/',
                method: "GET",
                params: { estadoReserva: estadoReserva }
            }).then(function (res) {
                if (res.data.Respuesta === true) {

                    $scope.abrirModalProximasVencer();
                    $scope.infMisCotizaciones = res.data;
                    console.log($scope.infMisCotizaciones);

                    $scope.construirGrid();

                    //Es Director / Prycing
                    if ($scope.infMisCotizaciones.LstMisCotizacionesRol !== null) {

                        for (var i = 0; i < $scope.infMisCotizaciones.LstMisCotizacionesRol.length; i++) {
                            $scope.infMisCotizaciones.LstMisCotizacionesRol[i].FechaVencimiento = new Date($scope.infMisCotizaciones.LstMisCotizacionesRol[i].FechaVencimiento).toLocaleDateString();
                        }

                        $scope.gridInfMisCotizacionesRol.data = $scope.infMisCotizaciones.LstMisCotizacionesRol;
                        $scope.lstCotizacionesRol = $scope.infMisCotizaciones.LstMisCotizacionesRol;                
                    } else {

                        for (var x = 0; x < $scope.infMisCotizaciones.LstMisCotizaciones.length; x++) {
                            $scope.infMisCotizaciones.LstMisCotizaciones[x].fechaVencimiento = new Date($scope.infMisCotizaciones.LstMisCotizaciones[x].fechaVencimiento).toLocaleDateString();
                        }

                        $scope.gridInfMisCotizaciones.data = $scope.infMisCotizaciones.LstMisCotizaciones;
                        $scope.lstCotizaciones = $scope.infMisCotizaciones.LstMisCotizaciones;
                    }
                }

            }).catch(function (res) {
                console.log(res.message);
            });
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
                "hideDuration": "1000",
                "timeOut": "5000",
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
        $scope.construirGrid = function () {

            //Estructura de Grid Asesor
            $scope.gridInfMisCotizaciones = {

                customScroller: function myScrolling(uiGridViewport, scrollHandler) {
                    uiGridViewport.on('scroll', function myScrollingOverride(event) {
                        $scope.scroll.top = uiGridViewport[0].scrollTop;
                        $scope.scroll.left = uiGridViewport[0].scrollLeft;
                        scrollHandler(event);
                    });
                },

                paginationPageSizes: [50, 100, 150, 200],
                paginationPageSize: 20,
                columnDefs: [
                    {
                        name: 'N° Reserva',
                        field: 'Id_Cotizacion',
                        width: 110,
                        cellTemplate:
                            '<div>{{ row.entity.id_Cotizacion }}</div>' +
                            '<img ' +
                            'ng-show="row.entity.validarVencida === true" ' +
                            'src="../../Image/load.gif" style="height: 25px;margin-top: -31px; margin-left: 57%" />'
                    },
                    { name: 'NIT', field: 'NIT', width: 150 },
                    { name: 'Razón Social', field: 'NombreEmpresa', width: 250 },
                    { name: 'Producto', field: 'Producto', width: 150 },
                    {
                        name: 'Fecha Expiración',
                        field: 'FechaVencimiento',
                        width: 150,
                        cellTemplate: '<div>{{ row.entity.fechaVencimiento | date }}</div>'
                    },
                    { name: 'Estado Reserva', field: 'Estado', width: 150 },
                    {
                        name: 'Renovar',
                        field: '',
                        enableSorting: false,
                        width: 100,
                        cellTemplate:
                            '<button' +
                            ' ng-click="grid.appScope.redirectRenovarReserva(row.entity.id_cotizacion)"' +
                            ' ng-disabled="row.entity.estado !== \'Vigente\'"' +
                            ' type="button" class="btn btn-view" style="left: 27px; border-top-width: 2px; top: 2px; padding-top: 0px; padding-bottom: 0px;"><span class="glyphicon glyphicon-chevron-right"></span></button>'

                    },                    
                    {
                        name: 'Cotizar/Consultar',
                        field: '',
                        enableSorting: false,
                        width: 200,
                        cellTemplate:
                            '<button ' +
                            'ng-show="row.entity.estadoCotizacion === \'Sin Cotizar\'" ' +
                            'ng-click="grid.appScope.redirectCotizacion(row.entity.id_cotizacion)" ' +
                            'type="button" class="btn btn-cotizar" style="top: 0px;">Cotizar</button>' +

                            '<label ' +
                            'ng-show="row.entity.estadoCotizacion === \'Cotizado\' && grid.appScope.asesor !== null" ' +
                            'ng-click="grid.appScope.visualizarPDF(row.entity.id_cotizacion)" ' +
                            'style="top: 0px; cursor: pointer;" > <img src="../../Image/pdf-file-format-symbol.svg" /></label>' +

                            '<button ' +
                            'ng-show="row.entity.estadoCotizacion === \'Pendiente Reconsideración\'" ' +
                            'type="button" class="btn btn-primary" style="top: 0px;">ReconsiderarReserva</button>'
                    },
                    { name: 'Estado Reserva', field: 'estado', width: 150 },
                    { name: 'Estado Cotización', field: 'estadoCotizacion', width: 150 },
                    {
                        name: 'N° Reserva',
                        field: 'id_cotizacion',
                        width: 110,
                        cellTemplate:
                            '<div class="ui-grid-cell-contents ng-binding ng-scope" style="font-weight: 900;">{{ row.entity.id_cotizacion }}</div>' +
                            '<img ' +
                            'ng-show="row.entity.validarVencida === true" ' +
                            'src="../../Image/load.gif" style="height: 25px;margin-top: -31px; margin-left: 57%" />'
                    },
                    {
                        name: 'NIT',
                        field: 'NIT',
                        width: 150,
                        cellTemplate:
                            '<div class="ui-grid-cell-contents ng-binding ng-scope" style="font-weight: 900;">{{ row.entity.NIT }}</div>'
                    },
                    { name: 'Razón Social', field: 'nombreEmpresa', width: 250 },
                    { name: 'Producto', field: 'productos', width: 150 },
                    {
                        name: 'Fecha Expiración',
                        field: 'fechaVencimiento',
                        width: 150,
                        cellTemplate:
                            '<div class="ui-grid-cell-contents ng-binding ng-scope">{{ row.entity.fechaVencimiento | date }}</div>'
                    }
                ]
            };

            //Estructura de Grid Rol Director, Pricyng
            $scope.gridInfMisCotizacionesRol = {
                customScroller: function myScrolling(uiGridViewport, scrollHandler) {
                    uiGridViewport.on('scroll', function myScrollingOverride(event) {
                        $scope.scroll.top = uiGridViewport[0].scrollTop;
                        $scope.scroll.left = uiGridViewport[0].scrollLeft;
                        scrollHandler(event);
                    });
                },

                paginationPageSizes: [100, 150],
                columnDefs: [
                    {
                        name: 'Renovar',
                        field: '',
                        headerCellClass: 'text-center',
                        enableSorting: false,
                        width: 100,
                        cellTemplate:
                            '<button ' +
                            'ng-click="grid.appScope.redirectRenovarReserva(row.entity.IdCotizacion)" ' +
                            'ng-disabled="row.entity.Estado !== \'Vigente\'" ' +
                            'type="button" class= "btn btn-view" style="left: 27px;border-top-width: 2px;top: 2px;padding-top: 0px;padding-bottom: 0px;"><span class="glyphicon glyphicon-chevron-right" ></span></button>'

                    },
                    {
                        name: 'Cotizar/Consultar',
                        field: '',
                        enableSorting: false,
                        headerCellClass: 'text-center',
                        width: 200,
                        cellTemplate:
                            '<button ' +
                            'ng-show="row.entity.EstadoCotizacion === \'Sin Cotizar\'" ' +
                            'ng-click="grid.appScope.redirectCotizacion(row.entity.IdCotizacion)" ' +
                            'type="button" class="btn btn-cotizar" style="left: 50px;top: 4px;bottom: 0px;padding-top: 0px;padding-bottom: 0px;">Cotizar</button>' +

                            '<label ' +
                            'ng-show="row.entity.EstadoCotizacion === \'Cotizado\'" ' +
                            'ng-click="grid.appScope.visualizarPDF(row.entity.IdCotizacion)" ' +
                            'style="margin-left: 88px;top: 1px;padding-top: 1px;padding-bottom: 1px;"> <img src="../../Image/pdf-file-format-symbol.svg"/></label>' +

                            '<button ' +
                            'ng-show="row.entity.EstadoCotizacion === \'Pendiente Reconsideración\'" ' +
                            'type="button" class="btn btn-primary" style="left: 50px;">ReconsiderarReserva</button>'
                    },
                    { name: 'Estado Reserva', field: 'Estado', headerCellClass: 'text-center', width: 150 },
                    { name: 'Estado Cotización', field: 'EstadoCotizacion', headerCellClass: 'text-center', width: 150 },
                    {
                        name: 'N° Reserva',
                        headerCellClass: 'text-center',
                        field: 'IdCotizacion',
                        width: 110,
                        cellTemplate:
                            '<div class="ui-grid-cell-contents ng-binding ng-scope" style="font-weight: 900;">{{ row.entity.IdCotizacion }}</div>' +
                            '<img ' +
                            'ng-show="row.entity.ValidarVencida === true" ' +
                            'src="../../Image/load.gif" style="height: 25px;margin-top: -31px; margin-left: 57%" />'
                    },
                    {
                        name: 'NIT',
                        field: 'Nit',
                        width: 150,
                        cellTemplate:
                            '<div class="ui-grid-cell-contents ng-binding ng-scope" style="font-weight: 900;">{{ row.entity.Nit }}</div>'
                    },
                    { name: 'Razón Social', field: 'NombreEmpresa', headerCellClass: 'text-center', width: 250 },
                    { name: 'Director', field: 'NombreDirector', headerCellClass: 'text-center', width: 300 },
                    { name: 'Asesor', field: 'NombreAsesor', headerCellClass: 'text-center', width: 300 },
                    { name: 'Producto', field: 'Producto', headerCellClass: 'text-center', width: 150 },
                    {
                        name: 'Fecha Expiración',
                        field: 'FechaVencimiento',
                        headerCellClass: 'text-center',
                        width: 150,
                        cellTemplate: '<div class="ui-grid-cell-contents ng-binding ng-scope">{{ row.entity.FechaVencimiento | date }}</div>'
                    }
                ]
            };

            //IdRol  7 = Director, 28 = Pricing, 8 = Asesor
            if ($scope.infMisCotizaciones.LstRoles !== null) {

                if ($scope.infMisCotizaciones.LstRoles.length > 0) {
                    for (var i = 0; i < $scope.infMisCotizaciones.LstRoles.length; i++) {
                        if ($scope.infMisCotizaciones.LstRoles[i] === "7") {
                            $scope.director = $scope.infMisCotizaciones.LstRoles[i];
                        } else if ($scope.infMisCotizaciones.LstRoles[i] === "28") {
                            $scope.pricyng = $scope.infMisCotizaciones.LstRoles[i];
                        } else if ($scope.infMisCotizaciones.LstRoles[i] === "8") {
                            $scope.asesor = $scope.infMisCotizaciones.LstRoles[i];
                        }
                    }
                }

                //Ocultar Columna Director
                if ($scope.director !== null) {
                    $scope.gridInfMisCotizacionesRol.columnDefs[3].visible = false;
                }
            }

        };
        $scope.abrirModalProximasVencer = function () {

            var options = { "backdrop": "static", keyboard: true };
            $('#myModalProximasVencer').modal(options);
            $('#myModalProximasVencer').modal('show');
        };
        $scope.visualizarPDF = function (value) {
            var strWindowFeatures = "location=yes,height=320,width=620,scrollbars=yes,status=yes";
            var url = '/BloqueoNit/VisualizarCotizacion?cotID=' + value;
            var win = $window.open(url, "_blank", strWindowFeatures);
        };
        $scope.redirectRenovarReserva = function (value) {
            $window.location.href = '/BloqueoNit/ReconsiderarReserva?cotID=' + value;
        };
        $scope.redirectCotizacion = function (value) {
            $window.location.href = '/BloqueoNit/CotizarReserva?cotID=' + value;
        };
        $scope.filtrarGrid = function (tipo, valor, valorFechaFinal) {
            var fechaInicial = null;
            var fechaFinal = null;

            //Tipo 1 = No es Rango de fecha, Tipo 2 = Rango Fecha
            if (tipo === 2) {
                fechaInicial = new Date(valor).toLocaleDateString();
                fechaFinal = new Date(valorFechaFinal).toLocaleDateString();
            }

            var dataMisCotizacionesRol = $scope.lstCotizacionesRol;
            var dataMisCotizaciones = $scope.lstCotizaciones;

            if (dataMisCotizacionesRol !== null) {
                $scope.gridInfMisCotizacionesRol.data = dataMisCotizacionesRol;

            } else if (dataMisCotizaciones !== null) {
                $scope.gridInfMisCotizaciones.data = dataMisCotizaciones;
            }

            //Rol Asesor
            if ($scope.gridInfMisCotizaciones.data !== undefined) {
                var dataCoincididaAsesor = $scope.gridInfMisCotizaciones.data.filter(
                    function (x) {
                        return x.NIT.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.nombreEmpresa.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.estado.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.estadoCotizacion.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.productos.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.fechaVencimiento >= fechaInicial && x.fechaVencimiento <= fechaFinal;
                    });

                if (dataCoincididaAsesor.length > 0) {
                    $scope.gridInfMisCotizaciones.data = dataCoincididaAsesor;
                }
            }

            //Rol Diferente a asesor
            if ($scope.gridInfMisCotizacionesRol.data !== undefined) {
                var dataCoincidida = $scope.gridInfMisCotizacionesRol.data.filter(
                    function (x) {
                        return x.NIT.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.NombreEmpresa.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.Estado.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.EstadoCotizacion.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.Producto.substr(0, valor.length).toUpperCase() === valor.toUpperCase() ||
                            x.FechaVencimiento >= fechaInicial && x.FechaVencimiento <= fechaFinal;
                    });

                if (dataCoincidida.length > 0) {
                    $scope.gridInfMisCotizacionesRol.data = dataCoincidida;
                }
            }
        };
        $scope.limpiarFiltros = function () {

            //Limpiar controles
            $scope.objDataFiltro.NitNombre = null;
            $scope.objDataFiltro.estadoCotizacion = null;
            $scope.objDataFiltro.producto = null;
            $scope.objDataFiltro.estadoReserva = null;
            $scope.objDataFiltro.fechaInicio = null;
            $scope.objDataFiltro.fechaFin = null;

            //Asignar Al grid la data antes de filtrarla
            var dataMisCotizacionesRol = $scope.lstCotizacionesRol;
            var dataMisCotizaciones = $scope.lstCotizaciones;

            if (dataMisCotizacionesRol !== null) {
                $scope.gridInfMisCotizacionesRol.data = dataMisCotizacionesRol;

            } else if (dataMisCotizaciones !== null) {
                $scope.gridInfMisCotizaciones.data = dataMisCotizaciones;
            }
        };

        //Obtener Listados
        $scope.getObtenerListados("EstadosReserva");
        $scope.getObtenerListados("EstadosCotizacion");
        $scope.getObtenerListados("Productos");
        $scope.getObtenerInfMisCotizaciones("Vigente");

    }

})();