using AMDB.Data;
using AMDB.Models;
using AMDB.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Controllers
{
    public class PersonController : IdentityControllerBase
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public PersonController(IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
        }

        [Route("persons")]
        public async Task<IActionResult> Persons()
        {
            var persons = await dbAccess.GetAllPersonsAsync();
            return View(persons);
        }

        [Route("person/{id}/{fullname}")]
        public async Task<IActionResult> Person(int id)
        {
            var person = await dbAccess.GetPersonAsync(id);
            return View(person);
        }

        [Route("createperson")]
        public IActionResult CreatePerson()
        {
            return View(new PersonVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("createperson")]
        public async Task<IActionResult> CreatePerson(PersonVM model)
        {
            if (model.ProfileImageFormFile != null)
            {
                if (Enum.GetNames(typeof(ImageTypes)).Select(n => n.Replace("_", "/")).Contains(model.ProfileImageFormFile.ContentType))
                    model.ProfileImageFileName = await FileUpload.ImageUploadAsync(webHostEnvironment, model.ProfileImageFormFile);
                else
                    ModelState.AddModelError("ProfileImageFormFile", "Please select a GIF, JPG or PNG image");
            }
            if (string.IsNullOrEmpty(model.ProfileImageFileName))
                ModelState.AddModelError("ProfileImageFormFile", "Please select a poster image");
            if (ModelState.IsValid)
            {
                int id = await dbAccess.CreatePersonAsync(model);
                return RedirectToAction("Person", new { id, fullname = (model.FirstName + " " + model.LastName).ToLower().Replace(" ", "-") });
            }
            return View(model);
        }

        [Route("editperson/{id}/{fullname}")]
        public async Task<IActionResult> EditPerson(int id)
        {
            var person = await dbAccess.GetPersonAsync(id);
            var model = new PersonVM
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                DateOfBirth = person.DateOfBirth,
                ProfileImageFileName = person.ProfileImage,
                Biography = person.Biography
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("editperson/{id}/{fullname}")]
        public async Task<IActionResult> EditPerson(PersonVM model, int id)
        {
            if (model.ProfileImageFormFile != null)
            {
                if (Enum.GetNames(typeof(ImageTypes)).Select(n => n.Replace("_", "/")).Contains(model.ProfileImageFormFile.ContentType))
                    model.ProfileImageFileName = await FileUpload.ImageUploadAsync(webHostEnvironment, model.ProfileImageFormFile);
                else
                    ModelState.AddModelError("ProfileImageFormFile", "Please select a GIF, JPG or PNG image");
            }
            if (string.IsNullOrEmpty(model.ProfileImageFileName))
                ModelState.AddModelError("ProfileImageFormFile", "Please select a poster image");
            if (ModelState.IsValid)
            {
                await dbAccess.UpdatePersonAsync(model, id);
                return RedirectToAction("Person", new { id, fullname = (model.FirstName + " " + model.LastName).ToLower().Replace(" ", "-") });
            }
            return View(model);
        }
    }
}
