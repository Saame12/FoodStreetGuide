var builder = DistributedApplication.CreateBuilder(args);

// 1. Đăng ký API Server (Bỏ HealthCheck nếu bạn chưa code trong API)
var server = builder.AddProject<Projects.FoodStreetGuide_API_Server>("server")
    .WithExternalHttpEndpoints();

// 2. Đăng ký Web MVC (Dự án quản lý POI của bạn)
// Phải thêm WithReference để Web biết địa chỉ của API
builder.AddProject<Projects.FoodStreetGuide_Web>("foodstreetguide-web")
    .WithReference(server);

// 3. Đăng ký Vite App (Nếu bạn có dùng React/Vue ở thư mục ../frontend)
var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithReference(server);

builder.Build().Run();
