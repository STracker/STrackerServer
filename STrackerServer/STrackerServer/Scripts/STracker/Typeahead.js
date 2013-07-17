(function () {
    window.addEventListener("DOMContentLoaded", function () {
        setupTypeahead();
    });
    
    var setupTypeahead = function () {
        $('#TvShowSearch').typeahead({
            items: 5,
            source: function (query, process) {

                var link = '/TvShows/Names?query=' + query;
                var xhr = new XMLHttpRequest();

                xhr.open('GET', link, true);

                xhr.onreadystatechange = function () {
                    if (xhr.status === 200 && xhr.readyState === XMLHttpRequest.DONE) {
                        process(jQuery.parseJSON(xhr.responseText));
                    }
                };
                xhr.send();
            },
        });
    };
})()