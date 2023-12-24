using WonderfulCaptcha;
using WonderfulCaptcha.Cache.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddMemoryCache();

builder.Services.AddWonderfulCaptcha(options =>
{
    options.TextOptions.TextColor = ColorEnum.Random;
    options.TextOptions.TextBrush = BrushEnum.Random;
    options.NoiseOptions.OilPaintLevel = 0;
    options.NoiseOptions.SaltAndPepperDensityPercent = 1;

}).UseInMemoryCacheProvider(builder.Services);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyPolicy");

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
