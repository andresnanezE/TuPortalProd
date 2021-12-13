var listaRmtCont = new Array();
$("#mensajeData").hide();


$("#TipoEnvio").on('change', function (value) {
    //RMT-CONT de clientes recurrentes. Value = RMT
    //Todos. Value = Todos
    var tipoEnvioSeleccionado = $("#TipoEnvio").val();
    if (tipoEnvioSeleccionado === "RMT") {
        $("#pnl-archivo").css('display', 'block');
    } else {
        $("#pnl-archivo").css('display', 'none');
    }
});
function solicitarCertificados() {
    $("#mensajeData").hide();
    $("#modalProcesando").modal('show');
    if ($("#TipoEnvio").val() == "RMT" && listaRmtCont.length === 0) {
        $("#modalProcesando").modal('hide');
        $("#modalAlert").modal('show');
        $("#mensajeResultado").text("Proporcione una lista de RMT-CONT recurrentes mediante un archivo Excel");
    } else {
        $.ajax({
            type: 'POST',
            data: {
                model: {
                    Año: $("#A_o").val(),
                    TipoContrato: $("#TipoContrato").val(),
                    FechaMaximaEnvio: $("#FechaMaximaEnvio").val(),
                    TipoEnvio: $("#TipoEnvio").val(),
                    RmtContRecurrentes: listaRmtCont
                }
            },
            url: '/CertificadoTributario/SolicitarCertificados',
            success: function (data) {
                $("#modalProcesando").modal('hide');
                $("#modalAlert").modal('show');
                $("#mensajeResultado").text(data.Mensaje);
                if (data.Exitoso) {
                    $("#TipoContrato").val("PPE");
                    $("#TipoEnvio").val("RMT");
                    $("#TipoEnvio").change();
                    $("#FechaMaximaEnvio").val('');
                    listaRmtCont = [];
                    document.getElementById("btnCargarArchivo").value = "";
                }
            }
        });
    }
}

var file = null;
document.getElementById("btnCargarArchivo").onchange = e => {
    file = e.target.files[0];
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        $.ajax({
            url: "/CertificadoTributario/GetRmtCont",
            method: "POST",
            data: { blobCsv: reader.result },
            success: function (data) {
                if (data.Message === "") {
                    if (data.List.length === 0 || data.List === undefined) {
                        $("#modalAlert").modal('show');
                        $("#mensajeResultado").text("El archivo no contiene ningún registro");
                        document.getElementById("btnCargarArchivo").value = "";
                    } else
                        listaRmtCont = data.List;
                } else {
                    $("#modalAlert").modal('show');
                    $("#mensajeResultado").text(data.Message);
                }
            }
        });
    };
};