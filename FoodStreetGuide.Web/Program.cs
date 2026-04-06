using FoodStreetGuide.Application.Interfaces;
using FoodStreetGuide.Application.Service;
using FoodStreetGuide.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ MVC vào container
builder.Services.AddControllersWithViews();

// 1. Cấu hình kết nối Database (Dùng chung file .db với API)
// Lưu ý: Đường dẫn có thể cần điều chỉnh tùy vào vị trí folder project của bạn
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=../foodstreet.db"));
// 2. Đăng ký Dependency Injection cho các Service
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddScoped<IPoiService, PoiService>();

var app = builder.Build();

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Cấu hình Route mặc định để chạy vào trang danh sách POI trước cho nhanh
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Poi}/{action=Index}/{id?}");

app.Run();