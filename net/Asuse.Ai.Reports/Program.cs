using FastReport.DataVisualization.Charting;
using Microsoft.Extensions.Configuration;
using Asuse.Ai.Reports.Services;
using Asuse.Ai.Reports.Settings;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ReportingSettings>(builder.Configuration.GetSection("Reporting"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddFastReport();
builder.Services.AddTransient<ReportingService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseFastReport();

app.Run();
