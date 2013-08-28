(function () {
    window.addEventListener('DOMContentLoaded', function () {
        setup();
    });

    var setup = function () {

        var stars = document.getElementsByClassName('star');

        var rateLink = document.getElementById('rating-link');
        rateLink.parentNode.removeChild(rateLink);

        for (var i = 0; i < stars.length; i++) {
            starSetup(stars[i], stars);
        }
    };
    
    var starSelectedImage = '/Content/images/star_red.png';

    var starSetup = function(star, stars) {
        star.oldSrc = star.src;
        
        star.addEventListener('mouseover', function (event) {
            changeStars(stars, event.target, starSelectedImage);
        });

        star.addEventListener('mouseout', function (event) {
            changeStars(stars, event.target);
        });
        
        star.addEventListener('click', function (event) {
            sendRating(event.target.getAttribute('data-star-value'));
        });
    };

    var changeStars = function (stars, selectedStar, starImage) {
        for (var i = 0; i < selectedStar.getAttribute('data-star-value'); i++) {
            
            if (starImage) {
                stars[i].src = starImage;
            } else {
                stars[i].src = stars[i].oldSrc;
            }
        }
    };

    var sendRating = function (value) {
        
        var xhr = new XMLHttpRequest();
        xhr.open('POST', window.location.pathname + '/Rate', true);
        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
        
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE) {
                showResponse(xhr.status, value);
            }
        };
        
        xhr.send('value=' + value);
    };

    var showResponse = function (status) {

        var resultNode = document.getElementById('rating-result');

        var childNodes = resultNode.childNodes;
        var childNodesLength = resultNode.childNodes.length;

        for (var i = 0; i < childNodesLength; i++) {
            resultNode.removeChild(childNodes[0]);
        }
        
        resultNode.appendChild(responseNodes[status]);
    };

    var createResultNode = function (className, message) {
        var node = document.createElement('div');
        node.className = className;
        node.appendChild(document.createTextNode(message));
        return node;
    };

    var responseNodes = {
        0: createResultNode('error_box', 'You need to be logged in.'),
        200: createResultNode('success_box', 'Success'),
        400: createResultNode('error_box','You have made a bad request.'),
        401: createResultNode('error_box', 'Unauthorized'),
        500: createResultNode('error_box', 'An internal error has occurred.')
    };
})()