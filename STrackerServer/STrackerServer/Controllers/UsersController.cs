using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using STrackerServer.BusinessLayer.Core;

namespace STrackerServer.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private IUsersOperations usersOperations;

        public UsersController(IUsersOperations usersOperations)
        {
            this.usersOperations = usersOperations;
        }

        public ActionResult GetInfo()
        {
            var user = usersOperations.Read(User.Identity.Name);
            return View(user);
        }
    }
}
