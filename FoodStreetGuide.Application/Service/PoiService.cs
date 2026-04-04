using FoodStreetGuide.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodStreetGuide.Application.Service
{
    internal class PoiService
    {
        private readonly AppDbContext _context;

        public PoiService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PoiDto>> GetAllAsync(string language = "vi")
        {
            var pois = await _context.Pois
                .Where(p => p.Language == language)
                .ToListAsync();

            return pois.Select(p => new PoiDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                RadiusMeters = p.RadiusMeters,
                AudioUrl = p.AudioUrl,
                Language = p.Language,
                Priority = p.Priority,
                ImageUrls = p.ImageUrls
            }).ToList();
        }

        public async Task<PoiDto?> GetByIdAsync(Guid id)
        {
            var poi = await _context.Pois.FindAsync(id);
            if (poi == null) return null;

            return new PoiDto
            {
                Id = poi.Id,
                Name = poi.Name,
                Description = poi.Description,
                Latitude = poi.Latitude,
                Longitude = poi.Longitude,
                RadiusMeters = poi.RadiusMeters,
                AudioUrl = poi.AudioUrl,
                Language = poi.Language,
                Priority = poi.Priority,
                ImageUrls = poi.ImageUrls
            };
        }

        public async Task<PoiDto> CreateAsync(PoiCreateDto dto)
        {
            var poi = new Poi
            {
                Name = dto.Name,
                Description = dto.Description,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                RadiusMeters = dto.RadiusMeters,
                AudioUrl = dto.AudioUrl,
                Language = dto.Language,
                Priority = dto.Priority,
                ImageUrls = dto.ImageUrls
            };

            _context.Pois.Add(poi);
            await _context.SaveChangesAsync();

            return new PoiDto
            {
                Id = poi.Id,
                Name = poi.Name,
                Description = poi.Description,
                Latitude = poi.Latitude,
                Longitude = poi.Longitude,
                RadiusMeters = poi.RadiusMeters,
                AudioUrl = poi.AudioUrl,
                Language = poi.Language,
                Priority = poi.Priority,
                ImageUrls = poi.ImageUrls
            };
        }
        public async Task<PoiDto?> UpdateAsync(Guid id, PoiCreateDto dto)
        {
            var poi = await _context.Pois.FindAsync(id);
            if (poi == null) return null;

            // Update fields
            poi.Name = dto.Name;
            poi.Description = dto.Description;
            poi.Latitude = dto.Latitude;
            poi.Longitude = dto.Longitude;
            poi.RadiusMeters = dto.RadiusMeters;
            poi.AudioUrl = dto.AudioUrl;
            poi.Language = dto.Language;
            poi.Priority = dto.Priority;
            poi.ImageUrls = dto.ImageUrls;

            _context.Pois.Update(poi);
            await _context.SaveChangesAsync();

            return new PoiDto
            {
                Id = poi.Id,
                Name = poi.Name,
                Description = poi.Description,
                Latitude = poi.Latitude,
                Longitude = poi.Longitude,
                RadiusMeters = poi.RadiusMeters,
                AudioUrl = poi.AudioUrl,
                Language = poi.Language,
                Priority = poi.Priority,
                ImageUrls = poi.ImageUrls
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var poi = await _context.Pois.FindAsync(id);
            if (poi == null) return false;

            _context.Pois.Remove(poi);
            await _context.SaveChangesAsync();
            return true;
        }

        //audio-images
        public async Task<string> UploadAudioAsync(Guid poiId, IFormFile audioFile)
        {
            var poi = await _context.Pois.FindAsync(poiId);
            if (poi == null) throw new KeyNotFoundException("Không tìm thấy POI");

            // Kiểm tra file audio
            if (audioFile == null || audioFile.Length == 0)
                throw new ArgumentException("File audio không hợp lệ");

            var allowedExtensions = new[] { ".mp3", ".wav", ".m4a" };
            var extension = Path.GetExtension(audioFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException("Chỉ hỗ trợ file .mp3, .wav, .m4a");

            // Tạo tên file unique
            var fileName = $"{poiId}_{DateTime.UtcNow.Ticks}{extension}";
            var filePath = Path.Combine("wwwroot", "audios", fileName);

            // Tạo thư mục nếu chưa có
            Directory.CreateDirectory(Path.Combine("wwwroot", "audios"));

            // Lưu file
            using var stream = new FileStream(filePath, FileMode.Create);
            await audioFile.CopyToAsync(stream);

            // Cập nhật đường dẫn vào POI
            poi.AudioUrl = $"/audios/{fileName}";
            await _context.SaveChangesAsync();

            return poi.AudioUrl;
        }

        public async Task<List<string>> UploadImagesAsync(Guid poiId, List<IFormFile> imageFiles)
        {
            var poi = await _context.Pois.FindAsync(poiId);
            if (poi == null) throw new KeyNotFoundException("Không tìm thấy POI");

            var uploadedUrls = new List<string>();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            Directory.CreateDirectory(Path.Combine("wwwroot", "images"));

            foreach (var file in imageFiles)
            {
                if (file.Length == 0) continue;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension)) continue;

                var fileName = $"{poiId}_{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine("wwwroot", "images", fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                var url = $"/images/{fileName}";
                uploadedUrls.Add(url);
            }

            // Cập nhật danh sách ảnh
            poi.ImageUrls.AddRange(uploadedUrls);
            await _context.SaveChangesAsync();

            return uploadedUrls;
        }

    }
}
