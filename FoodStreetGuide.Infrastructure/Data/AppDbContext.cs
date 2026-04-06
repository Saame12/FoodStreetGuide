using Microsoft.EntityFrameworkCore;
using FoodStreetGuide.Application.Interfaces;   // ← using này
using FoodStreetGuide.Core.Entities;

namespace FoodStreetGuide.Infrastructure.Data
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Poi> Pois { get; set; } = null!;
        public DbSet<AppUser> AppUsers { get; set; } = null!;
        public DbSet<UserPoiInteraction> UserPoiInteractions { get; set; } = null!;

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Poi>().ToTable("Pois");
        }
    }
}