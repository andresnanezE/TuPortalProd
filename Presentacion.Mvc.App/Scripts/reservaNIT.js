

$(document).ready(function () {
    $('#txCorreoElectronico').blur(function () {
        re = /([A-Z0-9a-z_-][^@])+?@[^$#<>?]+?\.[\w]{2,4}/.test(this.value);

        if (!re) {
            toastr.warning("Ingresa una dirección de correo válida", "Advertencia");
        }
    });

    $('#input-nit').on('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    $('#txTelefono').on('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });
    $('#txCelular').on('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    $('#txExt').on('input', function () {
        this.value = this.value.replace(/[^0-9]/g, '');
    });

    ////jQuery('#input-nit').keyup(function () {
    ////    this.value = this.value.replace(/[^0-9]/g, '');
    ////});
});




