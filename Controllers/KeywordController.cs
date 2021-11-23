using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMDB.Controllers
{
    public class KeywordController : IdentityControllerBase
    {
        [Route("keyword/{id}/{name}")]
        public async Task<IActionResult> Keyword(int id)
        {
            var Keyword = await dbAccess.GetKeywordAsync(id);
            return View(Keyword);
        }
    }
}
