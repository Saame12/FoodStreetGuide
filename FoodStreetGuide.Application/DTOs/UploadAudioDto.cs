using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace FoodStreetGuide.Application.DTOs
{
    public class UploadAudioDto
    {
        public Guid PoiId { get; set; }
        public IFormFile? AudioFile { get; set; }
    }
}