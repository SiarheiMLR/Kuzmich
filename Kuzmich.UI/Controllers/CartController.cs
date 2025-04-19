using Microsoft.AspNetCore.Mvc;
using Kuzmich.Domain.Entities;

namespace Kuzmich.UI.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";

        // Добавление товара в корзину
        public IActionResult Add(int id, string returnUrl)
        {
            // Получаем корзину из сессии (или создаём новую)
            var cart = HttpContext.Session.GetObjectFromJson<List<int>>(CartSessionKey) ?? new List<int>();

            // Добавляем товар по ID
            cart.Add(id);

            // Сохраняем обратно в сессию
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

            // Уведомление через TempData (одноразовое)
            TempData["info"] = $"Товар с ID {id} добавлен в корзину";

            return Redirect(returnUrl ?? "/");
        }

        // Просмотр содержимого корзины (пока только ID-шники)
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<int>>(CartSessionKey) ?? new List<int>();
            return View(cart);
        }
    }

    // Вспомогательные методы расширения для работы с JSON-сессией
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, System.Text.Json.JsonSerializer.Serialize(value));
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
    }
}


