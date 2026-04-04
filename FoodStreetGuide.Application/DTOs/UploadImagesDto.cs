using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStreetGuide.Application.DTOs
{
    internal class UploadImagesDto
    {
        public Guid PoiId { get; set; }
        public List<IFormFile> ImageFiles { get; set; } = new();
    }
}
