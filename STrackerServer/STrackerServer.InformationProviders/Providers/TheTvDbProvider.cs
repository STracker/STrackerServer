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
        /// The XML document.
        /// </summary>
        private readonly XmlDocument xdoc;

        /// <summary>
        /// Verification if the XML document is loaded.
        /// </summary>
        private bool xdocLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheTvDbProvider"/> class.
        /// </summary>
        public TheTvDbProvider()
        {
            this.apiKey = ConfigurationManager.AppSettings["TvDbAPI"];
            this.mirrorPath = ConfigurationManager.AppSettings["TvDbMirrorPath"];

            this.xdoc = new XmlDocument();
            this.xdocLoaded = false;
        }

        /// <summary>
        /// The verify if exists by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string VerifyIfExistsByName(string name)
        {
            var url = string.Format("{0}/api/GetSeries.php?seriesname={1}", this.mirrorPath, name);
            var xdoc2 = new XmlDocument();
            xdoc2.Load(new XmlTextReader(url));

            var seriesNode = xdoc2.SelectNodes("//Series");

            // In this phase, STracker only accept requests for total name of the television show.
            if (seriesNode == null)
            {
                return null;
            }

            for (var i = 0; i < seriesNode.Count; i++)
            {
                var xmlNode = seriesNode.Item(i);
                if (xmlNode == null)
                {
                    continue;
                }

                var nodeName = xdoc2.SelectSingleNode("//SeriesName");
                if (nodeName != null && nodeName.LastChild != null && nodeName.LastChild.Value.Equals(name))
                {
                    var imdbIdNode = xdoc2.SelectSingleNode("//IMDB_ID");
                    return (imdbIdNode != null && imdbIdNode.LastChild != null) ? imdbIdNode.LastChild.Value : null;
                }
            }

            var imdbIdNoderet = xdoc2.SelectSingleNode("//IMDB_ID");
            return (imdbIdNoderet != null && imdbIdNoderet.LastChild != null) ? imdbIdNoderet.LastChild.Value : null;
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
            
            this.LoadXmlDocument(id);

            var tvshow = new TvShow(imdbId);
            
            // Get the basic information.
            var nameNode = this.xdoc.SelectSingleNode("//SeriesName");
            tvshow.Name = (nameNode != null && nameNode.LastChild != null) ? nameNode.LastChild.Value : null;

            var descrNode = this.xdoc.SelectSingleNode("//Overview");
            tvshow.Description = (descrNode != null && descrNode.LastChild != null) ? descrNode.LastChild.Value : null;

            var firstAiredNode = this.xdoc.SelectSingleNode("//FirstAired");
            tvshow.FirstAired = (firstAiredNode != null && firstAiredNode.LastChild != null) ? firstAiredNode.LastChild.Value : null;

            var airDayNode = this.xdoc.SelectSingleNode("//Airs_DayOfWeek");
            tvshow.AirDay = (airDayNode != null && airDayNode.LastChild != null) ? airDayNode.LastChild.Value : null;

            var runtimeNode = this.xdoc.SelectSingleNode("//Runtime");
            tvshow.Runtime = (runtimeNode != null && runtimeNode.LastChild != null) ? int.Parse(runtimeNode.LastChild.Value) : 0;

            var posterImageNode = this.xdoc.SelectSingleNode("//poster");
            if (posterImageNode != null && posterImageNode.LastChild != null)
            {
                tvshow.Artworks.Add(
                    new Artwork
                        {
                            ImageUrl = string.Format("{0}/banners/{1}", this.mirrorPath, posterImageNode.LastChild.Value)
                        });
            }

            this.GetActors(id, ref tvshow);
            return tvshow;
        }

        /// <summary>
        /// Get seasons information.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB Id.
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

            this.LoadXmlDocument(id);

            var seasonsNodes = this.xdoc.SelectNodes("//SeasonNumber");
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
        /// The IMDB Id.
        /// </param>
        /// <param name="seasonNumber">
        /// The season Number.
        /// </param>
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

            this.LoadXmlDocument(id);

            var episodesNodes = this.xdoc.SelectNodes(string.Format("//Episode[SeasonNumber={0}]", seasonNumber));
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

                var episodeNumberNode = xmlNode.SelectSingleNode("EpisodeNumber");
                if (episodeNumberNode == null)
                {
                    continue;
                }

                var episode = new Episode(new Tuple<string, int, int>(imdbId, seasonNumber, int.Parse(episodeNumberNode.LastChild.Value)));

                var nameNode = xmlNode.SelectSingleNode("EpisodeName");

                episode.Name = (nameNode != null && nameNode.LastChild != null) ? nameNode.LastChild.Value : null;

                var descriptionNode = xmlNode.SelectSingleNode("Overview");

                episode.Description = (descriptionNode != null && descriptionNode.LastChild != null) ? descriptionNode.LastChild.Value : null;

                var directorNode = xmlNode.SelectSingleNode("Director");

                episode.Directors = new List<Person>();

                string[] directors = (directorNode != null && directorNode.LastChild != null) ? directorNode.LastChild.Value.Split('|') : new string[0];

                foreach (var director in directors)
                {
                    if (director != string.Empty)
                    {
                        episode.Directors.Add(new Person
                        {
                            Name = director.Trim()
                        });
                    }
                }

                var guestsNode = xmlNode.SelectSingleNode("GuestStars");

                episode.GuestActors = new List<Actor>();

                string[] guests = (guestsNode != null && guestsNode.LastChild != null) ? guestsNode.LastChild.Value.Split('|') : new string[0];

                foreach (var guest in guests)
                {
                    if (guest != string.Empty)
                    {
                        episode.GuestActors.Add(new Actor
                        {
                            Name = guest.Trim()
                        });
                    }
                }

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
        /// Auxiliary method for get the actors.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="tvshow">
        /// The television show.
        /// </param>
        private void GetActors(string id, ref TvShow tvshow)
        {
            var xdocActors = new XmlDocument();
            var url = string.Format("{0}/api/{1}/series/{2}/actors.xml", this.mirrorPath, this.apiKey, id);
            xdocActors.Load(new XmlTextReader(url));

            var actorsNodes = xdocActors.SelectNodes("//Actor");
            if (actorsNodes == null)
            {
                return;
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

                var nodeName = xmlNode.SelectSingleNode("Name");
                actor.Name = (nodeName != null && nodeName.LastChild != null) ? nodeName.LastChild.Value : null;

                var characterNameNode = xmlNode.SelectSingleNode("Role");
                actor.CharacterName = (characterNameNode != null && characterNameNode.LastChild != null) ? characterNameNode.LastChild.Value : null;

                var imageNode = xmlNode.SelectSingleNode("Image");
                if (imageNode != null && imageNode.LastChild != null)
                {
                    actor.Photo = new Artwork { ImageUrl = string.Format("{0}/banners/{1}", this.mirrorPath, imageNode.LastChild.Value) };
                }

                tvshow.Actors.Add(actor);
            }
        }

        /// <summary>
        /// Auxiliary method for load xml document.
        /// </summary>
        /// <param name="id">
        /// The Id.
        /// </param>
        private void LoadXmlDocument(string id)
        {
            if (this.xdocLoaded)
            {
                return;
            }

            var url = string.Format("{0}/api/{1}/series/{2}/all", this.mirrorPath, this.apiKey, id);
            this.xdoc.Load(new XmlTextReader(url));
            this.xdocLoaded = true;
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
            var url = string.Format("{0}/api/GetSeriesByRemoteID.php?imdbid={1}", this.mirrorPath, imdbId);
            var xdocId = new XmlDocument();
            xdocId.Load(new XmlTextReader(url));

            var seriesIdNode = xdocId.SelectSingleNode("//seriesid");

            return seriesIdNode != null ? seriesIdNode.LastChild.Value : null;
        }
    }
}