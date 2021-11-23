using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AMDB.Models;
using AMDB.Data;
using AMDB.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AMDB.Controllers
{
    public class HomeController : IdentityControllerBase
    {
        public HomeController()
        {
            dbAccess.SeedGenres();
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        
        [Route("search")]
        public async Task<IActionResult> Search(string searchString)
        {
            SearchResultVM result;
            if (!string.IsNullOrEmpty(searchString))
            {
                result = await dbAccess.SearchAsync(searchString);
            }
            else
            {
                result = new SearchResultVM
                {
                    SearchString = "", 
                    Movies = new List<Production>(),
                    TVShows = new List<Production>(),
                    People = new List<Person>()
                };
            }
            return View(result);
        }
    }
}
