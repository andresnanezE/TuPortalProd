/// John Nelson Rodriguez
/// Enero 2017
    $(function () {

        $('.table').dataTable({
            oLanguage: {
                sEmptyTable: "No se encuentran datos para esta búsqueda"
            },
            scrollX: false,
            scrollCollapse: false,
            bFilter: false,
            bInfo: false,
            bPaginate: false,
        });

        $('#Modificar').val("No");
        $('#cancelar').prop("disabled", true);

        document.getElementById("Id").readOnly = true;
        document.getElementById("Id").style.color = "#c0c0c0";

        //Manejar el evento de perdida del foco del campo del documento.
        $('#Codusuario').focusout(function () {


            ObtValoresCamposFormulario();

        });

        $('#Codusuario').on('click', function () {
            $('.alert').toggle("slow");
        });

        $('#cancelar').on('click', function () { location.reload(); });

        //Manejar el evento clic de la imagen del cambio de estado del usuario:
        $(".activar, .desactivar").on("click", function () {

            var estado = '';
            $('.alert').hide();

            estado = $(this).hasClass("activar") ? "A" : "I";

            $.wait('ACTUALIZANDO ESTADO DE USUARIO...');

            $.ajax({
                method: "POST",
                url: $("#ActEstadoUsuario").val(),
                data: '{"id":"' + $(this).data('id') + '","estado":"' + estado + '"}',
                contentType: "application/json; charset=utf-8"
            }).done(function (data) {
                if (data.Error != null) {
                    alert('El registro no se actualizo.');
                }
                else {
                    location.href = $("#Index").val();
                    $.closeWait();
                }
            }).fail(function () {
                $('.modal-alert').html("Ocurrio un error");

                $('#modalAlert').modal('toggle');
            });
        });

        //Manejar evento de actualizar
        $('.editar').on('click', function () {

            $('#Codusuario').val($(this).data('doc'));
            $('.title-usuario').html('Modificar usuario.');
            $('.alert').hide();

            document.getElementById("Codusuario").readOnly = true;
            document.getElementById("Codusuario").style.color = "#c0c0c0";

            $('.btn-crear').html(' Modificar.');
            $('#cancelar').prop("disabled", false);
            $('#Modificar').val("Si");
            ObtValoresCamposFormulario($(this).data('id'));
        });

        function ObtValoresCamposFormulario(editarId) {
            var $intoDoc = $('#Codusuario');

            if (!$.isNumeric($intoDoc.val())) {
                $('select').html('');
                $('#enviar').prop("disabled", true);
                $('#Nombre').val('');
                $('#Codusuario').val('');

                return;
            }

            $.wait('RECUPERANDO INFORMACIÓN DE USUARIO...');

            $.ajax({
                type: 'POST',
                url: $("#ObtValoresCamposFormulario").val(),
                data: '{"doc":"' + $intoDoc.val() + '","id":"' + (editarId ? editarId : 0) + '"}',
                contentType: "application/json; charset=utf-8",
            }).done(function (data) {

                if ((typeof editarId) == 'undefined' && data.Error != null) {
                    $.closeWait();
                    $('select').html('');
                    $('#enviar').prop("disabled", true);
                    $('#Nombre').val('');
                    $('#Codusuario').val('');

                    $('.modal-alert').html(data.Error);

                    $('#modalAlert').modal('toggle');

                    return;
                }

                //fill select canal:
                $('#Canal').html('');
                data.CanalUsuario.forEach(function (canal) {
                    $('#Canal').append('<option value="' + canal.Value + '">' + canal.Text + '</option>');
                });
                //fill select perfil:
                $('#Perfil').html('');
                data.PerfilUsuario.forEach(function (perfil) {
                    $('#Perfil').append('<option value="' + perfil.Value + '">' + perfil.Text + '</option>');
                });
                //fill select ciudad:
                $('#Ciudad').html('');
                data.CiudadUsuario.forEach(function (ciudad) {
                    $('#Ciudad').append('<option value="' + ciudad.Value + '">' + ciudad.Text + '</option>');
                });
                //fill select segmento:
                if (data.SegmentoUsuario.length > 0) {
                    $('.segmento').show();
                    $('#Segmento').html('');
                    data.SegmentoUsuario.forEach(function (segmento) {
                        $('#Segmento').append('<option value="' + segmento.Value + '">' + segmento.Text + '</option>');
                    });
                }
                else {
                    $('.segmento').hide();
                }

                $('#Nombre').val(data.Nombre);
                if (data.Id > 0) {
                    $('#conten-id').removeClass('hidden');
                }

                $('#Idlbl').html(data.Id);
                $('#Id').val(data.Id);
                //$('#Codusuario').val(data.Codusuario);

                $('#enviar').prop("disabled", false);
                $.closeWait();

            }).fail(function () {
                $.closeWait();
                
                $('.modal-alert').html("Ocurrio un error");

                $('#modalAlert').modal('toggle');

            });
        }

    });
