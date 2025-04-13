using Microsoft.AspNetCore.Mvc;

namespace Kuzmich.UI.Components
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() 
        {
            return View();
        }
    }
}
