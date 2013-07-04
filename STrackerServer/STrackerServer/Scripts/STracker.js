(function () {
    window.addEventListener("DOMContentLoaded", function () {

        $('#TvShowSearch').typeahead({
            items: 5,
            source: function (query, process) {
                return $.get('TvShows/Names', { query: query }, function (data) {
                    return process(data);
                });
            },
        });
    });
})()