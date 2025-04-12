using Microsoft.AspNetCore.Mvc;

namespace Kuzmich.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
