using FoodStreetGuide.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStreetGuide.Application.Service
{
    internal class IPoiService
    {
        Task<List<PoiDto>> GetAllAsync(string language = "vi");
        Task<PoiDto?> GetByIdAsync(Guid id);
        Task<PoiDto> CreateAsync(PoiCreateDto dto);

        // Thêm 2 method mới
        Task<PoiDto?> UpdateAsync(Guid id, PoiCreateDto dto);  // Update POI
        Task<bool> DeleteAsync(Guid id);                      // Delete POI
                                                              // audio- images
        Task<string> UploadAudioAsync(Guid poiId, IFormFile audioFile);
        Task<List<string>> UploadImagesAsync(Guid poiId, List<IFormFile> imageFiles);
    }
}
