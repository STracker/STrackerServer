(function () {
    window.addEventListener('DOMContentLoaded', function () {
        setupSeasonInfoDisplay();
    });

    var displayElement;
    var displayTable;
    
    var displayActive = function (active) {
        if (active) {
            displayElement.className = 'box';
        } else {
            displayElement.className = 'box hide';
        }
    };

    var setupSeasonInfoDisplay = function () {

        var seasons = document.getElementsByClassName('season');

        displayElement = document.getElementById("display-season");
        displayTable = document.getElementById('display-table');
        var displayHide = document.getElementById('display-hide');

        displayHide.addEventListener('click', function(event) {
            event.preventDefault();
            displayActive(false);
        });
        
        for (var idx = 0; idx < seasons.length; idx++) {
            addDisplayInfoEvent(seasons[idx], seasons[idx].attributes['data-api'].value);
        }
    };

    var addDisplayInfoEvent = function (element, link) {
        element.addEventListener('click', function (event) {
            event.preventDefault();

            var prevInfo = displayTable.childNodes;
            var length = prevInfo.length;
            
            for (var i = 0; i < length; i++) {
                displayTable.removeChild(prevInfo[0]);
            }

            var xhr = new XMLHttpRequest();

            xhr.open('GET', link, true);

            xhr.onreadystatechange = function () {
                if (xhr.status === 200 && xhr.readyState === XMLHttpRequest.DONE) {
                    displayActive(true);
                    displaySeason(jQuery.parseJSON(xhr.responseText));
                }
            };
            xhr.send();
        });
    };

    var displaySeason = function (season) {
        for (var i = 0; i < season.EpisodeSynopses.length; i++) {
            displayEpisode(season.EpisodeSynopses[i]);
        }
    };
    
    var displayEpisode = function (episode) {
        var episodeRow = document.createElement("tr");
        episodeRow.innerHTML = '<td>' + episode.EpisodeNumber + '</td><td> <a href="../' + episode.Uri + '">' + episode.Name + '</a></td>';
        displayTable.appendChild(episodeRow);
    };
})()