using OJMonitor.API.Services;
using OJMonitor.API.Services.Crawlers;
using OJMonitor.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 注册HTTP客户端
builder.Services.AddHttpClient();

// 注册爬虫服务
builder.Services.AddScoped<IDataCrawler, LuoguCrawler>();
builder.Services.AddScoped<IDataCrawler, LsyoiCrawler>();
builder.Services.AddScoped<IDataCrawler, OIClassCrawler>();

// 注册业务服务
builder.Services.AddScoped<IPlatformService, PlatformService>();

// 配置CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowVueApp");
app.UseAuthorization();
app.MapControllers();

app.Run();