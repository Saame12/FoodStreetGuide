using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStreetGuide.Core.Entities
{
    internal class AppUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = "ShopOwner"; // Admin | ShopOwner
    }
}
