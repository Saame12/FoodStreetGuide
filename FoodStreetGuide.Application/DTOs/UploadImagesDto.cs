using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace FoodStreetGuide.Application.DTOs
{
    public class UploadImagesDto
    {
        public Guid PoiId { get; set; }
        public List<IFormFile> ImageFiles { get; set; } = new();
    }
}