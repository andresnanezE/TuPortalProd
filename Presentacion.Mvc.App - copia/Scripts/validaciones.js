

var elementos = document.getElementsByClassName('ValidacionScriptPrevent');
var elementosNumericos = document.getElementsByClassName('ValidacionOnlyNumber');
var elementosLetras = document.getElementsByClassName('ValidacionOnlyletterNumber');


for (var i = 0; i < elementos.length; i++) {
    elementos[i].oncopy = LockFucntion;
    elementos[i].oncut = LockFucntion;
    elementos[i].addEventListener("keypress", keyPressHandler, true);
    elementos[i].oncontextmenu = onContextMenu;
}

for (var i = 0; i < elementosNumericos.length; i++) {
    elementosNumericos[i].addEventListener("keypress", keyPressHandlerNumeric, true);
    elementosNumericos[i].oncontextmenu = onContextMenu;
    elementosNumericos[i].oncut = LockFucntion;
    elementosNumericos[i].oncopy = LockFucntion;
}
for (var i = 0; i < elementosNumericos.length; i++) {
    elementosLetras[i].addEventListener("keypress", keyPressHandlerLetterNumber, true);
    elementosLetras[i].oncontextmenu = onContextMenu;
    elementosLetras[i].oncut = LockFucntion;
    elementosLetras[i].oncopy = LockFucntion;
}


function keyPressHandler(event) {
    if (event.charCode != 13 || event.charCode != 32) {
        var regex = new RegExp("^[a-zA-Z0-9ñÑ*@@_.+-]$");
        var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
        if (!regex.test(key)) {
            event.preventDefault();
            //TODO:Nsarmiento:colocar mecanismo para alertar que se esta ingresado un caracter indebido
            return false;
        }
    }
};

function keyPressHandlerNumeric(e) {
    if (!/^([0-9])*$/.test(e.key))
        e.preventDefault();
};
function keyPressHandlerLetterNumber(e) {
    if (!/^([a-zA-Z])*$/.test(e.key))
        e.preventDefault();
};

function LockFucntion() {
    return false;
}

function onContextMenu() {
    return false;
}


