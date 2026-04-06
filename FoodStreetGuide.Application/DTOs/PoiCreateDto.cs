using System.Collections.Generic;

namespace FoodStreetGuide.Application.DTOs
{
    public class PoiCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusMeters { get; set; } = 50;
        public string AudioUrl { get; set; } = string.Empty;
        public string Language { get; set; } = "vi";
        public int Priority { get; set; } = 1;
        public List<string> ImageUrls { get; set; } = new();
    }
}