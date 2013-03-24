using System.Net;
using System.Web.Mvc;
using STrackerServer.Testing_Repository;

namespace STrackerServer.Controllers
{
    public class TvShowController : Controller
    {
        private readonly TvShowsTestRepository _repository = TestRepositoryLocator.TvShowsTestRepo;

        public JsonResult Show(int id)
        {
            var tvshow = _repository.Get(id);
            if (tvshow == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(tvshow, JsonRequestBehavior.AllowGet);
        }
    }
}
