using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMDB.Controllers
{
    public class GenreController : IdentityControllerBase
    {
        [Route("genre/{id}/{name}")]
        public async Task<IActionResult> Genre(int id)
        {
            var genre = await dbAccess.GetGenreAsync(id);
            return View(genre);
        }
    }
}
