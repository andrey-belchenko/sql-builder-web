using FastReport.DataVisualization.Charting;
using Microsoft.Extensions.Configuration;
using Asuse.Ai.Reports.Services;
using Asuse.Ai.Reports.Settings;
using System.Runtime;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ReportingSettings>(builder.Configuration.GetSection("Reporting"));

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddFastReport();
builder.Services.AddTransient<ReportingService>();
builder.Services.AddTransient<SqlBuilderService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map API controllers (attribute-based routing)
app.MapControllers();

// Map MVC controllers (convention-based routing)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseFastReport();

app.Run();
