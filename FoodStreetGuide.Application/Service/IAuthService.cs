using System.Threading.Tasks;   

namespace FoodStreetGuide.Application.Service
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string username, string password);

        // Bạn có thể thêm các phương thức khác sau này
        // Task<string> RegisterAsync(...);
        // Task LogoutAsync();
    }
}