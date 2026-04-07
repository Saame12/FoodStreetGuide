using Microsoft.EntityFrameworkCore;
using FoodStreetGuide.Application.Interfaces;
using FoodStreetGuide.Core.Entities;

namespace FoodStreetGuide.Infrastructure.Data
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Poi> Pois { get; set; } = null!;
        public DbSet<AppUser> AppUsers { get; set; } = null!;
        public DbSet<UserPoiInteraction> UserPoiInteractions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Poi>(entity => {
                // SỬA TẠI ĐÂY: Đổi "POIs" thành "Pois" để khớp với lỗi "no such table: Pois"
                entity.ToTable("Pois");

                entity.Property(p => p.ImageUrls)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}