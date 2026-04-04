using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStreetGuide.Core.Entities
{
    internal class UserPoiInteraction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PoiId { get; set; }
        public string DeviceId { get; set; } = string.Empty; // anonymous device ID (không lưu thông tin cá nhân)
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public double ListenSeconds { get; set; } // thời gian nghe narration
        public double? Latitude { get; set; } // vị trí lúc nghe (cho heatmap anonymous)
        public double? Longitude { get; set; }
    }
}
