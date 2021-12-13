//Clear form
jQuery.fn.reset = function () {
    $(this).each(function () { this.reset(); });
};

kendo.culture("es-CO");
var dateFormat = 'yyyy-MM-dd';

var mentorLoginCallback = Array();
var klogoutwindow;

var kendoValidationMessage =
{
    required: "Campo obligatorio",
    email: "Ingresa una dirección de correo válida",
    verifyPasswords: "Las contraseñas deben coincidir",
    date: "Ingrese una fecha válida",
    radio: "Debe seleccionar una opción",
    number: "Este campo debe ser numérico",
    url: "Debe ingresar una url válida"
};

var linkLocation;

$(function () {
    $('input, textarea').placeholder();

    var styles = $("style");
    var tmp = "";

    styles.each
        (
            function () {
                tmp += $(this).html() + "\r\n";
                $(this).remove();
            }
        );
    var newStyles = $("<style>" + tmp + "</style>");
    newStyles.appendTo($("head"));

    klogoutwindow = $("#logout-window").kendoWindow
        ({
            title: "Inicar Sesión",
            visible: false,
            width: "430px",
            draggable: true,
            modal: true,
            resizable: false,
            actions: []
        }).data("kendoWindow");

    $.ajaxSetup({ cache: false });
    $(document).ajaxError(function (event, request, options, thrownError) {
        //AjaxUpdateProgressHide();
        if (request.status === 401) {
            klogoutwindow.center();
            klogoutwindow.open();
            $("#logout-window").show();
        }
        else {
            if (thrownError === "timeout") {
                errorMessage("Error", "Timeout Error");
            }
            else {
                errorMessage("Error", "General Error");
            }
        }
    });

    //Yes/no radios
    yesNoRadios();

    //Button checkboxes
    btnCheckboxes();
});

//Ajax loader
function AjaxUpdateProgressShow() {
    $("#AjaxUpdateProgress").show();
}

function AjaxUpdateProgressHide() {
    $("#AjaxUpdateProgress").hide();
}

function redirectPage() {
    window.location = linkLocation;
}

function LoadCascadingDropdownScripts(cascadingId, parameter, ddlSelector, url, dataTextField, dataValueField) {
    //Countries
    var ddl = $(ddlSelector);
    ddl.kendoDropDownList
        (
            {
                autoBind: false,
                cascadeFrom: cascadingId,
                value: ddl.data("value"),
                dataTextField: dataTextField,
                dataValueField: dataValueField,
                dataSource:
                {
                    serverFiltering: true,
                    transport:
                    {
                        read: url,
                        parameterMap: function (data, type) {
                            var values = eval('({ "' + parameter + '": ' + data.filter.filters[0].value + ' })');

                            return values;
                        }
                    }
                }
            }
        );
}

function LoadDropdownScripts(ddlSelector, url, dataTextField, dataValueField, optionLabel) {
    var ddl = $(ddlSelector);
    if (ddl.data("kendoDropDownList")) {
        //ddl.data("kendoDropDownList").destroy();
        ddl.data("kendoDropDownList").dataSource.transport.options.read.url = url;
        ddl.data("kendoDropDownList").dataSource.read();
    }
    else {
        ddl.kendoDropDownList
            (
                {
                    value: ddl.data("value"),
                    dataTextField: dataTextField,
                    dataValueField: dataValueField,
                    optionLabel: optionLabel,
                    dataSource:
                    {
                        serverFiltering: true,
                        transport:
                        {
                            cache: false,
                            read: url
                        }
                    },
                    dataBound: function (e) {
                        //ddl.data("kendoDropDownList").select(0);
                    }
                }
            );
    }
}

function LoadDropdownScriptsWithDs(ddlSelector, datasource, dataTextField, dataValueField, optionLabel) {
    var ddl = $(ddlSelector);
    ddl.kendoDropDownList
        (
            {
                value: ddl.data("value"),
                dataTextField: dataTextField,
                dataValueField: dataValueField,
                optionLabel: optionLabel,
                dataSource: datasource,
                dataBound: function (e) {
                    //ddl.data("kendoDropDownList").select(0);
                }
            }
        );
}

function LoadDropdownEvents(ddlSelector, event, callback) {
    var ddl = $(ddlSelector);
    ddl.data("kendoDropDownList").unbind(event);
    ddl.bind(event, callback);
}

function errorMessage(title, message, closeCalback) {
    $("#error-window #error-message").html(message);
    if (!$("#error-window").data("kendoWindow")) {
        $("#error-window").kendoWindow
            ({
                title: title,
                visible: true,
                width: "430px",
                draggable: true,
                modal: true,
                resizable: false,
                close: function () {
                    if ($.isFunction(closeCalback)) {
                        closeCalback();
                    }
                }
            }).data("kendoWindow").center();
        $("#error-window").show();
    }
    else {
        if ($.isFunction(closeCalback)) {
            $("#information-window").data("kendoWindow").bind("close", closeCalback);
        }
        $("#error-window").data("kendoWindow").open();
        $("#error-window").data("kendoWindow").center();
    }
}

