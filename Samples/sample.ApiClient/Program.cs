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

builder.Services.AddWonderfulCaptcha(builder.Configuration,
    option => option
    .UseInMemoryCacheProvider(builder.Services)
    .WithTextFontSize(100)
    .WithTextBrush(BrushEnum.Solid)
    .WithTextShadow(false)
    .WithTextColor(ColorEnum.Blue)
    .WithTextLength(5, 5)
    .WithImageSizeStrategy(ImageSizeStrategy.Fit)
    .WithTextFontSizeVarietyRange(20)
    .WithTextSkewRange(10)
    .UseNoise(new()
    {
        SaltAndPepperDensity = 0.09,
        OilPaintLevel = 0,
        MaxLineNumbers = 10,
    }));

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
