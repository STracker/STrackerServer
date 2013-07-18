(function () {
    window.addEventListener("DOMContentLoaded", function () {
        setupCreateCommentModal();
    });

    var setupCreateCommentModal = function () {
        var createButton = document.getElementById('create-comment');
        createButton.removeAttribute('href');
        createButton.setAttribute('href', '#myModal');
    };
})()