using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OJMonitor.API.Data;
using OJMonitor.API.Services;
using OJMonitor.API.Services.Crawlers;
using OJMonitor.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 添加 Swagger 服务
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "OJ Monitor API", Version = "v1" });
});

// 添加数据库上下文（使用内存数据库）
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("OJMonitor"));

// 注册HTTP客户端
builder.Services.AddHttpClient();

// 注册爬虫服务
builder.Services.AddScoped<IDataCrawler, LuoguCrawler>();

// 注册业务服务
builder.Services.AddScoped<IPlatformService, PlatformService>();

// 配置CORS - 允许所有来源以简化调试
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OJ Monitor API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// 添加一个根路径的欢迎页面
app.MapGet("/", () => "OJ Monitor API is running! Visit /swagger for API documentation.");

app.Run();