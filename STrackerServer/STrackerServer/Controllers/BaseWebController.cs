using System.Net;
using STrackerServer.BusinessLayer.Core;

namespace STrackerServer.Controllers
{
    using System.Web.Mvc;

    public abstract class BaseWebController : Controller
    {
        protected ActionResult GetView(OperationResultState state)
        {
            switch (state)
            {
                case OperationResultState.InProcess:
                    Response.StatusCode = (int)HttpStatusCode.Accepted;
                    return this.View("Error", Response.StatusCode);

                case OperationResultState.NotFound:
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return this.View("Error", Response.StatusCode);

                default:
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return this.View("Error", Response.StatusCode);
            }  
        }
    }
}
