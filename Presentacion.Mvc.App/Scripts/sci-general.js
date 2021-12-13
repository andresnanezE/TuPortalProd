// #region --MODAL--
function openModal(title, type, message, title2) {
    $(".btn-m-cancelar").removeClass("col-md-6").removeClass("visible-sm").addClass("col-md-6");
    $(".btn-m-aceptar").removeClass("col-md-6").removeClass("col-md-12").addClass("col-md-6");

    switch (type) {
        case "alert":
            $("#m-img").attr("src", "/Content/images/sci-alert.svg");
            break;
        case "confir":
            $("#m-img").attr("src", "/Content/images/sci-confirmar.svg");
            $(".btn-m-cancelar").addClass("visible-sm");
            $(".btn-m-aceptar").removeClass("col-md-6").addClass("col-md-12");
            break;
        case "renovar":
            $("#m-img").attr("src", "/Content/images/sci-renovar.svg");
            break;
        default:
    }

    $("#m-title").text(title);
    $("#m-title-2").text(title2);
    $("#m-message").text(message);
    $("#m-cotizacion").modal();
}
// #endregion MODAL