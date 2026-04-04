using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStreetGuide.Application.DTOs
{
    internal class UploadAudioDto
    {
        public Guid PoiId { get; set; }
        public IFormFile AudioFile { get; set; } = null!;
    }
}
