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
    public class MovieController : IdentityControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public MovieController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        [Route("movies")]
        public async Task<IActionResult> Movies()
        {
            var movies = await dbAccess.GetAllMoviesAsync();
            return View(movies);
        }

        [Route("movie/{id}/{title}")]
        public async Task<IActionResult> Movie(int id)
        {
            var movie = await dbAccess.GetMovieAsync(id);
            movie.Suggestions = await dbAccess.GetProductionsOrderedByBestKeywordMatches(movie);
            return View(movie);
        }

        [Route("createmovie")]
        public async Task<IActionResult> CreateMovie()
        {
            var model = new MovieVM
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
        [Route("createmovie")]
        public async Task<IActionResult> CreateMovie(MovieVM model)
        {
            if (model.SelectedKeywords != null)
                await dbAccess.CreateUniqueKeywordNamesAsync(model.SelectedKeywords.ToList());
            if (model.PosterFormFile != null) {
                if (Enum.GetNames(typeof(ImageTypes)).Select(n => n.Replace("_", "/")).Contains(model.PosterFormFile.ContentType))
                    model.PosterFileName = await FileUpload.ImageUploadAsync(webHostEnvironment, model.PosterFormFile);
                else
                    ModelState.AddModelError("PosterFormFile", "Please select a GIF, JPG or PNG image");
            }
            if (string.IsNullOrEmpty(model.PosterFileName))
                ModelState.AddModelError("PosterFormFile", "Please select a poster image");
            if (ModelState.IsValid)
            {
                int id = await dbAccess.CreateMovieAsync(model);
                return RedirectToAction("Movie", new { id, title = model.Title.ToLower().Replace(" ","-") });
            }
            model.AllGenres = new SelectList(await dbAccess.GetAllGenresAsync(), "GenreId", "Name");
            model.AllPersons = new SelectList(await dbAccess.GetAllPersonsAsync(), "PersonId", "FullName");
            model.AllKeywords = new SelectList(await dbAccess.GetAllKeywordNamesAsync());
            return View(model);
        }

        [Route("editmovie/{id}/{title}")]
        public async Task<IActionResult> EditMovie(int id)
        {
            var movie = await dbAccess.GetMovieAsync(id);
            var model = new MovieVM
            {
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Duration = new TimeSpan(movie.Duration.Ticks),
                PosterFileName = movie.PosterImage,
                Description = movie.Description,
                TrailerURL = movie.TrailerURL,
                Director = movie.Director.PersonId
            };
            model.AllGenres = new MultiSelectList(await dbAccess.GetAllGenresAsync(), "GenreId", "Name", movie.Genres.Select(t => t.GenreId));
            model.AllPersons = new MultiSelectList(await dbAccess.GetAllPersonsAsync(), "PersonId", "FullName", movie.Stars.Select(t => t.PersonId));
            model.AllKeywords = new MultiSelectList(await dbAccess.GetAllKeywordNamesAsync(), movie.Keywords.Select(t => t.Keyword.Name));
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editmovie/{id}/{title}")]
        public async Task<IActionResult> EditMovie(MovieVM model, int id)
        {
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
                await dbAccess.UpdateMovieAsync(model, id);
                return RedirectToAction("Movie", new { id, title = model.Title.ToLower().Replace(" ", "-") });
            }
            model.AllGenres = new SelectList(await dbAccess.GetAllGenresAsync(), "GenreId", "Name");
            model.AllPersons = new SelectList(await dbAccess.GetAllPersonsAsync(), "PersonId", "FullName");
            model.AllKeywords = new SelectList(await dbAccess.GetAllKeywordNamesAsync());
            return View(model);
        }
    }
}
