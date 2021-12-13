/*
         Febrero 2017
         John Nelson Rodriguez.
         https://stephanwagner.me/only-css-loading-spinner
         https://stephanwagner.me/only-css-loading-spinner
         https://stephanwagner.me/loading-spinner-with-animation
         http://materializecss.com/preloader.html
         http://www.dotnetcurry.com/jquery/1104/jquery-submit-form-using-ajax
         http://stackoverflow.com/questions/16670209/download-excel-file-via-ajax-mvc
*/
jQuery.extend({
    addSpinner: function (el, static_pos) {
        var spinner = el.children('.spinner');
        if (spinner.length && !spinner.hasClass('spinner-remove')) return null;
        !spinner.length && (spinner = $('<div class="spinner' + (static_pos ? '' : ' spinner-absolute') + '"/>').appendTo(el));
        $.animateSpinner(spinner, 'add');
    },

    removeSpinner: function (el, complete) {
        var spinner = el.children('.spinner');
        spinner.length && $.animateSpinner(spinner, 'remove', complete);
    },

    animateSpinner: function (el, animation, complete) {
        if (el.data('animating')) {
            el.removeClass(el.data('animating')).data('animating', null);
            el.data('animationTimeout') && clearTimeout(el.data('animationTimeout'));
        }
        el.addClass('spinner-' + animation).data('animating', 'spinner-' + animation);
        el.data('animationTimeout', setTimeout(function () { animation == 'remove' && el.remove(); complete && complete(); }, parseFloat(el.css('animation-duration')) * 1000));
    }
});