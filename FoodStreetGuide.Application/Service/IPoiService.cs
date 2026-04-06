using FoodStreetGuide.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodStreetGuide.Application.Service
{
    public interface IPoiService
    {
        Task<List<PoiDto>> GetAllAsync(string language = "vi");
        Task<PoiDto?> GetByIdAsync(Guid id);
        Task<PoiDto> CreateAsync(PoiCreateDto dto);

        // Thêm 2 method mới
        Task<PoiDto?> UpdateAsync(Guid id, PoiCreateDto dto);
        Task<bool> DeleteAsync(Guid id);

        // Upload audio & images
        Task<string> UploadAudioAsync(Guid poiId, IFormFile audioFile);
        Task<List<string>> UploadImagesAsync(Guid poiId, List<IFormFile> imageFiles);
    }
}