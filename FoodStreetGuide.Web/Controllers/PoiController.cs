using Microsoft.AspNetCore.Mvc;
using FoodStreetGuide.Application.Service;
using FoodStreetGuide.Application.DTOs;

namespace FoodStreet.Web.Controllers
{
    public class PoiController : Controller
    {
        private readonly IPoiService _poiService;

        public PoiController(IPoiService poiService)
        {
            _poiService = poiService;
        }

        // Trang hiển thị danh sách POI
        public async Task<IActionResult> Index()
        {
            var pois = await _poiService.GetAllAsync();
            return View(pois);
        }

        // Trang tạo mới POI
        public IActionResult Create()
        {
            return View(new PoiCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PoiCreateDto dto)
        {
            if (ModelState.IsValid)
            {
                await _poiService.CreateAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> UploadMedia(Guid id, IFormFile audio, List<IFormFile> images)
        {
            if (audio != null)
            {
                await _poiService.UploadAudioAsync(id, audio); // Gọi service đã có [cite: 292]
            }
            if (images != null && images.Any())
            {
                await _poiService.UploadImagesAsync(id, images); // Gọi service đã có [cite: 312]
            }
            return RedirectToAction(nameof(Index));
        }
    }
}