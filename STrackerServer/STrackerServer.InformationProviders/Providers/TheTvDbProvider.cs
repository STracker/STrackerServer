// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TheTvDbProvider.cs" company="STracker">
//  Copyright (c) STracker Developers. All rights reserved.
// </copyright>
// <summary>
//  Implementation of the TVDB provider.
//  More info at http://thetvdb.com/ and http://thetvdb.com/wiki/index.php?title=Programmers_API
// The TVDB send the information only in XML format.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace STrackerServer.InformationProviders.Providers
{
    using System.Xml;

    using STrackerServer.DataAccessLayer.DomainEntities;
    using STrackerServer.InformationProviders.Core;

    /// <summary>
    /// The TVDB provider.
    /// </summary>
    public class TheTvDbProvider : IInformationProvider
    {
        /// <summary>
        /// URL for getting information from one television show by IMDB id.
        /// </summary>
        private const string InfoTvUrl = "http://thetvdb.com/api/GetSeriesByRemoteID.php?imdbid=";

        /// <summary>
        /// Get a television show object with information filled.
        /// </summary>
        /// <param name="imdbId">
        /// The IMDB id.
        /// </param>
        /// <returns>
        /// The <see cref="TvShow"/>.
        /// </returns>
        /// Uses xpath for querys.
        public TvShow GetTvShowInformationByImdbId(string imdbId)
        {
            var url = string.Format("{0}{1}", InfoTvUrl, imdbId);

            var xdoc = new XmlDocument();
            xdoc.Load(new XmlTextReader(url));

            var seriesNode = xdoc.SelectSingleNode("//Series");
            if (seriesNode == null)
            {
                return null;
            }

            var tvshow = new TvShow(imdbId);

            var nameNode = xdoc.SelectSingleNode("//SeriesName");
            if (nameNode != null)
            {
                tvshow.Name = nameNode.LastChild.Value;
            }

            var descrNode = xdoc.SelectSingleNode("//Overview");
            if (descrNode != null)
            {
                tvshow.Description = descrNode.LastChild.Value;
            }

            return tvshow;
        }
    }
}
