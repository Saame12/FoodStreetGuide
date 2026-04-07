using FoodStreetGuide.Application.Interfaces;
using FoodStreetGuide.Application.Service;
using FoodStreetGuide.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ MVC vào container
builder.Services.AddControllersWithViews();

// --- SỬA: Dùng chung logic tìm thư mục gốc giống hệt bên API ---
var currentDir = AppContext.BaseDirectory;
DirectoryInfo? rootDir = new DirectoryInfo(currentDir);
while (rootDir != null && !rootDir.GetFiles("*.slnx").Any() && !rootDir.GetFiles("*.sln").Any())
{
    rootDir = rootDir.Parent;
}
string dbPath = Path.Combine(rootDir?.FullName ?? currentDir, "foodstreet.db");
// -------------------------------------------------------------

// 1. Cấu hình kết nối Database (Trỏ đúng vào file mà API đã tạo)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

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

// Cấu hình Route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Poi}/{action=Index}/{id?}");

app.Run();