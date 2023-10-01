using Microsoft.AspNetCore.Mvc;

namespace TerriaMVC.Controllers
{
    public class TerriaMapController : Controller
    {
        public IActionResult TerriaMap()
        {
            return View();
        }
    }
}
