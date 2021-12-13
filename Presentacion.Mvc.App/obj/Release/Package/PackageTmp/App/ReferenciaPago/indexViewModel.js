var ReferenciaPagoViewModel = function () {

    var self = this;

    self.tipoAfiliacion = ko.observable('nuevo');
    self.cedulaNuevo = ko.observable('');
    self.numeroContratoInclusion = ko.observable();
    self.cedulaContratante = ko.observable();
    self.cedulaBeneficiario = ko.observable();
    self.validezContratoInclusion = ko.observable('');
    self.referenciaGenerada = ko.observable('');
    self.verBotonEnviarReferencia = ko.observable(false); 2
    self.tituloInfo = ko.observable();
    self.resultadoInfo = ko.observable();

    self.validating = function () {
        if (self.tipoAfiliacion() === 'inclusion') {
            if (self.numeroContratoInclusion() === undefined ||
                self.numeroContratoInclusion() === null ||
                self.numeroContratoInclusion() === '') {
                return false;
            }
            if (self.cedulaContratante() === undefined ||
                self.cedulaContratante() === null ||
                self.cedulaContratante() === '') {
                return false;
            }
            if (self.cedulaBeneficiario() === undefined ||
                self.cedulaBeneficiario() === null ||
                self.cedulaBeneficiario() === '') {
                return false;
            }
        } else {
            if (self.cedulaNuevo() === undefined ||
                self.cedulaNuevo() === null ||
                self.cedulaNuevo() === '') {
                return false;
            }
        }
        return true;
    };

    self.tipoAfiliacion.subscribe(function () {
        self.cedulaNuevo('');
        self.numeroContratoInclusion('');
        self.cedulaContratante('');
        self.cedulaBeneficiario('');
        self.referenciaGenerada('');
        self.validezContratoInclusion('');
        self.verBotonEnviarReferencia(false);
    });

    self.generarReferencia = function () {
        $.ajax({ success: function () { $('#btn-generar-referencia').attr('value', 'Generando...') } });
        self.verBotonEnviarReferencia(false);
        self.referenciaGenerada('');
        self.validezContratoInclusion('');
        var validado = false;
        validado = self.validating();
        if (validado) {
            $.ajax({
                url: "/ReferenciaPago/GenerarReferencia",
                method: "POST",
                data: {
                    Cedula: self.cedulaNuevo(),
                    isInclusion: (self.tipoAfiliacion() === 'inclusion'),
                    NumeroContratoInclusion: self.numeroContratoInclusion(),
                    CedulaContratante: self.cedulaContratante(),
                    CedulaBeneficiario: self.cedulaBeneficiario()
                },
                success: function (data) {
                    if (data.Exitoso) {
                        if (data.IsInclusion) {
                            self.validezContratoInclusion("<label>Estado del contrato</label>: " +
                                (data.ContratoValido ? 'VÁLIDO' : 'NO VÁLIDO'));
                            if (data.ContratoValido) {
                                self.verBotonEnviarReferencia(true);
                                self.referenciaGenerada(data.ReferenciaPago.replace('"', ''));
                                self.referenciaGenerada("<label>Referencia</label>: " +
                                    self.referenciaGenerada().replace('"', '') +
                                    "<br>* Este número se utiliza para realizar el pago correspondiente a la afiliación");
                                $('#btn-generar-referencia').attr('value', 'Generar');
                            } else {
                                self.tituloInfo('Contrato no válido');
                                self.resultadoInfo('El contrato proporcionado no es válido, ya que se encuentra bloqueado, ' +
                                    'cancelado o no existe.');
                                $("#mdl-info").modal();
                                self.verBotonEnviarReferencia(false);
                                self.referenciaGenerada('');
                                $('#btn-generar-referencia').attr('value', 'Generar');
                            }
                        }
                        else {
                            self.verBotonEnviarReferencia(true);
                            self.referenciaGenerada(data.ReferenciaPago.replace('"', ''));
                            self.referenciaGenerada("<label>Referencia</label>: " +
                                self.referenciaGenerada().replace('"', '') +
                                "<br>* Este número se utiliza para realizar el pago correspondiente a la afiliación");
                            $('#btn-generar-referencia').attr('value', 'Generar');
                        }
                    } else {
                        self.resultadoInfo(data.Mensaje);
                        $("#mdl-info").modal();
                        $('#btn-generar-referencia').attr('value', 'Generar');
                    }
                }
            });
        } else {
            self.tituloInfo('Datos incompletos');
            self.resultadoInfo("Asegúrese de diligenciar los campos obligatorios");
            $("#mdl-info").modal();
            $('#btn-generar-referencia').attr('value', 'Generar');
        }
    };
};

var refVM = new ReferenciaPagoViewModel();
ko.applyBindings(refVM);