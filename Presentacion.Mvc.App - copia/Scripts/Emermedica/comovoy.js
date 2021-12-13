(function (comovoy, $, undefined) {
    //nivel bateria para apoyo rodamiento
    var index = 0;
    var barrasBateria = '1';
    if ($('#capa2Apoyo').get(0) != undefined) {
        index = $('#capa2Apoyo').html().indexOf('[') + 1;
        barrasBateria = $('#capa2Apoyo').html().substring(index, index + 1);
        $('#capa2Apoyo').html($('#capa2Apoyo').html().substring(0, index - 1));
    }

    if (barrasBateria === '1')
        $('#bat1Apoyo').css('display', 'inline');
    else if (barrasBateria === '2')
        $('#bat2Apoyo').css('display', 'inline');
    else if (barrasBateria === '3')
        $('#bat3Apoyo').css('display', 'inline');
 /*
    if ($('#capa2Afiliaciones').get().length > 0) {
        //nivel bateria para afiliaciones
        index = $('#capa2Afiliaciones').html().indexOf('[') + 1;
        barrasBateria = $('#capa2Afiliaciones').html().substring(index, index + 1);

        if (barrasBateria === '1')
            $('#bat1').css('display', 'inline');
        else if (barrasBateria === '2')
            $('#bat2').css('display', 'inline');
        else if (barrasBateria === '3')
            $('#bat3').css('display', 'inline');

        $('#capa2Afiliaciones').html($('#capa2Afiliaciones').html().substring(0, index - 1));
    }

    //Afiliaciones
    $('#afiliaciones').mouseenter(function () {
        $(this).css('background-color', '#BBCA2F');

        //$(this).animate({ opacity: '0.4' }, "slow");
        //$(this).animate({ opacity: '1.0' }, "slow");

        $('#capa2Afiliaciones').css('display', 'inline');
        $('#capa1Afiliaciones').css('display', 'none');

        $('#imgCapa2Afiliaciones').css('display', 'inline');
        $('#imgCapa1Afiliaciones').css('display', 'none');

        $('#imgCapa2Afiliaciones').css({ 'padding-top': '80px' });
    });
    $('#afiliaciones').mouseleave(function () {
        $(this).css('background-color', '#1C5CA6');

        $('#capa2Afiliaciones').css('display', 'none');
        $('#capa1Afiliaciones').css('display', 'inline');

        $('#imgCapa2Afiliaciones').css('display', 'none');
        $('#imgCapa1Afiliaciones').css('display', 'inline');
    });

    //Apoyo
    $('#Apoyo').mouseenter(function () {
        $(this).css('background-color', '#BBCA2F');

        $('#capa2Apoyo').css('display', 'inline');
        $('#capa1Apoyo').css('display', 'none');

        $('#imgCapa2Apoyo').css('display', 'inline');
        $('#imgCapa1Apoyo').css('display', 'none');
    });
    $('#Apoyo').mouseleave(function () {
        $(this).css('background-color', '#1C5CA6');

        $('#batery').css('display', 'inline');
        $('#imagen').css('display', 'inline');

        $('#capa2Apoyo').css('display', 'none');
        $('#capa1Apoyo').css('display', 'inline');

        $('#imgCapa2Apoyo').css('display', 'none');
        $('#imgCapa1Apoyo').css('display', 'inline');
    });

    //Transmilenio
    $('#Transmilenio').mouseenter(function () {
        $(this).css('background-color', '#BBCA2F');

        $('#capa2Transmilenio').css('display', 'inline');
        $('#capa1Transmilenio').css('display', 'none');

        $('#imgCapa2Transmilenio').css('display', 'inline');
        $('#imgCapa1Transmilenio').css('display', 'none');
    });
    $('#Transmilenio').mouseleave(function () {
        $(this).css('background-color', '#1C5CA6');

        $('#capa2Transmilenio').css('display', 'none');
        $('#capa1Transmilenio').css('display', 'inline');

        $('#imgCapa2Transmilenio').css('display', 'none');
        $('#imgCapa1Transmilenio').css('display', 'inline');

    });

    //Cancelados
    $('#Cancelados').mouseenter(function () {
        $(this).css('background-color', '#BBCA2F');

        $('#capa2Cancelados').css('display', 'inline');
        $('#capa1Cancelados').css('display', 'none');

        $('#imgCapa2Cancelados').css('display', 'inline');
        $('#imgCapa1Cancelados').css('display', 'none');
    });
    $('#Cancelados').mouseleave(function () {
        $(this).css('background-color', '#1C5CA6');

        $('#capa2Cancelados').css('display', 'none');
        $('#capa1Cancelados').css('display', 'inline');

        $('#imgCapa2Cancelados').css('display', 'none');
        $('#imgCapa1Cancelados').css('display', 'inline');

    });

    //Reactivados
    $('#Reactivados').mouseenter(function () {
        $(this).css('background-color', '#BBCA2F');

        $('#capa2Reactivados').css('display', 'inline');
        $('#capa1Reactivados').css('display', 'none');

        $('#imgCapa2Reactivados').css('display', 'inline');
        $('#imgCapa1Reactivados').css('display', 'none');
    });
    $('#Reactivados').mouseleave(function () {
        $(this).css('background-color', '#1C5CA6');

        $('#capa2Reactivados').css('display', 'none');
        $('#capa1Reactivados').css('display', 'inline');

        $('#imgCapa2Reactivados').css('display', 'none');
        $('#imgCapa1Reactivados').css('display', 'inline');

    });

    //Estatus
    $('#Estatus').mouseenter(function () {
        $(this).css('background-color', '#BBCA2F');

        $('#capa2Estatus').css('display', 'inline');
        $('#capa1Estatus').css('display', 'none');

        $('#imgCapa2Estatus').css('display', 'inline');
        $('#imgCapa1Estatus').css('display', 'none');
    });
    $('#Estatus').mouseleave(function () {
        $(this).css('background-color', '#1C5CA6');

        $('#capa2Estatus').css('display', 'none');
        $('#capa1Estatus').css('display', 'inline');

        $('#imgCapa2Estatus').css('display', 'none');
        $('#imgCapa1Estatus').css('display', 'inline');

    });

    //Formularios
    $('#Formularios').mouseenter(function () {
        $(this).css('background-color', '#BBCA2F');

        $('#capa2Formularios').css('display', 'inline');
        $('#capa1Formularios').css('display', 'none');

        $('#imgCapa2Formularios').css('display', 'inline');
        $('#imgCapa1Formularios').css('display', 'none');
    });
    $('#Formularios').mouseleave(function () {
        $(this).css('background-color', '#1C5CA6');

        $('#capa2Formularios').css('display', 'none');
        $('#capa1Formularios').css('display', 'inline');

        $('#imgCapa2Formularios').css('display', 'none');
        $('#imgCapa1Formularios').css('display', 'inline');

    });

    //PlanVuelo
    $('#PlanVuelo').mouseenter(function () {
        $(this).css('background-color', '#BBCA2F');

        $('#capa2PlanVuelo').css('display', 'inline');
        $('#capa1PlanVuelo').css('display', 'none');

        $('#imgCapa2PlanVuelo').css('display', 'inline');
        $('#imgCapa1PlanVuelo').css('display', 'none');
    });
    $('#PlanVuelo').mouseleave(function () {
        $(this).css('background-color', '#1C5CA6');

        $('#capa2PlanVuelo').css('display', 'none');
        $('#capa1PlanVuelo').css('display', 'inline');

        $('#imgCapa2PlanVuelo').css('display', 'none');
        $('#imgCapa1PlanVuelo').css('display', 'inline');

    });

    if ($('#capa2Reactivados').get().length > 0) {
        index = $('#capa2Reactivados').html().indexOf('[') + 1;
        $('#capa2Reactivados').html($('#capa2Reactivados').html().substring(0, index - 1));
    }*/
    /* new script dnmk*/

    $('.meta').hover(function () {
        $(this).find('.texto').toggleClass('hide-me');
    });

}(window.comovoy = window.comovoy || {}, jQuery));