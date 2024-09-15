using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient<FinancialDataService>();
builder.Services.AddScoped<StockAnalysisService>();
builder.Services.AddScoped<EMAService>();
builder.Services.AddScoped<GrowthRateCalculator>();
builder.Services.AddScoped<DCFCalculator>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://laughing-umbrella-4gp6q4w4pjp24xg-8080.app.github.dev")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable CORS
app.UseCors("AllowSpecificOrigin");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Urls.Add("https://localhost:5000");
app.Urls.Add("https://localhost:5001");

app.Run();