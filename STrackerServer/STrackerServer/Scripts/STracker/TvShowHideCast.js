(function () {
    window.addEventListener('DOMContentLoaded', function () {
        //setupTvShowCastDisplay();
    });

    var buttonOpen;
    var buttonClose;

    var displayCastButton;
    var displayCastTable;
    
    var setupTvShowCastDisplay = function () {

        displayCastButton = document.getElementById('cast-button');
        displayCastTable = document.getElementById('cast-table');
        buttonOpen = generateButton('(Open)', eventOpen);
        buttonClose = generateButton('(Close)', eventClose);
        
        displayCastTable.className = 'hide table table-condensed';

        setDisplayCastButton(buttonOpen);
    };

    var generateButton = function(text, clickEvent) {
        var button = document.createElement('button');
        button.className = 'btn-link';
        button.appendChild(document.createTextNode(text));
        button.addEventListener('click', clickEvent);
        return button;
    };

    var eventOpen = function(event) {
        event.preventDefault();
        displayCastTable.className = 'table table-condensed';
        setDisplayCastButton(buttonClose);
    };
    
    var eventClose = function (event) {
        event.preventDefault();
        displayCastTable.className = 'hide table table-condensed';
        setDisplayCastButton(buttonOpen);
    };

    var setDisplayCastButton = function(button) {
        var children = displayCastButton.childNodes;
        var count = children.length;

        for (var i = 0; i < count; i++) {
            displayCastButton.removeChild(children[0]);
        }
        displayCastButton.appendChild(button);
    };
})()