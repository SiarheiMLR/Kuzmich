using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Kuzmich.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Text"] = "Лабораторная работа №2";
            var list = new List<ListDemo>
            {
                new ListDemo {Id=1, Name="Item 1"},
                new ListDemo {Id=2, Name="Item 2"},
                new ListDemo {Id=3, Name="Item 3"}
            };
            SelectList data = new SelectList(list, "Id", "Name");
            return View(data);
        }
    }

    public class ListDemo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
