(function () {
    window.addEventListener("DOMContentLoaded", function () {
        setupInvitesUpdater();
    });

    var invitesValueNode;

    var setupInvitesUpdater = function () {
        invitesValueNode = document.getElementById('invites-value');
        var invitesNode = document.getElementById('invites');
        var invitesDividerNode = document.getElementById('invites-divider');
        invitesNode.className = '';
        invitesDividerNode.className = 'divider-vertical';
        updater();
        setInterval(updater, 2000);

    };

    var updater = function() {
        var link = '/User/FriendRequestsCount';
        var xhr = new XMLHttpRequest();

        xhr.open('GET', link, true);

        xhr.onreadystatechange = function() {
            if (xhr.status === 200 && xhr.readyState === XMLHttpRequest.DONE) {
                var response = jQuery.parseJSON(xhr.responseText);
                invitesValueNode.innerHTML = response.value;
            }
        };
        xhr.send();
    };
})()