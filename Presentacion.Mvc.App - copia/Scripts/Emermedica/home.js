(function (home, $, undefined)
{
    //Como voy
    //nivel bateria para apoyo rodamiento
    var index = 0;
    var barrasBateria = '1';
    if ($('#capa2Apoyo').get(0) != undefined) {
        index = $('#capa2Apoyo').html().indexOf('[') + 1;
        barrasBateria = $('#capa2Apoyo').html().substring(index, index + 1);
        $('#capa2Apoyo').html($('#capa2Apoyo').html().substring(0, index - 1));
    }

    if (barrasBateria === '1')
        $('#bat1Apoyo').css({
            'display': 'inline',
            'position': 'relative',
            'left': '-5px'
        });
    else if (barrasBateria === '2')
        $('#bat2Apoyo').css({
            'display': 'inline',
            'position': 'relative',
            'left': '-5px'
        });
    else if (barrasBateria === '3')
        $('#bat3Apoyo').css({
            'display': 'inline',
            'position': 'relative',
            'left': '-5px'
        });

    if ($('#capa2Afiliaciones').get().length > 0) {
        //nivel bateria para afiliaciones
        index = $('#capa2Afiliaciones').html().indexOf('[') + 1;
        barrasBateria = $('#capa2Afiliaciones').html().substring(index, index + 1);

        if (barrasBateria === '1')
            $('#bat1Afiliaciones').css({
                'display': 'inline',
                'position': 'relative',
                'left': '-5px'
            });
        else if (barrasBateria === '2')
            $('#bat2Afiliaciones').css({
                'display': 'inline',
                'position': 'relative',
                'left': '-5px'
            });
        else if (barrasBateria === '3')
            $('#bat3Afiliaciones').css({
                'display': 'inline',
                'position': 'relative',
                'left': '-5px'
            });

        $('#capa2Afiliaciones').html($('#capa2Afiliaciones').html().substring(0, index - 1));
    }
    if ($('#capa2PlanVuelo').get().length > 0) {
        //nivel bateria para plan de vuelo 1
        index = $('#capa2PlanVuelo').html().indexOf('[') + 1;
        barrasBateria = $('#capa2PlanVuelo').html().substring(index, index + 1);

        if (barrasBateria === '1')
            $('#bat1PlanVuelo').css({
                'display': 'inline',
                'position': 'relative',
                'left': '-5px'
            });
        else if (barrasBateria === '2')
            $('#bat2PlanVuelo').css({
                'display': 'inline',
                'position': 'relative',
                'left': '-5px'
            });
        else if (barrasBateria === '3')
            $('#bat3PlanVuelo').css({
                'display': 'inline',
                'position': 'relative',
                'left': '-5px'
            });

        $('#capa2PlanVuelo').html($('#capa2PlanVuelo').html().substring(0, index - 1));
    }
    if ($('#capa2PlanVuelo2').get().length > 0) {
        //nivel bateria para plan de vuelo 2
        index = $('#capa2PlanVuelo2').html().indexOf('[') + 1;
        barrasBateria = $('#capa2PlanVuelo2').html().substring(index, index + 1);
        $('#capa1PlanVuelo2.texto').css('width', '61%');
        if (barrasBateria === '1')
            $('#bat1PlanVuelo2').css({
                'display': 'inline',
                'position': 'relative',
                'left': '-5px'
            });
        else if (barrasBateria === '2')
            $('#bat2PlanVuelo2').css({
                'display': 'inline',
                'position': 'relative',
                'left': '-5px'
            });
        else if (barrasBateria === '3')
            $('#bat3PlanVuelo2').css({
                'display': 'inline',
                'position': 'relative',
                'left': '-5px'
            });

        $('#capa2PlanVuelo2').html($('#capa2PlanVuelo2').html().substring(0, index - 1));
    }
    if ($('#capa2Reactivados').get().length > 0) {
        index = $('#capa2Reactivados').html().indexOf('[') + 1;
        $('#capa2Reactivados').html($('#capa2Reactivados').html().substring(0, index - 1));
    }

    $('.meta').hover(function ()
    {
        $(this).find('.texto').toggleClass('hide-me');
    });

    traerBanners();
    traerDestacados();
    traerNoticias();

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // FUNCIONES PRIVADAS
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    function traerBanners()
    {
        $.ajax
        ({
            type: 'POST',
            url: urlTraerBanners,
            data: {},
            dataType: "json",
            beforeSend: function ()
            {
                //AjaxUpdateProgressShow();
            },
            success: function (data)
            {
                var banners = data.data;
                var bannersTemplate = kendo.template($("#banners-template").html());
                $("#banners-container").html(bannersTemplate(banners));

                var flkty = null;
                setTimeout(function ()
                {
                    //Initialize carrousel
                    flkty = $('#slider-banners').flickity(
                    {
                        groupCells: 1,
                        wrapAround: true,
                        cellAlign: "left",
                        autoPlay: 2000,
                        arrowShape:
                        {
                            x0: 10,
                            x1: 80,
                            y1: 50,
                            x2: 80,
                            y2: 0,
                            x3: 80
                        },
                        pageDots: false
                    })

                    //flkty.playPlayer();
                }, 200);
            },
            error: function (request, status, error)
            {
                //alert(request.responseText);
                console.log(error);
            }
        });
    }

    function traerDestacados()
    {
        $.ajax
        ({
            type: 'POST',
            url: urlTraerDestacados,
            data: {},
            dataType: "json",
            beforeSend: function ()
            {
                //AjaxUpdateProgressShow();
            },
            success: function (data)
            {
                var template = kendo.template($("#destacado-template").html());
                var templateensitio = kendo.template($("#destacado-ensitio-template").html());

                for (var i = 0; i < data.length; ++i)
                {
                    var destacado = data[i];
                    if (destacado.ABRIRENSITIO)
                    {
                        $("#destacado" + i).html(templateensitio(destacado));
                    }
                    else 
                    {
                        $("#destacado" + i).html(template(destacado));
                    }
                }

                $('.clickmodal').click(function ()
                {
                    var link = $(this).attr('data-url');
                    $('#destacado-modal iframe').attr('src', link);
                    $('#destacado-modal').modal('show');
                });
            },
            error: function (request, status, error)
            {
                //alert(request.responseText);
                console.log(error);
            }
        });
    }

    function traerNoticias()
    {
        $.ajax
        ({
            type: 'POST',
            url: urlTraerNoticias,
            data: {},
            dataType: "json",
            beforeSend: function ()
            {
                //AjaxUpdateProgressShow();
            },
            success: function (data)
            {
                var noticias = data.data;
                var noticiasTemplate = kendo.template($("#noticias-template").html());
                $("#noticias-container").html(noticiasTemplate(noticias));
            },
            error: function (request, status, error)
            {
                //alert(request.responseText);
                console.log(error);
            }
        });
    }

}(window.home = window.home || {}, jQuery));