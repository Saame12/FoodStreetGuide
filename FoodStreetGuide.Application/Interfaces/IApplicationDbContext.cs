using FoodStreetGuide.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FoodStreetGuide.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Poi> Pois { get; set; }
        DbSet<AppUser> AppUsers { get; set; }
        DbSet<UserPoiInteraction> UserPoiInteractions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}