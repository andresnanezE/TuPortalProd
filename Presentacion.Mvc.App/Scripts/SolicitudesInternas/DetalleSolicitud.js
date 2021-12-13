///-- JOhn Nelson Rodriguez
///-- Manejo del poup Enviar Notas
///-- Enero 2017
$(function () {
    $('#Attach').filer({
        showThumbs: true,
        limit: 3,
        maxSize: 3,
        extensions: ["jpg", "png", "pdf", "xls", "xlsx", "doc", "docx"],
        addMore: true,
        allowDuplicates: false
    });

    $('.d-detalle').on('click', function () {
        var $item = $(this);
        $('#ialert').css('display', 'none');

        var ticket = {
            'displayid': $item.data('displayid'),
            'id': $item.data('id'),
            'subject': $item.data('subject'),
            'ciudad': $item.data('ciudad'),
            'treq': $item.data('treq'),
            'area': $item.data('area'),
            'status': $item.data('status'),
            'fcrea': $item.data('fcrea'),
            'url_attach': $item.data('url_attach'),
            'ffin': $item.data('ffin'),
            'content_file_name': (typeof $item.data('content_file_name')) == 'undefined' ? '' : $item.data('content_file_name')
        };

        $('.fcrea').html(ticket.fcrea.substring(0, 10));
        $('.ffin').html(ticket.ffin.substring(0, 10));
        $('.area').html(ticket.area);
        $('.ciudad').html(ticket.ciudad);
        $('.subject').html(ticket.subject.length > 20 ? ticket.subject.substr(0, 17) + '...' : ticket.subject);
        $('.treq').html(ticket.treq);
        $('.displayid').html(ticket.displayid);
        $('#IdTicket').val(ticket.displayid);
        $('.id').html(ticket.displayid);

        $('#url_attach').html('');

        $('#url_attach').append('<option value="0">-- seleccione--</option>');

        if (ticket.content_file_name.split(";").length > 0) {
            for (var nfile in ticket.url_attach.split(";")) {
                if (ticket.content_file_name.split(';')[nfile].length > 0)
                    $('#url_attach').append('<option value="' + ticket.url_attach.split(";")[nfile] + '">' + ticket.content_file_name.split(';')[nfile] + '</a>' + '</option>');
            }
        }

        $('#url_attach').on('change', function () {
            if ($(this).val() != "0")
                window.open($(this).val(), '_blank');
        });

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            data: "{'idTicket':'" + ticket.displayid + "'}",
            url: $('#urlActionGetNotes').val(),
            dataType: "json",
        }).done(function (items) {
            var str = '<hr>';

            for (var i in items) {
                var attach = items[i].url_attach == null ? '' : items[i].url_attach;
                var name_attach = items[i].content_file_name == null ? '' : items[i].content_file_name;

                str = str + new Date(parseInt(items[i].created_at.substr(6))).toLocaleString() + '<br>';
                str = str + items[i].body + '<br>';
                str = str + (typeof (items[i].url_attach) == 'undefined' ? '<br>' :
                    '<a href="' + attach + '" target= "_blank">' + name_attach + '</a>');
                str = str + '<hr>';
            }

            $('#notas').html('');

            $('#notas').append(str);

            $('#modalDetalle').modal('toggle');
        }).fail(function () {
            $('.modal-alert').html('Ha surgido algo inesperado y no ha sido posible obtener los tickets.');

            $('#modalAlert').modal('toggle');
        });
    });
});

function validar() {
    var $nota = $("#Nota");
    if ($nota.val().length <= 3 || $nota.val().length > 100) {
        var error = new PNotify({
            title: 'Mensaje',
            text: 'El campo nota requiere [5-100] caractéres.',
            type: 'error'
        });
    } else {
        $('#modal-wait').modal('toggle');
        $('#notas-form').submit();
    }
}