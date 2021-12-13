/*
    Enero 2017.
    John Nelson Rodriguez G
    Cambiar la imagen del banner dependiendo
    el click sobre el menú.
 */
$(function () {
    var pathVirtual = $("#pathBanners").val() === '/' ? '' : $("#pathBanners").val();

    /*

        Test, directorio virtual base:

        console.log($("#pathBanners").val());

    */

    var m = localStorage.getItem('namemenu');

    if (m === "") {
        activemenu = localStorage.getItem('namemenu');
    }

    if (m == null) {
        changeImg('home');
        localStorage.setItem('namemenu', 'home');
    } else {
        if (location.pathname === pathVirtual + '/Home/') {
            changeImg('home');
            localStorage.setItem('namemenu', 'home');
        } else {
            $('#bnr img').data('ult-click-menu', m);
            changeImg(m);
            localStorage.setItem('namemenu', m);
        }
    }

    /*
        Click ménu padre:
    */
    $('a[href="#"]').mouseup(function (event) {
        var activemenu = $(this).get(0).innerText.trim();

        if (activemenu == "") {
            activemenu = localStorage.getItem('namemenu');
        }

        if ($(this).next().is(':visible')) {
            changeImg(localStorage.getItem('namemenu'));
        } else {
            changeImg(activemenu);
        }
    });

    /*
        Click ménu hijo:
    */
    $('ul.dropdown-menu, a[href="' + pathVirtual + '/Home/"]').on('click', function () {
        var menuhijo = "";

        if ($(this).hasClass('active'))
            menuhijo = 'home';
        else {
            /*
                Ir hasta el subménu para almacenar.
            */
            menuhijo =
                $(this).
                    parentsUntil("dropdown").
                    first().
                    children().
                    first().
                    find('span.hidden-xs').
                    text().trim();

            if (menuhijo.length > 0) {
                localStorage.setItem('namemenu', menuhijo);
            }
        }
    });

    /*
        Intercambiar imagen:
    */
    function changeImg(namemenu) {
        var n = pathVirtual + '/Image/Banner/*.jpg';

        if (namemenu === "null")
            namemenu = "home";

        n = n.replace('*', namemenu);

        $('#bnr').fadeOut("slow", function () {
            $('#bnr img').prop('src', n + '?dummy=271662');
        }).fadeIn("slow");

        $('#bnr img').error(function () {
            if (namemenu.toString().length > 0 && namemenu != "null")
                n = n.replace(namemenu, 'home');
            else
                // '/Image/Banner/home.jpg' esta imagen no debe faltar en esta ruta,
                // puede producir un bloqueo del navegar por recurrencia de errores.
                n = pathVirtual + '/Image/Banner/home.jpg';
            $('#bnr img').prop('src', n + '?dummy=271662');
            $('#bnr img').addClass("img-responsive");
        });
    }

    /*
        Cerrar sesión,
        Ir hasta el ménu de cerra sesión:
     */
    $('#menu.nav.main-menu').find('span.hidden-xs').last().on('click', function () {
        localStorage.setItem('namemenu', 'home');
    });
});