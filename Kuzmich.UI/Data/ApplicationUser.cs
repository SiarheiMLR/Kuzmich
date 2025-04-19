using Microsoft.AspNetCore.Identity;

namespace Kuzmich.UI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? Avatar { get; set; } // Свойство для хранения изображение
        public string? MimeType { get; set; } = string.Empty; // Свойство, описывающее MIME-тип изображения
    }
}
