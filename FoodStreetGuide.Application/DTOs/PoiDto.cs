using System;
using System.Collections.Generic;

namespace FoodStreetGuide.Application.DTOs
{
    public class PoiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusMeters { get; set; }
        public string AudioUrl { get; set; } = string.Empty;
        public string Language { get; set; } = "vi";
        public int Priority { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }
}