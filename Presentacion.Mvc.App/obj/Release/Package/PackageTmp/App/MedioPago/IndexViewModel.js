function IndexViewModel() {

    var self = this;

    self.filtro = ko.observable();
    self.identificationType = ko.observable();
    self.identifiquer = ko.observable();
    self.direccion = ko.observable();
    self.email = ko.observable();
    self.numeroBanco = ko.observable();
    self.planesVigentes = ko.observableArray([]);
    self.telefono = ko.observable();
    self.telefonoContratante = ko.observable();
    self.tipoBancoId = ko.observable();
    self.tipoBancoId = ko.observable();
    self.tipoBancos = ko.observableArray([]);
    self.tipoTarjeta = ko.observable("C");
    self.medioPagoActual = ko.observable();
    self.datos = ko.observableArray([]);
    self.emailTP = ko.observable();
    self.telefonoTP = ko.observable();
    self.nombre = ko.observable();
    self.apellido = ko.observable();
    self.busquedaPorTexto = ko.observable(false);
    self.mostrarIvr = ko.observable(false);

    self.titulo = ko.observable();
    self.numCont = ko.observable();
    self.rmtCont = ko.observable();
    self.identificacion = ko.observable();
    self.contratante = ko.observable();
    self.telefono = ko.observable();
    self.estado = ko.observable();
    self.estadoIvr = ko.observable();
    self.numeroSesion = ko.observable();
    self.asesor = ko.observable();
    self.estadoAsesor = ko.observable();
    self.estadoAse = ko.observable();
    self.telefonoAse = ko.observable();
    self.modoPago = ko.observable();
    self.formaPago = ko.observable();
    self.fechaInicio = ko.observable();
    self.fechaFin = ko.observable();
    self.viewBene = ko.observable(false);
    self.numIdenBen = ko.observable();
    self.nomBene = ko.observable();
    self.fecNaciBen = ko.observable();
    self.telBene = ko.observable();
    self.viewPMP = ko.observable(false);
    self.cantidad = ko.observable();
    self.cuotaMensual = ko.observable();
    self.valorContrato = ko.observable();
    self.valorCartera = ko.observable();
    self.viewAP = ko.observable(false);    
    self.verData = ko.observable(false);    


    self.getBancos = function () {

        fetch('../MedioPago/GetBancos', {
            method: 'GET'
        })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                self.tipoBancos(data.res);
            })
            .catch(function (err) {
                console.error(err);
            });
    };

    self.validFiltro = ko.validatedObservable([
        self.filtro = ko.observable().extend({
            required: {
                required: true,
                message: "Campo requerido"
            }
        })
    ]);

    self.validSesionIvr = ko.validatedObservable([
        self.numeroSesion = ko.observable().extend({
            required: {
                required: true,
                message: "Campo requerido"
            }
        })
    ]);

    self.guardarEstadoIvr = function (item) {        

        $("#modalProcesando").modal('show');

        fetch('../MedioPago/UpdateMedioPagoIvr', {
            method: 'post',
            headers: { "content-type": "application/json" },
            body: JSON.stringify({
                Id_CC_ACTFP: self.identifiquer(),
                EST_PASARELA: self.estadoIvr(),
                SESION_IVR: self.numeroSesion()
            })
        })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {
                $("#modalProcesando").modal('hide');
                window.location.href = "../MedioPago/Index";                
            })
            .catch(function (err) {
                console.log("Error:", err);
                $("#modalProcesando").modal('hide');
            });
    };

    self.direccionarPorNombre = function (item) {
        $("#modalProcesando").modal('show');
        var a = new PNotify({
            title: 'Contratos',
            text: 'Abriendo Contrato...',
            type: 'success',
            icon: false
        });

        fetch("../MedioPago/DetalleContrato?prefijo=" + item.PrefijoCont + "&busqueda=" + self.filtro() + "&rmt=" + item.RmtCont + "&contratante=" + item.Identificacion + "&tipoContrato=" + item.TipoContrato + "&tipoBusqueda=" + item.Tipo + "&nombreContratante=" + item.Nombre + "&numCont=" + item.Num_cont, {
            method: 'GET'
        })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {

                self.titulo(data.Titulo);
                self.numCont(data.NumCont);
                self.rmtCont(data.Rmt_Cont);
                self.identificacion(data.Identificacion);
                self.contratante(data.Contratante);
                self.telefonoContratante(data.Telefono);
                self.estado(data.Estado);
                self.asesor(data.Asesor);
                self.estadoAse(data.EstadoAse);
                self.telefonoAse(data.TelefonoAse);
                self.formaPago(data.FormaPago);
                self.modoPago(data.ModoPago);
                self.fechaInicio(data.FechaInicio);
                self.fechaFin(data.FechaFin);
                self.numIdenBen(data.Num_idenBen);
                self.nomBene(data.Nom_bene);
                self.fecNaciBen(data.Fec_naciBen);
                self.telBene(data.Tel_bene);
                self.cantidad(data.Cantidad);
                self.cuotaMensual(data.CuotaMensual);
                self.valorContrato(data.ValorContrato);
                self.valorCartera(data.ValorCartera);
                self.viewAP(data.viewAP);
                self.viewPMP(data.viewPMP);
                self.viewBene(data.viewBene);
                $("#modalProcesando").modal('hide');

                if ($('#detalle-medio-pago') !== 'undefined') {
                    $('#detalle-medio-pago').modal('toggle');
                    return;
                }
            
            })
            .catch(function (err) {
                console.error(err);
            });

        //window.location.href = "../MedioPago/Detalle?prefijo=" + item.PrefijoCont + "&busqueda=" + self.filtro() + "&rmt=" + item.RmtCont + "&contratante=" + item.Identificacion + "&tipoContrato=" + item.TipoContrato + "&tipoBusqueda=&nombreContratante=" + item.Nombre + "&numCont=" + item.Num_cont;

    };

    self.mostarModalAceptacionMedioPago = function () {
        if ($('#confirmar-medio-pago') !== 'undefined') {            
            $('#confirmar-medio-pago').modal('toggle');
            return;
        }
    };

    self.mostarModalIvrRechazado = function () {
        if ($('#confirmar-estado-ivr') !== 'undefined') {
            if (!self.validSesionIvr.isValid())
                return self.validSesionIvr.errors.showAllMessages();

            self.estadoIvr("RECHAZADA");
            $('#confirmar-estado-ivr').modal('toggle');
            return;
        }
    };

    self.mostarModalIvrAprobado = function () {
        if ($('#confirmar-estado-ivr') !== 'undefined') {
            if (!self.validSesionIvr.isValid())
                return self.validSesionIvr.errors.showAllMessages();

            self.estadoIvr("APROBADA");
            $('#confirmar-estado-ivr').modal('toggle');
            return;
        }
    };

    self.direccionarPorContrato = function (item) {
        $("#modalProcesando").modal('show');
        var a = new PNotify({
            title: 'Contratos',
            text: 'Abriendo Contrato...',
            type: 'success',
            icon: false
        });

        fetch("../MedioPago/DetalleContrato?prefijo=" + item.PrefijoCont + "&busqueda=" + item.RmtCont + "&rmt=" + item.RmtCont + "&contratante=" + item.Identificacion + "&tipoContrato=" + item.TipoContrato + "&tipoBusqueda=" + item.Tipo + "&nombreContratante=" + item.Nombre + "&numCont=" + item.Num_cont, {
            method: 'GET'
        })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {

                self.titulo(data.Titulo);
                self.numCont(data.NumCont);
                self.rmtCont(data.Rmt_Cont);
                self.identificacion(data.Identificacion);
                self.contratante(data.Contratante);
                self.telefonoContratante(data.Telefono);
                self.estado(data.Estado);
                self.asesor(data.Asesor);
                self.estadoAse(data.EstadoAse);
                self.telefonoAse(data.TelefonoAse);
                self.formaPago(data.FormaPago);
                self.modoPago(data.ModoPago);
                self.fechaInicio(data.FechaInicio);
                self.fechaFin(data.FechaFin);
                self.numIdenBen(data.Num_idenBen);
                self.nomBene(data.Nom_bene);
                self.fecNaciBen(data.Fec_naciBen);
                self.telBene(data.Tel_bene);
                self.cantidad(data.Cantidad);
                self.cuotaMensual(data.CuotaMensual);
                self.valorContrato(data.ValorContrato);
                self.valorCartera(data.ValorCartera);
                self.viewAP(data.viewAP);
                self.viewPMP(data.viewPMP);
                self.viewBene(data.viewBene);
                $("#modalProcesando").modal('hide');

                if ($('#detalle-medio-pago') !== 'undefined') {
                    $('#detalle-medio-pago').modal('toggle');
                    return;
                }
            
            })
            .catch(function (err) {
                console.error(err);
                $("#modalProcesando").modal('hide');
            });

        //window.location.href = "../MedioPago/Detalle?prefijo=" + item.PrefijoCont + "&busqueda=" + item.RmtCont + "&rmt=" + item.RmtCont + "&contratante=" + item.Identificacion + "&tipoContrato=" + item.TipoContrato + "&tipoBusqueda=&nombreContratante=" + item.Nombre + "&numCont=" + item.Num_cont;
    };

    self.getInfoMedioPago = function () {
                
        if (!self.validFiltro.isValid())
            return self.validFiltro.errors.showAllMessages();

        $("#modalProcesando").modal('show');
        self.verData(true);
        self.clearDataTable();
        self.busquedaPorTexto(isNaN(parseFloat(self.filtro())));

        fetch('../MedioPago/ObtenerPlanesVigentes?filtro=' + self.filtro(), {
            method: 'GET'
        })
        .then(function (response) {
            return response.json();
        })
        .then(function (data) {
            self.planesVigentes(data);
            self.drawTable();
            $("#modalProcesando").modal('hide');

        })
        .catch(function (err) {
            console.error(err);
            $("#modalProcesando").modal('hide');
        });
    };

    self.actualizarInformacion = function (param) {        

        if (!self.wizard1.isValid())
            return self.wizard1.errors.showAllMessages();

        $("#modalProcesando").modal('show');

        var radioValue = $("input[name='optradio']:checked").val();

        if (param === "d") {

            if (!self.wizard2.isValid())
                return self.wizard2.errors.showAllMessages();

            self.update(param);

        } else {
            switch (radioValue) {
                case "tc":
                    self.tipoTarjeta('C');
                    self.update();
                    break;
                case "ta":

                    self.tipoTarjeta($("input[name='opTjRadio']:checked").val());
                    $('#smartwizard').smartWizard("next");
                    $("#modalProcesando").modal('hide');
                    break;
                default:
            }
        }
    };

    self.update = function (param) {

        fetch('../MedioPago/UpdateMedioPago', {
            method: 'post',
            headers: { "content-type": "application/json" },
            body: JSON.stringify({
                identificationNumber: self.identificacion(),
                email: self.email(),
                telefono: self.telefono(),
                direccion: self.direccion(),
                rmtCont: self.rmtCont(),
                tipDeau: self.tipoTarjeta(),
                codBanc: self.tipoBancoId(),
                cueDeau: self.numeroBanco()
            })
        })
            .then(function (response) {
                return response.json();
            })
            .then(function (data) {

                if (data !== undefined) {
                    self.identifiquer(data.Identifiquer);
                }  

                if (param === undefined) {

                    $("#modalProcesando").modal('hide');
                    //alert("Solicitud creada correctamente:  REDIRECCIÓN IVR......");
                    self.mostrarIvr(true);
                    return;
                }                              

                if (param === "d" && data !== undefined) {
                    $("#modalProcesando").modal('hide');
                    window.location.href = "../MedioPago/Response";
                }
            })
            .catch(function (err) {
                console.log("Error:", err);
                $("#modalProcesando").modal('hide');
            });
    };

    self.wizard1 = ko.validatedObservable([
        //self.direccion = ko.observable().extend({
        //    required: {
        //        required: true,
        //        message: "Campo requerido"
        //    }
        //}),
        self.email = ko.observable().extend({
            required: {
                required: true,
                message: "Campo requerido"
            },
            pattern: {
                params: /^([\d\w-\.]+@([\d\w-]+\.)+[\w]{2,4})?$/,
                message: "Correo no valido"
            }
        }),
        self.telefono = ko.observable().extend({
            required: {
                required: true,
                message: "Campo requerido"
            },
            pattern: {
                params: "^[3][0-9]{9}$",
                message: "Número de celular no valido"
            }
        })
    ]);

    self.wizard2 = ko.validatedObservable([
        self.tipoBancoId = ko.observable().extend({
            required: {
                required: true,
                message: "Campo requerido"
            }
        }),
        self.numeroBanco = ko.observable().extend({
            required: {
                required: true,
                message: "Campo requerido"
            },
            validation: [{
                validator: function () {

                    var param = Enumerable.From(self.tipoBancos())
                        .Where(function (b) { return b.CodigoBanco === self.tipoBancoId(); })
                        .Select(function (b) { return b.DigitosAutorizados; })
                        .FirstOrDefault();

                    var regex = new RegExp("^[0-9]{" + param + "}$");

                    var n = 0;

                    if (regex.test(self.numeroBanco())) {
                        n = -1;
                    }

                    return n < 0;
                },
                message: "Tarjeta no valida"
            }]
        })
    ]);

    self.clearDataTable = function () {
        if ($.fn.DataTable.isDataTable('#tb-info-mp')) {
            var dataTable = $("#tb-info-mp").DataTable();
            dataTable.clear().draw();
            dataTable.destroy();
        }
    };

    self.drawTable = function () {          

        $('#tb-info-mp').DataTable({
            processing: true,
            select: false,
            scrollY: true,
            scrollX: true,
            scrollCollapse: true,
            columnDefs: [
                { width: "10", targets: 0 },
                { width: "10", targets: 1 },
                { width: "10", targets: 2 },
                { width: "10", targets: 3 },
                { width: "10", targets: 4 },
                { width: "10", targets: 5 },
                { width: "10", targets: 6 }
            ],
            fixedColumns: {
                rightColumns: 1
            },
            "lengthChange": false,
            initComplete: function () {
                $("#tb-info-mp_filter").remove();
                $("#search-tb-info-mp").prop("disabled", false);
            },
            language: {
                "lengthMenu": "Listar _MENU_",
                "zeroRecords": "Nada encontrado - lo siento",
                "info": "Mostrando página _PAGE_ de _PAGES_",
                "infoEmpty": "No hay registros disponibles",
                "infoFiltered": "(filtered from _MAX_ total records)",
                "sSearch": "Buscar:",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                }
            }
        });
    };

    self.onChageBancos = function () {
        self.numeroBanco(undefined);
    };

    self.cambiarMedioPago = function (data) {
        //if (data === undefined)
        //    return;

        //self.identificationType(data.TipoDocumento);
        //self.filtro(data.NumeroDocumento);
        //self.rmtCont(data.Id_Contrato);
        //self.medioPagoActual(data.DescripcionTarjeta);
        $('.modal').modal('hide');
        $('#smartwizard').smartWizard("next");
    };
}

