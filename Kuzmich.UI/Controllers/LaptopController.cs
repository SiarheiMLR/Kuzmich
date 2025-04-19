using Kuzmich.Domain.Models;
using Kuzmich.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Kuzmich.Domain.Entities;
using Kuzmich.UI.Services.CategoryService;

namespace Kuzmich.UI.Controllers
{
    public class LaptopController : Controller
    {
        private readonly ILaptopService _laptopService;
        private readonly ICategoryService _categoryService;

        public LaptopController(ILaptopService laptopService, ICategoryService categoryService)
        {
            _laptopService = laptopService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(string? category)
        {
            var laptopResult = await _laptopService.GetProductListAsync(category);
            var categoryResult = await _categoryService.GetCategoryListAsync();

            if (!laptopResult.Success || !categoryResult.Success)
                return View("Error");

            ViewData["categories"] = categoryResult.Data;
            ViewData["currentCategory"] = category ?? "все";

            return View(laptopResult.Data);
        }
    }
}

