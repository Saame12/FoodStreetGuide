using System;

namespace FoodStreetGuide.Core.Entities
{
    public class UserPoiInteraction   // đổi internal → public
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PoiId { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public double ListenSeconds { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}