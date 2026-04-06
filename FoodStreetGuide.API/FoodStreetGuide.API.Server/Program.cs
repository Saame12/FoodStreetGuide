using FluentValidation;
using FoodStreetGuide.Application.Interfaces;
using FoodStreetGuide.Application.Service;
using FoodStreetGuide.Application.Validation;
using FoodStreetGuide.Infrastructure.Data;
using FoodStreetGuide.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Cấu hình Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=../foodstreet.db")); // Đưa ra thư mục cha dùng chung

// 2. Đăng ký Dependency Injection (Sửa lỗi thiếu Interface)
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