var reporteVM = new IndexViewModel();
ko.applyBindings(reporteVM);

$(document).ready(function () {

    //indexVM.getInfoMedioPago();
    reporteVM.getBancos();

    $('#smartwizard').smartWizard({
        keyNavigation: false,
        useURLhash: false,
        theme: 'circles',
        transitionEffect: 'fade',
        transitionSpeed: '400',
        showStepURLhash: false,
        selected: 0,
        toolbarSettings: {
            showNextButton: false,
            showPreviousButton: false
        }
    });

    var IdContrato = getParameterByName("IdContrato");
    if (IdContrato !== "") {
        $('#smartwizard').smartWizard("next");
    }

    if (data === undefined)
        return;

    self.identificationType(TipoDocumento);
    self.filtro(NumeroDocumento);
    self.rmtCont(IdContrato);
    self.medioPagoActual(DescripcionTarjeta);

    $('#smartwizard').smartWizard("next");

});

function getParameterByName(name) //courtesy Artem
{
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results === null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

$("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {
    switch (stepNumber) {
        case 1:
            break;
    }

});

$('input[type=radio][name=optradio]').change(function () {
    reporteVM.tipoTarjeta("C");
});

$('input[type=radio][name=opTjRadio]').change(function () {
    reporteVM.tipoTarjeta($("input[name='opTjRadio']:checked").val());
});