(function () {
    window.addEventListener('DOMContentLoaded', function () {
        setup();
    });

    var invitesValueNode;
    var suggestionsValueNode;
    
    var setup = function () {
        invitesValueNode = document.getElementById('invites-value');
        var invitesNode = document.getElementById('invites');
        var invitesDividerNode = document.getElementById('invites-divider');
        invitesNode.className = '';
        invitesDividerNode.className = 'divider-vertical';
        
        suggestionsValueNode = document.getElementById('suggestions-value');
        var suggestionsNode = document.getElementById('suggestions');
        var suggestionsDividerNode = document.getElementById('suggestions-divider');
        suggestionsNode.className = '';
        suggestionsDividerNode.className = 'divider-vertical';

        updater();
        setInterval(updater, 30000);
    };

    var updater = function() {
        var link = '/User/Updater';
        var xhr = new XMLHttpRequest();

        xhr.open('GET', link, true);

        xhr.onreadystatechange = function() {
            if (xhr.status === 200 && xhr.readyState === XMLHttpRequest.DONE) {
                var response = jQuery.parseJSON(xhr.responseText);
                invitesValueNode.innerHTML = response.FriendRequests;
                suggestionsValueNode.innerHTML = response.Suggestions;
            }
        };
        xhr.send();
    };
})()