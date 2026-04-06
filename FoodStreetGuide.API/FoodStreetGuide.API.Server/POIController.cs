using FoodStreetGuide.Core.Entities;                    // ← Dùng Poi entity
using FoodStreetGuide.Infrastructure.Data;              // ← Dùng AppDbContext
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FoodStreetGuide.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class POIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public POIController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pois = await _context.Pois
                .ToListAsync<Poi>();           // ← THÊM <Poi>

            return Ok(pois);
        }

        [HttpGet("near")]
        public async Task<IActionResult> GetNear(double lat, double lng)
        {
            var pois = await _context.Pois
                .ToListAsync<Poi>();           // ← THÊM <Poi>

            var result = pois
                .Select(p => new
                {
                    Poi = p,
                    Distance = Math.Sqrt(
                        Math.Pow(p.Latitude - lat, 2) +
                        Math.Pow(p.Longitude - lng, 2)
                    )
                })
                .OrderBy(x => x.Distance)
                .Take(10)
                .Select(x => x.Poi);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Poi poi)        // ← Sửa POI thành Poi
        {
            if (poi == null)
                return BadRequest("POI data is required");

            _context.Pois.Add(poi);                          // ← Sửa POIs thành Pois
            await _context.SaveChangesAsync();               // ← Nên dùng async

            return CreatedAtAction(nameof(GetAll), new { id = poi.Id }, poi);
        }
    }
}