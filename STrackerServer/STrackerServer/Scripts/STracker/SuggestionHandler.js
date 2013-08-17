(function () {
    window.addEventListener('DOMContentLoaded', function () {
        setup();
    });
    
    var createResultNode = function(username, success) {
        var node = document.createElement('div');

        if (success) {
            node.className = 'success_box';
            node.appendChild(document.createTextNode(username + ' has received your suggestion.'));
        } else {
            node.className = 'error_box';
            node.appendChild(document.createTextNode('An error has occured when sending a suggestion to ' + username + '.'));
        }

        return node;
    };

    var setup = function () {

        var resultNode = document.getElementById('suggestion-result');

        if (!resultNode) {
            return;
        }

        var forms = document.getElementsByTagName('form');

        for (var i = 0; i < forms.length; i++) {
            if (forms[i].getAttribute('type') == 'suggest') {
                changeSubmitEvent(forms[i], resultNode);
            }
        }
    };

    var changeSubmitEvent = function (form, resultNode) {
        form.addEventListener('submit', function (event) {
            event.preventDefault();

            var childNodes = resultNode.childNodes;
            var childNodesLength = resultNode.childNodes.length;

            for (var i = 0; i < childNodesLength; i++) {
                resultNode.removeChild(childNodes[0]);
            }

            var tvshowId = resultNode.getAttribute('data-tvshow');

            var link = '/TvShows/' + tvshowId + '/Suggest';
            var xhr = new XMLHttpRequest();

            xhr.open('POST', link, true);
            xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            
            xhr.onreadystatechange = function () {
                if (xhr.readyState === XMLHttpRequest.DONE) {
                    resultNode.appendChild(createResultNode(form.getAttribute('data-username'), xhr.status === 200));
                }
            };
            xhr.send('friendId=' + form.getAttribute('data-id') + '&tvshowId=' + tvshowId);
        });
    };
})()