function informationMessage(title, message, closeCallback) {
    $("#information-window #information-message").html(message);
    if (!$("#information-window").data("kendoWindow")) {
        $("#information-window").kendoWindow
            ({
                title: title,
                visible: true,
                width: "430px",
                draggable: true,
                modal: true,
                resizable: false,
                close: function () {
                    if ($.isFunction(closeCallback)) {
                        closeCallback();
                    }
                }
            }).data("kendoWindow").center();
        $("#information-window").show();
    }
    else {
        if ($.isFunction(closeCallback)) {
            $("#information-window").data("kendoWindow").unbind("close");
            $("#information-window").data("kendoWindow").bind("close", closeCallback);
        }
        $("#information-window").data("kendoWindow").open();
        $("#information-window").data("kendoWindow").center();
    }
}

function confirmMessage(title, message, cancelCallback, acceptCallback) {
    $("#confirmation-window #confirm-title").html(title);
    $("#confirmation-window #confirm-message").html(message);
    $("#confirmation-window #confirm-cancel").unbind("click");
    $("#confirmation-window #confirm-cancel").click
        (
            function () {
                if ($.isFunction(cancelCallback)) {
                    cancelCallback();
                }
                $("#confirmation-window").data("kendoWindow").close();
            }
        );
    $("#confirmation-window #confirm-accept").unbind("click");
    $("#confirmation-window #confirm-accept").click
        (
            function () {
                if ($.isFunction(acceptCallback)) {
                    acceptCallback();
                }
                $("#confirmation-window").data("kendoWindow").close();
            }
        );

    if (!$("#confirmation-window").data("kendoWindow")) {
        $("#confirmation-window").kendoWindow
            ({
                title: title,
                visible: true,
                width: "430px",
                draggable: true,
                modal: true,
                resizable: false,
                actions: []
            }).data("kendoWindow").center();
        $("#confirmation-window").show();
    }
    else {
        $("#confirmation-window").data("kendoWindow").open();
        $("#confirmation-window").data("kendoWindow").center();
    }
}

function yesNoRadios() {
    $(".yes-no input[type='radio']").each
        (
            function () {
                $(this).hide();
            }
        );

    $(".yes-no .btn").click
        (
            function () {
                $(this).siblings(".btn").removeClass("btn-danger").addClass("btn-default");
                $(this).removeClass("btn-default").addClass("btn-danger");
                //$(this).find("input[type='radio']").click();
            }
        );
}

function yesNoRadiosSelector(selector) {
    $(selector + " input[type='radio']").each
        (
            function () {
                $(this).hide();
            }
        );

    $(selector + " .btn").click
        (
            function () {
                $(this).siblings(".btn").removeClass("btn-danger").addClass("btn-default");
                $(this).removeClass("btn-default").addClass("btn-danger");
                //$(this).find("input[type='radio']").click();
            }
        );
}

function btnCheckboxes() {
    $(".btn-cbx input[type='checkbox']").each
        (
            function () {
                $(this).hide();
            }
        );

    $(".btn-cbx .btn").click
        (
            function () {
                var cbx = $(this).find("input[type='checkbox']");
                if (cbx.is(":checked")) {
                    $(this).removeClass("btn-default").addClass("btn-danger");
                }
                else {
                    $(this).removeClass("btn-danger").addClass("btn-default");
                }
            }
        );
}

function btnCheckboxes(selector) {
    $(selector + " input[type='checkbox']").each
        (
            function () {
                $(this).hide();
            }
        );

    $(selector + " .btn").click
        (
            function () {
                var cbx = $(this).find("input[type='checkbox']");
                if (cbx.is(":checked")) {
                    $(this).removeClass("btn-default").addClass("btn-danger");
                }
                else {
                    $(this).removeClass("btn-danger").addClass("btn-default");
                }
            }
        );
}

//Re-set the model
function setCollection(model, collection) {
    model.splice(0, model.length);
    $.each(collection, function (index, value) {
        model.push(value);
    });
}

//Render a kendo template
function renderKendoTemplate(container, data, templateId) {
    var template = kendo.template($("#" + templateId).html());
    var result = template(data);
    container.append(result);
}

