using AMDB.Data;
using AMDB.Models;
using AMDB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Controllers
{
    public abstract class IdentityControllerBase : Controller
    {
        public readonly DatabaseAccess dbAccess;
        public UserVM CurrentUser { get; set; }

        protected IdentityControllerBase()
        {
            dbAccess = new DatabaseAccess();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = dbAccess.GetSignedInUser();
            if (user != null)
            {
                CurrentUser = new UserVM { UserId = user.UserId, FullName = user.FullName };
                ViewBag.SignedIn = true;
                ViewBag.UserId = CurrentUser.UserId;
                ViewBag.UserFullName = CurrentUser.FullName;
            }
            else
            {
                ViewBag.SignedIn = false;
                ViewBag.UserId = 0;
                ViewBag.UserFullName = "Anonymous";
            }
            base.OnActionExecuting(filterContext);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
