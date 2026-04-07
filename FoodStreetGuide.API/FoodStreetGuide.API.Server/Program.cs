using FluentValidation;
using FoodStreetGuide.Application.Interfaces;
using FoodStreetGuide.Application.Service;
using FoodStreetGuide.Application.Validation;
using FoodStreetGuide.Infrastructure.Data;
using FoodStreetGuide.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// --- SỬA: Tự động tìm thư mục gốc Solution (chứa file .slnx) để đặt DB dùng chung ---
var currentDir = AppContext.BaseDirectory;
DirectoryInfo? rootDir = new DirectoryInfo(currentDir);
while (rootDir != null && !rootDir.GetFiles("*.slnx").Any() && !rootDir.GetFiles("*.sln").Any())
{
    rootDir = rootDir.Parent;
}
string dbPath = Path.Combine(rootDir?.FullName ?? currentDir, "foodstreet.db");
// ----------------------------------------------------------------------------------

// 1. Cấu hình Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}")); // Dùng chung dbPath tuyệt đối

// 2. Đăng ký Dependency Injection
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddScoped<IPoiService, PoiService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 3. Cấu hình Controller & Swagger
builder.Services.AddValidatorsFromAssemblyContaining<PoiCreateDtoValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- SỬA: Khởi tạo Database và tạo bảng ngay khi chạy ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Sử dụng EnsureCreated để bỏ qua lỗi Metadata/Migration và tạo bảng Pois ngay lập tức
    db.Database.EnsureCreated();
}
// -------------------------------------------------------

// 4. Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();

app.Run();