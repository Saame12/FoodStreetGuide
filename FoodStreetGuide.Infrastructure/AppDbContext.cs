using Microsoft.EntityFrameworkCore;
using FoodStreetGuide.Core;

namespace FoodStreetGuide.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<POI> POIs { get; set; }
}