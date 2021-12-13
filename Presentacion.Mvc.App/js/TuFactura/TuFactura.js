$("#mensajeData").hide();


function buscarContratos() {

    var numeroDoc = $("#NumeroDocumento").val();
    $("#mensajeData").hide();

    if ($.trim(numeroDoc) != "" && $.trim(numeroDoc) != "0" && $.trim(numeroDoc) != null) {

        $("#modalProcesando").modal('show');
        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: $("#urlObtenerContratosNit").val(),
            data: window.JSON.stringify({ numeroDoc: numeroDoc }),
            dataType: 'json',
            success: function (result) {

                $("#ContratoId").empty();
                if (result.length > 0) {

                    for (var i = 0; i < result.length; i++) {
                        $("#ContratoId").append("<option value='" + result[i]["RmtCont"] + "'>" + result[i]["CONTRATOCOMBO"] + "</option>");
                    }
                }
                else {
                    $("#mensajeData").show();
                }
            },
            error: function () {

                $("#modalProcesando").modal('hide');
            },
            complete: function () {

                $("#modalProcesando").modal('hide');


                if (($("#ContratoId").val() != "-1") && ($("#ContratoId").val() != "") && ($("#ContratoId").val() != undefined)) {
                    listadoFacturas();
                }
            }
        });
    }
}

$("#ContratoId").change(function () {

    if ($("#ContratoId").val() != "-1") {
        listadoFacturas();
    }
});

function reporteFactura(NUM_DOCU, COD_DOCU, PREFIJO, TIPO_DOCUMENTO, RMT_CONT) {

    $("#NUM_DOCU").val(NUM_DOCU);
    $("#COD_DOCU").val(COD_DOCU);
    $("#PREFIJO").val(PREFIJO);
    $("#TIPO_DOCUMENTO").val(TIPO_DOCUMENTO);
    $("#RMT_CONT").val(RMT_CONT);
    $("#NumeroDocumento2").val($("#NumeroDocumento").val());

    $("#reporteFactura").submit();
  }

function listadoFacturas() {

    $("#NumeroDocumento2").val($("#NumeroDocumento").val());
    $("#listado").submit();
}