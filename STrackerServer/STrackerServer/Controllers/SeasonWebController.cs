using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using STrackerServer.BusinessLayer.Core;
using STrackerServer.DataAccessLayer.DomainEntities;
using STrackerServer.Models.TvShow;

namespace STrackerServer.Controllers
{
    public class SeasonWebController : Controller
    {
        private readonly ITvShowsOperations showOps;
        private readonly ISeasonsOperations seasonOps;

        public SeasonWebController(ITvShowsOperations showOps, ISeasonsOperations seasonOps)
        {
            this.showOps = showOps;
            this.seasonOps = seasonOps;
        }

        [HttpGet]
        public ActionResult Index(string tvshowId)
        {
            var tvshow = this.showOps.Read(tvshowId);

            if(tvshow == null)
            {
                Response.StatusCode = (int) HttpStatusCode.NotFound;
                return View("Error");
            }

            var info = new TvShowEpisodesInfo
                           {
                               TvShowId = tvshowId,
                               NumberOfSeasons = tvshow.SeasonSynopses.Count
                           };

            foreach (var season in tvshow.SeasonSynopses.OrderBy(s => s.SeasonNumber))
            {
                info.List.Add(seasonOps.Read(new Tuple<string, int>(tvshowId,season.SeasonNumber)).EpisodeSynopses);
            }


            return this.View(info);
        }

        [HttpGet]
        public ActionResult Show(string tvshowId, int seasonId)
        {
            return this.View();
        }
    }
}
