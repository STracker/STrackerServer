(function () {
    window.addEventListener('DOMContentLoaded', function () {
        setup();
    });

    var sectionNode;

    var setup = function () {
        
        sectionNode = document.getElementById('similar-tvshows');
        var tvshowIdNode = document.getElementById('tvshowId');
        var genres = document.getElementsByClassName('genre');
        
        if (!sectionNode || !tvshowIdNode || genres.length == 0) {
            return;
        }

        var genresQuery = 'genres=' + genres[0].innerHTML;

        for (var i = 1; i < genres.length; i++) {
            genresQuery = genresQuery + '&genres=' + genres[i].innerHTML;
        }

        var link = '/Genres/Search?' + genresQuery + '&excludeTvShow=' + tvshowIdNode.innerHTML;
        
        var xhr = new XMLHttpRequest();

        xhr.open('GET', link, true);

        xhr.onreadystatechange = function () {
            if (xhr.status === 200 && xhr.readyState === XMLHttpRequest.DONE) {
                sectionNode.innerHTML = xhr.responseText;
            }
        };
        xhr.send();
    };
})()