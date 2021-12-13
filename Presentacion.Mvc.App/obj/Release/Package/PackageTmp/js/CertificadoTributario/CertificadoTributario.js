
$("#mensajeData").hide();


function buscarContratos() {

    var numeroDoc = $("#NumeroDoc").val();
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
            }
        });
    }
}


function validaCertificado() {

    console.log("aaaa");

    var numeroDoc = $("#NumeroDoc").val();
    var rmt_cont = $("#ContratoId").val();

    if ($.trim(numeroDoc) != "" && $.trim(numeroDoc) != "0" && $.trim(numeroDoc) != null) {
        if (rmt_cont != "-1" && rmt_cont != "" && rmt_cont != null && rmt_cont != undefined) {
            $("#certificado").submit();
        }
    }
    else {
        if ($.trim(numeroDoc) != "" && $.trim(numeroDoc) != "0" && $.trim(numeroDoc) != null) {
            $("#L_mensaje").html("Debe agregar un Número de documento !");
        }
        else if (rmt_cont != "-1" && rmt_cont != "" && rmt_cont != null && rmt_cont != undefined) {
            $("#L_mensaje").html("Debe seleccionar un contrato !");
        }
        $("#mensajeData").show();
    }
}