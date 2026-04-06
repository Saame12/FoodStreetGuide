using System;

namespace FoodStreetGuide.Core.Entities
{
    // Đổi internal → public
    public class AppUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Username { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;

        public string Role { get; set; } = "ShopOwner";   // Admin | ShopOwner

        // Nên thêm các trường hữu ích cho authentication
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }
}