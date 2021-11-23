using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AMDB.Models;
using AMDB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AMDB.Controllers
{
    public class UserController : IdentityControllerBase
    {
        [Route("register")]
        public IActionResult RegisterUser()
        {
            var model = new RegisterUserVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserVM model)
        {
            if (ModelState.IsValid)
            {
                User user = await dbAccess.RegisterUserAsync(model);
                if (user != null)
                {
                    return RedirectToAction("SignInUser", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Registration failed, please retry");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Route("signin")]
        public IActionResult SignInUser()
        {
            var model = new SignInUserVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("signin")]
        public async Task<IActionResult> SignInUser(SignInUserVM model)
        {
            if (ModelState.IsValid)
            {
                User user = await dbAccess.SignInUserAsync(model);
                if (user != null)
                {
                    CurrentUser = new UserVM { UserId = user.UserId, FullName = user.FullName };
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Sign in failed, please retry");
            return View(model);
        }

        [Route("signout")]
        public async Task<IActionResult> SignOutUser()
        {
            await dbAccess.SignOutUserAsync(CurrentUser.UserId);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RateProduction(UserRatingVM model)
        {
            var prod = await dbAccess.CreateUserRating(model);
            return RedirectToAction(prod is Movie ? "Movie" : "TVShow", prod is Movie ? "Movie" : "TVShow", new { id = prod.ProductionId, title = prod.Title.ToLower().Replace(" ", "-") });
        }
    }
}