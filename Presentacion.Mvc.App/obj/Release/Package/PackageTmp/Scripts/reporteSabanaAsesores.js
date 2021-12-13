function pestana(grupo_pesta, id_pesta) {
    alert(id_pesta);

    //var pestanas = document.getElementById(grupo_pesta);
    //var Tpestanas = pestanas.getElementsByTagName("div");
    //for (var i = 0; i < Tpestanas.length; i++) {
    //    //Tpestanas[i].style.zindex = "-1000";
    //    Tpestanas[i].style.visibility = "hidden";
    //}
    if (id_pesta == 'seccion-resumen-estatus') {
        alert("si");
        document.getElementById("seccion_resumen").style.visibility = "hidden";
        alert("pere");
        document.getElementById("seccion_resumen-estatus").style.display = 'block';
    }
    else { alert("no"); }
    //document.getElementById(id_pesta).style.zIndex = "1000";
    //document.getElementById(id_pesta).style.visibility = "visible";
}