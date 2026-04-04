using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStreetGuide.Application.Service
{
    internal class IAuthService
    {
        Task<string> LoginAsync(string username, string password);

    }
}
