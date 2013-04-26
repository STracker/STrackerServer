// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TheTvDbProvider.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of the TVDB provider.
//  More info at http://thetvdb.com/ and http://thetvdb.com/wiki/index.php?title=Programmers_API
//  The TVDB send the information only in XML format.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.InformationProviders.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Xml;

    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.InformationProviders.Core;

    /// <summary>
    /// The TVDB provider.
    /// </summary>
    public class TheTvDbProvider : ITvShowsInformationProvider
    {
        /// <summary>
        /// The mirror path.
        /// </summary>
        private readonly string mirrorPath;

        /// <summary>
        /// The API key.
        /// </summary>
        private readonly string apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheTvDbProvider"/> class.
        /// </summary>
        public TheTvDbProvider()
        {
            this.apiKey = ConfigurationManager.AppSettings["TvDbAPI"];
            this.mirrorPath = ConfigurationManager.AppSettings["TvDbMirrorPath"];
        }

        /// <summary>
        /// Get a television show object with information filled.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        public TvShow GetTvShowInformation(string imdbId)
        {
            // Get thetvdb id first.
            var id = this.GetTheTvDbId(imdbId);
            if (id == null)
            {
                return null;
            }

            var xdoc = CreateXmlDocument(string.Format("{0}/api/{1}/series/{2}", this.mirrorPath, this.apiKey, id));
            var tvshow = new TvShow(imdbId);
            
            // Get the basic information.
            var nameNode = xdoc.SelectSingleNode("//SeriesName");
            tvshow.Name = (nameNode != null) ? nameNode.LastChild.Value : null;

            var descrNode = xdoc.SelectSingleNode("//Overview");
            tvshow.Description = (descrNode != null) ? descrNode.LastChild.Value : null;

            var firstAiredNode = xdoc.SelectSingleNode("//FirstAired");
            tvshow.FirstAired = (firstAiredNode != null) ? firstAiredNode.LastChild.Value : null;

            var airDayNode = xdoc.SelectSingleNode("//Airs_DayOfWeek");
            tvshow.AirDay = (airDayNode != null) ? airDayNode.LastChild.Value : null;

            var runtimeNode = xdoc.SelectSingleNode("//Runtime");
            tvshow.Runtime = (runtimeNode != null) ? int.Parse(runtimeNode.LastChild.Value) : 0;
            
            // Get the actors.
            xdoc = CreateXmlDocument(string.Format("{0}/api/{1}/series/{2}/actors.xml", this.mirrorPath, this.apiKey, id));

            var actorsNodes = xdoc.SelectNodes("//Actor");
            if (actorsNodes == null)
            {
                return tvshow;
            }

            for (var i = 0; i < actorsNodes.Count; i++)
            {
                var xmlNode = actorsNodes.Item(i);
                if (xmlNode == null)
                {
                    continue;
                }

                // In this fase of the project, the actor dont need the key.
                var actor = new Actor();

                var nodeName = xmlNode.SelectSingleNode("//Name");
                actor.Name = (nodeName != null) ? nodeName.LastChild.Value : null;

                var characterNameNode = xmlNode.SelectSingleNode("//Role");
                actor.CharacterName = (characterNameNode != null) ? characterNameNode.LastChild.Value : null;

                tvshow.Actors.Add(actor);
            }

            return tvshow;
        }

        /// <summary>
        /// Get seasons information.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<Season> GetSeasonsInformation(string imdbId)
        {
            // Get thetvdb id first.
            var id = this.GetTheTvDbId(imdbId);
            if (id == null)
            {
                return null;
            }

            var xdoc = CreateXmlDocument(string.Format("{0}/api/{1}/series/{2}/all", this.mirrorPath, this.apiKey, id));

            var seasonsNodes = xdoc.SelectNodes("//SeasonNumber");
            if (seasonsNodes == null)
            {
                return null;
            }

            var numbers = new HashSet<int>();
            for (var i = 0; i < seasonsNodes.Count; i++)
            {
                var xmlNode = seasonsNodes.Item(i);
                if (xmlNode == null)
                {
                    continue;
                }

                numbers.Add(int.Parse(xmlNode.LastChild.Value));
            }

            var list = new List<Season>();
            var enumerator = numbers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                list.Add(new Season(new Tuple<string, int>(imdbId, enumerator.Current)));
            }
             
            return list;
        }

        /// <summary>
        /// Get episodes information.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season number.
        ///  </param>
        /// <returns>
        /// The <see>
        ///       <cref>IEnumerable</cref>
        ///     </see> .
        /// </returns>
        public IEnumerable<Episode> GetEpisodesInformation(string imdbId, int seasonNumber)
        {
            // Get thetvdb id first.
            var id = this.GetTheTvDbId(imdbId);
            if (id == null)
            {
                return null;
            }

            // Get the basic information.
            var url = string.Format("{0}/api/{1}/series/{2}/all", this.mirrorPath, this.apiKey, id);
            var xdoc = new XmlDocument();
            xdoc.Load(new XmlTextReader(url));

            var episodesNodes = xdoc.SelectNodes("//Episode");
            if (episodesNodes == null)
            {
                return null;
            }

            var list = new List<Episode>();

            for (var i = 0; i < episodesNodes.Count; i++)
            {
                var xmlNode = episodesNodes.Item(i);
                if (xmlNode == null)
                {
                    continue;
                }

                var seasonNumberNode = xmlNode.SelectSingleNode("//SeasonNumber");
                var episodeNumberNode = xmlNode.SelectSingleNode("//EpisodeNumber");
                if (seasonNumberNode == null || episodeNumberNode == null)
                {
                    continue;
                }

                var episode = new Episode(new Tuple<string, int, int>(imdbId, int.Parse(seasonNumberNode.LastChild.Value), int.Parse(episodeNumberNode.LastChild.Value)));

                var nameNode = xmlNode.SelectSingleNode("//EpisodeName");
                episode.Name = (nameNode != null) ? nameNode.LastChild.Value : null;

                list.Add(episode);
            }

            return list;
        }

        /// <summary>
        /// The verify if exists.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool VerifyIfExists(string imdbId)
        {
            return this.GetTheTvDbId(imdbId) != null;
        }

        /// <summary>
        /// Auxiliary method for create xml document.
        /// </summary>
        /// <param name="url">
        /// The url.
        /// </param>
        /// <returns>
        /// The <see cref="XmlDocument"/>.
        /// </returns>
        private static XmlDocument CreateXmlDocument(string url)
        {
            var xdoc = new XmlDocument();
            xdoc.Load(new XmlTextReader(url));

            return xdoc;
        }

        /// <summary>
        /// Auxiliary method for getting the TVDB id.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetTheTvDbId(string imdbId)
        {
            var xdoc = CreateXmlDocument(string.Format("{0}/api/GetSeriesByRemoteID.php?imdbid={1}", this.mirrorPath, imdbId));
            var seriesIdNode = xdoc.SelectSingleNode("//seriesid");

            return seriesIdNode != null ? seriesIdNode.LastChild.Value : null;
        }
    }
}