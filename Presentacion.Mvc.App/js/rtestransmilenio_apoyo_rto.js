$(document).ready(function () {
    var periodos = dates().reverse();
    var pf2Fin = periodos[0].periodoFin;
    var pNoCerrados = [];

    $("#sDefinitivo").html('No');
    $("input#Definitivo").val('N');
    $("select#Definitivo").val('N');

    modal_wait('CARGANDO PERIODOS...');

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        //url: window.location.href.indexOf('localhost') >= 0 ?
        //      $('#path-controler').attr('data-path') :
        //    "/SCI/HerramientasComercialesPruebas" + $('#path-controler').attr('data-path'),
        url: $('#path-controler').attr('data-path'),
        dataType: "json",
    }).done(function (datar) {
        pNoCerrados = datar;

        //fillSelectPeriodoIni(periodos, pNoCerrados);

        fillSelectPeriodoIni(pNoCerrados);
        var _date = new Date(pf2Fin.anio, +pf2Fin.mes - 1, pf2Fin.dia);
        $('#PeriodoFin').append('<option value="' + formatDate(_date).format + '">' + formatDate(_date).format + '</option>');
        $('#popup_wait').dialog('close');
    }).fail(function () {
        $('#informe-consolidado').attr('disabled', true);
        $('#popup_wait').dialog('close');
        modal('Error cargando los periodos.', null, { "Cerrar": function () { $(this).dialog("close"); } });
    });

    $('#PeriodoIni').on('change', function () {
        fillSelectPeriodoFin(pNoCerrados);

        if ($("option:selected", this).text() === $('#PeriodoIni option').get(0).value) {
            $('select#Definitivo').attr('title', 'Periodo no cerrado!');
            $("#sDefinitivo").html('No');
            $("input#Definitivo").val('N');
            $("select#Definitivo").val('N');
        } else {
            $('select#Definitivo').attr('title', 'Periodo ya cerrado!');
            $("#sDefinitivo").html('Si');
            $("input#Definitivo").val('S');
            $("select#Definitivo").val('S');
        }
    });
    $("input#informe-consolidado").on("click", function () {
        $("form#consulta-form input[name=ExportarFormato]").val("Excel");
        $("form#consulta-form").submit();
    }); // fin on.click
});
function modal_wait(msg) {
    $('<div id="popup_wait">' + msg + '</div>').dialog({
        height: 100,
        width: 200,
        modal: true,
        position: { of: $("#consulta-form"), my: 0, at: 0 },
        open: function () {
            $(this)
                .closest(".ui-dialog")
                .find(".ui-dialog-titlebar:first")
                .hide();
        },
        dialogClass: 'ui-dialog-content1'
    });
}
function modal(msg, p, _buttons) {
    $('<div id="popup">' + msg + '</div>')
        .dialog({
            modal: true, title: 'Reporte', zIndex: 10000, autoOpen: true,
            width: 420, resizable: false,
            //position: [50, 50],
            buttons: !_buttons ? {
                "Aceptar": function () {
                    p();
                    $(this).dialog("close");
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            } : _buttons,
            close: function (event, ui) {
                $(this).remove();
            }
        });
}

function fillSelectPeriodoIni(periodos) {
    $('#PeriodoIni option').remove();

    for (var f in periodos) {
        $('#PeriodoIni').append('<option value="' + periodos[f].Item1 + '">' + periodos[f].Item1 + '</option>');
        $('#_hideselect').append('<option value="' + periodos[f].Item1 + '">' + periodos[f].Item2 + '</option>');
    } // fin for
}
function fillSelectPeriodoFin(pCerrados) {
    var periodoIniSeleccionado = $('#PeriodoIni option:selected').text();
    var options = $('#_hideselect option').get().reverse();
    var index = -1;
    var _array = [];

    $('#PeriodoFin option').remove();

    for (var p in options) {
        if (options[p].value === periodoIniSeleccionado) {
            index = +p + 1;
            if (options[index] !== undefined) {
                periodoIniSeleccionado = options[index].value;
                _array[p] = options[p].text;
            } else {
                _array[p] = options[p].text;
            }
        }
    }

    if (_array.length > 0) {
        _array = _array.reverse();

        _array.forEach(function (item) {
            $('#PeriodoFin').append('<option value="' + item + '">' + item + '</option>');
        });
    }
}
function dates(_mes, _anio) {
    var fIni = new Date();
    var dia = 26;
    var mes = 0;
    var anio = 2015;
    var periodos = [];

    if (arguments.length > 0) {
        mes = _mes - 1;
        anio = _anio;
    }
    else {
        for (var ntimes = 1; ntimes <= 12; ++ntimes) {
            fIni = new Date(fIni.getFullYear(), fIni.getMonth() - 1, 25);
        }

        mes = fIni.getMonth();
        anio = fIni.getFullYear();
    }

    fIni = new Date(anio, mes, dia);
    var hoy = new Date();

    ///var diaHoy = hoy.getDate();
    var mesHoy = hoy.getMonth() - 1;
    var anioHoy = hoy.getFullYear();

    while (fIni.getFullYear() < anioHoy || !(fIni.getMonth() == mesHoy + 1)) {
        var fFin = new Date(anio, mes + 1, 25);
        var periodo =
        {
            periodoIni: formatDate(fIni),
            periodoFin: formatDate(fFin),
        };
        periodos.push(periodo);
        mes = mes + 1;
        fIni = new Date(anio, mes, dia);
    }

    return periodos;
}
function formatDate(d) {
    var dd = d.getDate();

    if (dd < 10) dd = '0' + dd;
    var mm = d.getMonth() + 1;
    if (mm < 10) mm = '0' + mm;
    //var yy = d.getFullYear() % 100
    //if ( yy < 10 ) yy = '0' + yy

    var fecha =
    {
        dia: dd,
        mes: mm,
        anio: d.getFullYear(),
        format: dd + '/' + mm + '/' + d.getFullYear()
    };

    return fecha;
}
function getSelectPeriodo(select) {
    var sleccion = $('#' + select + ' option:selected').text();
    var mes = +sleccion.substring(3, 5);
    var anio = +sleccion.substring(6, 10);
    return dates(mes, anio);
}
function esNoDefinitivo(select) {
    var sleccion = $('#' + select + ' option:selected').text();
    return sleccion.indexOf('Definitivo') >= 0;
}
function esDefinitivoSelect() {
    var sleccion = $('#Definitivo option:selected').text();
    return sleccion === 'Si';
}
function eliminarNoDefinitivoSeleccionados(periodos, pNoCerrados) {
    for (var p in periodos) {
        for (var p2 in pNoCerrados) {
            if (pNoCerrados[p2] == periodos[p].substring(13, 23)) {
                pNoCerrados.splice(p2, 1);
                break;
            }
        }
    }
}