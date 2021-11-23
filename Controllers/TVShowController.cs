using System;
using System.Collections.Generic;
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
    public class TVShowController : IdentityControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public TVShowController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        [Route("tvshows")]
        public async Task<IActionResult> TVShows()
        {
            var tvshows = await dbAccess.GetAllTVShowsAsync();
            return View(tvshows);
        }

        [Route("tvshow/{id}/{title}")]
        public async Task<IActionResult> TVShow(int id)
        {
            var tvshow = await dbAccess.GetTVShowAsync(id);
            tvshow.Suggestions = await dbAccess.GetProductionsOrderedByBestKeywordMatches(tvshow);
            return View(tvshow);
        }

        [Route("createtvshow")]
        public async Task<IActionResult> CreateTVShow()
        {
            var model = new TVShowVM
            {
                SelectedGenres = new List<int>(),
                AllGenres = new SelectList(await dbAccess.GetAllGenresAsync(), "GenreId", "Name"),
                SelectedStars = new List<int>(),
                AllPersons = new SelectList(await dbAccess.GetAllPersonsAsync(), "PersonId", "FullName"),
                SelectedKeywords = new List<string>(),
                AllKeywords = new SelectList(await dbAccess.GetAllKeywordNamesAsync()),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("createtvshow")]
        public async Task<IActionResult> CreateTVShow(TVShowVM model)
        {
            ModelState["EndDate"].ValidationState = ModelValidationState.Valid;
            if (model.SelectedKeywords != null)
                await dbAccess.CreateUniqueKeywordNamesAsync(model.SelectedKeywords.ToList());
            if (model.PosterFormFile != null)
            {
                if (Enum.GetNames(typeof(ImageTypes)).Select(n => n.Replace("_", "/")).Contains(model.PosterFormFile.ContentType))
                    model.PosterFileName = await FileUpload.ImageUploadAsync(webHostEnvironment, model.PosterFormFile);
                else
                    ModelState.AddModelError("PosterFormFile", "Please select a GIF, JPG or PNG image");
            }
            if (string.IsNullOrEmpty(model.PosterFileName))
                ModelState.AddModelError("PosterFormFile", "Please select a poster image");
            if (ModelState.IsValid)
            {
                int id = await dbAccess.CreateTVShowAsync(model);
                return RedirectToAction("TVShow", new { id, title = model.Title.ToLower().Replace(" ", "-") });
            }
            model.AllGenres = new SelectList(await dbAccess.GetAllGenresAsync(), "GenreId", "Name");
            model.AllPersons = new SelectList(await dbAccess.GetAllPersonsAsync(), "PersonId", "FullName");
            model.AllKeywords = new SelectList(await dbAccess.GetAllKeywordNamesAsync());
            return View(model);
        }

        [Route("edittvshow/{id}/{title}")]
        public async Task<IActionResult> EditTVShow(int id)
        {
            var tvshow = await dbAccess.GetTVShowAsync(id);
            var model = new TVShowVM
            {
                Title = tvshow.Title,
                ReleaseDate = tvshow.ReleaseDate,
                EndDate = tvshow.EndDate,
                Seasons = tvshow.Seasons,
                Duration = new TimeSpan(tvshow.Duration.Ticks),
                PosterFileName = tvshow.PosterImage,
                Description = tvshow.Description,
                TrailerURL = tvshow.TrailerURL,
                Director = tvshow.Director.PersonId

            };
            model.AllGenres = new MultiSelectList(await dbAccess.GetAllGenresAsync(), "GenreId", "Name", tvshow.Genres.Select(t => t.GenreId));
            model.AllPersons = new MultiSelectList(await dbAccess.GetAllPersonsAsync(), "PersonId", "FullName", tvshow.Stars.Select(t => t.PersonId));
            model.AllKeywords = new MultiSelectList(await dbAccess.GetAllKeywordNamesAsync(), tvshow.Keywords.Select(t => t.Keyword.Name));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edittvshow/{id}/{title}")]
        public async Task<IActionResult> EditTVShow(TVShowVM model, int id)
        {
            if (ModelState["EndDate"] != null) ModelState["EndDate"].Errors.Clear();
            ModelState.Where(m => m.Key == "EndDate").FirstOrDefault().Value.Errors.Clear();
            if (model.SelectedKeywords != null)
                await dbAccess.CreateUniqueKeywordNamesAsync(model.SelectedKeywords.ToList());
            if (model.PosterFormFile != null)
            {
                if (Enum.GetNames(typeof(ImageTypes)).Select(n => n.Replace("_", "/")).Contains(model.PosterFormFile.ContentType))
                    model.PosterFileName = await FileUpload.ImageUploadAsync(webHostEnvironment, model.PosterFormFile);
                else
                    ModelState.AddModelError("PosterFormFile", "Please select a GIF, JPG or PNG image");
            }
            if (string.IsNullOrEmpty(model.PosterFileName))
                ModelState.AddModelError("PosterFormFile", "Please select a poster image");
            if (ModelState.IsValid)
            {
                await dbAccess.UpdateTVShowAsync(model, id);
                return RedirectToAction("TVShow", new { id, title = model.Title.ToLower().Replace(" ", "-") });
            }
            model.AllGenres = new SelectList(await dbAccess.GetAllGenresAsync(), "GenreId", "Name");
            model.AllPersons = new SelectList(await dbAccess.GetAllPersonsAsync(), "PersonId", "FullName");
            model.AllKeywords = new SelectList(await dbAccess.GetAllKeywordNamesAsync());
            return View(model);
        }
    }
}