//Parse Json Date
function parseJsonDate(jsonDate) {
    if (!jsonDate)
        return "";

    //var offset = new Date().getTimezoneOffset() * 60000;
    var offset = 0;
    var parts = /\/Date\((-?\d+)([+-]\d{2})?(\d{2})?.*/.exec(jsonDate);

    if (parts[2] == undefined)
        parts[2] = 0;

    if (parts[3] == undefined)
        parts[3] = 0;

    return new Date(+parts[1] + offset + parts[2] * 3600000 + parts[3] * 60000);
}

//Parse Date
function parseDate(date) {
    if (!date)
        return "";

    return new Date(date);
}

//Capitalise first letter
function capitaliseFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

//Clone object
function cloneKendoObject(target, source) {
    return $.extend(true, {}, source);
}

//Init a kendo autocomplete
function initKendoAutocomplete(item, url, data, dataTextField, headerTemplate, template, minLength) {
    var autocomplete = item.kendoAutoComplete
        ({
            minLength: minLength,
            dataTextField: dataTextField,
            headerTemplate: headerTemplate,
            template: template,
            filter: "contains",
            dataSource:
            {
                transport:
                {
                    read:
                    {
                        type: "POST",
                        dataType: "json",
                        url: url,
                        data: data
                    }
                },
                serverFiltering: true
            }
        }).data("kendoAutoComplete");

    return autocomplete;
}

//Scroll to
function reservationScrollTo(element) {
    if (!$(element) || $(element).length == 0) {
        return;
    }
    $('html, body').animate({
        scrollTop: $(element).offset().top - 150
    }, 500);
}

//Scroll to
function scrollTo(element) {
    if (!$(element) || $(element).length == 0) {
        return;
    }
    $('html, body').animate({
        scrollTop: $(element).offset().top - 80
    }, 500);
}

//Date to utc
function convertDateToUTC(date) {
    return new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds(), date.getUTCMilliseconds());
}

//Hide column grid
function hideKGridColumn(grid, index) {
    $('#' + grid + ' .k-header').eq(index).hide();
    $('#' + grid + ' .k-grid-header colgroup col').eq(index).remove();
    $('#' + grid + ' .k-grid-content colgroup col').eq(index).remove();
    $('#' + grid + ' .k-grid-content tr').each
        (
            function () {
                $(this).find('td').eq(index).hide();
            }
        );
}

//Show column grid
function showKGridColumn(grid, index) {
    $('#' + grid + ' .k-header').eq(index).show();
    $('#' + grid + ' .k-grid-content tr').each
        (
            function () {
                $(this).find('td').eq(index).show();
            }
        );
    $('#' + grid + ' .k-grid-header colgroup').append('<col></col>');
    $('#' + grid + ' .k-grid-content colgroup').append('<col></col>');
}

//Index of obj
function objectIndexOf(array, searchTerm, property) {
    for (var i = 0, len = array.length; i < len; i++) {
        if (array[i][property] === searchTerm) return i;
    }
    return -1;
}

//Compare 2 object arrays and return true if both are equal no mather the order
function areObjectArraysEqual(array1, array2, property) {
    var temp = new Array();
    if ((!array1[0]) || (!array2[0])) {
        // If either is not an array
        return false;
    }
    if (array1.length != array2.length) {
        return false;
    }
    // Put all the elements from array1 into a "tagged" array
    for (var i = 0; i < array1.length; i++) {
        var key = (typeof array1[i]) + "~" + array1[i][property];
        // Use "typeof" so a number 1 isn't equal to a string "1".
        if (temp[key]) {
            temp[key]++;
        }
        else {
            temp[key] = 1;
        }
        // temp[key] = # of occurrences of the value (so an element could appear multiple times)
    }

    // Go through array2 - if same tag missing in "tagged" array, not equal
    for (var j = 0; j < array2.length; j++) {
        key = (typeof array2[j]) + "~" + array2[j][property];
        if (temp[key]) {
            if (temp[key] == 0) {
                return false;
            }
            else {
                temp[key]--;
            }
            // Subtract to keep track of # of appearances in array2
        }
        else {
            // Key didn't appear in array1, arrays are not equal.
            return false;
        }
    }
    // If we get to this point, then every generated key in array1 showed up the exact same
    // number of times in array2, so the arrays are equal.
    return true;
}

//Generates a new GUID
function newGuid() {
    try {
        var guid = generateGuidPart() + generateGuidPart() + "-" + generateGuidPart() + "-" + generateGuidPart() + "-" + generateGuidPart() + "-" + generateGuidPart() + generateGuidPart() + generateGuidPart();
        return validateGuid(guid);
    }
    catch (e) {
        var g = "";
        for (var i = 0; i < 32; i++) {
            g += Math.floor(Math.random() * 0xF).toString(0xF) + (i == 8 || i == 12 || i == 16 || i == 20 ? "-" : "");
        }

        return validateGuid(g);
    }
}

//Generates a GUID part: 4 chars
function generateGuidPart() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}

//Validates a GUID
function validateGuid(guid) {
    var exp = /^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$/;
    if (exp.test(guid)) {
        return guid;
    }
    else {
        return newGuid();
    }
}