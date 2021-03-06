﻿(function () {
    window.addEventListener('DOMContentLoaded', function () {
        setup();
    });

    var setup = function () {
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
                setTimeout(removeTooltip(inputs[i]), 1000);
            }
        }

        for (var j = 0; j < textareas.length; j++) {
            if (!validateInput(textareas[j])) {
                event.preventDefault();
                $(textareas[j]).tooltip('show');
                setTimeout(removeTooltip(textareas[j]), 1000);
            }
        }
    };

    var removeTooltip = function(node) {
        return function () {
            $(node).tooltip('destroy');
            $('.tooltip').remove();
        };
    };

    var validateInput = function (input) {

        if (!input.getAttribute('data-val')) {
            return true;
        }

        var attributes = input.attributes;

        for (var i = 0; i < attributes.length; i++) {

            var validationFunction = validationFunctions[attributes[i].name];

            if (validationFunction && !validationFunction(input)) {
                $(input).tooltip({
                    placement: 'bottom',
                    trigger: 'manual',
                    container: 'li',
                    title: input.getAttribute(attributes[i].name)
                });

                return false;
            }
        }

        return true;
    };

    var validationFunctions = {};

    validationFunctions['data-val-required'] = function (input) {
        return input.value.trim();
    };

    validationFunctions['data-val-range'] = function (input) {

        var min = input.getAttribute('data-val-range-min');
        var max = input.getAttribute('data-val-range-max');

        if (isNaN(input.value)) {
            return false;
        }

        if (min && input.value < min) {
            return false;
        }

        if (max && input.value > max) {
            return false;
        }

        return true;
    };

    validationFunctions['data-val-number'] = function (input) {
        return !isNaN(input.value);
    };
})()