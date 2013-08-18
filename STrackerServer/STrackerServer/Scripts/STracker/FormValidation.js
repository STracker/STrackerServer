(function () {
    window.addEventListener('DOMContentLoaded', function () {
        setup();
    });

    var setup = function() {
        var forms = document.getElementsByTagName('form');
        for (var i = 0; i < forms.length; i++) {
            forms[i].addEventListener('submit', validationHandler);
        }
    };

    var validationHandler = function (event) {

        var inputs = event.target.getElementsByTagName('input');
        var textareas = event.target.getElementsByTagName('textarea');

        for (var i = 0; i < inputs.length; i++) {

            if (!validateInput(inputs[i])) {
                event.preventDefault();
                $(inputs[i]).tooltip('show');
            }
        }
        
        for (var j = 0; j < textareas.length; j++) {

            if (!validateInput(textareas[j])) {
                event.preventDefault();
                $(textareas[j]).tooltip('show');
            }
        }
        
        setTimeout(function () {
            $('[data-val]').tooltip('hide');
        }, 1000);
    };

    var validateInput = function (input) {

        if (!input.getAttribute('data-val')) {
            return true;
        }

        var isValid = true;
        var messages = [];
        var attributes = input.attributes;

        for (var i = 0; i < attributes.length; i++) {

            var validationFunction = validationFunctions[attributes[i].name];

            if (validationFunction && !validationFunction(input)) {
                isValid = false;
                messages[messages.length] = input.getAttribute(attributes[i].name);
            }
        }

        var titleText = '';
        
        for (var idx = 0; idx < messages.length; idx++) {
            titleText = titleText + '\n' + messages[idx];
        }

        $(input).tooltip({
            placement: 'bottom',
            trigger: 'manual',
            container: 'body',
            title: titleText
        });


        return isValid;
    };

    var validationFunctions = {};

    validationFunctions['data-val-required'] = function (input) {
        return input.value.trim();
    };
})()