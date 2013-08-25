(function () {
    window.addEventListener('DOMContentLoaded', function () {
        setup();
    });

    var starSelectedImage = '/Content/images/star_red.png'; 

    var setup = function () {
        var stars = document.getElementsByClassName('star');

        for (var i = 0; i < stars.length; i++) {
            stars[i].oldSrc = stars[i].src;

            stars[i].addEventListener('mouseover', function (event) {
                changeStars(stars, event.target, starSelectedImage);
            });
            
            stars[i].addEventListener('mouseout', function (event) {
                changeStars(stars, event.target, event.target.oldSrc);
            });
        }
    };

    var changeStars = function(stars, selectedStar, starImage) {

    };
})()