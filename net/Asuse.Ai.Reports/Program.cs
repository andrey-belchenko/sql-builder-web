using FastReport.DataVisualization.Charting;
using Microsoft.Extensions.Configuration;
using Asuse.Ai.Reports.Services;
using Asuse.Ai.Reports.Settings;
using Asuse.Ai.Reports.Converters;
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
        options.JsonSerializerOptions.Converters.Add(new ObjectDictionaryJsonConverter());
    });
builder.Services.AddFastReport();
builder.Services.AddTransient<TempDataService>();
builder.Services.AddTransient<ReportingService>();
builder.Services.AddTransient<SqlBuilderService>();
var requestTimeoutSeconds = builder.Configuration.GetValue("Reporting:RequestTimeoutSeconds", 3600);
builder.Services.AddRequestTimeouts(options =>
{
    options.DefaultPolicy = new Microsoft.AspNetCore.Http.Timeouts.RequestTimeoutPolicy
    {
        Timeout = TimeSpan.FromSeconds(requestTimeoutSeconds)
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRequestTimeouts();

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
