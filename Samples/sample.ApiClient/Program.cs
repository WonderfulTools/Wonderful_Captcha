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
builder.Services.AddWonderfulCaptcha(o =>
{
    o.TextOptions.Strategy = StrategyEnum.SumOfTwoNumbersStrategy;
    o.NoiseOptions.MaxLineNumbers = 0;
    o.NoiseOptions.OilPaintLevel = 2;
    o.UseInMemoryCacheProvider();
});

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
