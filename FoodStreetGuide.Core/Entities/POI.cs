using System;
using System.Collections.Generic;

namespace FoodStreetGuide.Core.Entities
{
    public class Poi
    {
        public Guid Id { get; set; } = Guid.NewGuid();     // ← Đổi thành Guid

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RadiusMeters { get; set; } = 50;

        public string? AudioUrl { get; set; }
        public string Language { get; set; } = "vi";
        public int Priority { get; set; } = 0;
        public string Category { get; set; } = string.Empty;

        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}