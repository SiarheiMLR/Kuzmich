using Kuzmich.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kuzmich.UI.Controllers
{
    public class ImageController(UserManager<ApplicationUser> userManager) : Controller
    {
        public async Task<IActionResult> GetAvatar()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null) return NotFound();

            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return NotFound();

            if (user.Avatar != null)
                return File(user.Avatar, user.MimeType);

            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "default-profile-picture.png");
            return PhysicalFile(imagePath, "image/png");
        }
    }
}
