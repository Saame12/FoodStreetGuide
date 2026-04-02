using Microsoft.AspNetCore.Mvc;
using FoodStreetGuide.Infrastructure;
using FoodStreetGuide.Core;

namespace FoodStreetGuide.API.Controllers;

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
    public IActionResult GetAll()
    {
        return Ok(_context.POIs.ToList());
    }
    [HttpGet("near")]
    public IActionResult GetNear(double lat, double lng)
    {
        var pois = _context.POIs.ToList();

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
    public IActionResult Add(POI poi)
    {
        _context.POIs.Add(poi);
        _context.SaveChanges();
        return Ok(poi);
    }
}