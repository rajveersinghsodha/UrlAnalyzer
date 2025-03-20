using Serilog;
using UrlAnalyzer.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() 
    .WriteTo.File("Logs/urlanalyzer-.log",
        rollingInterval: RollingInterval.Day,  // Create new log file each day
        retainedFileCountLimit: 7,             // Keep last 7 days of logs
        fileSizeLimitBytes: 5242880)          // Limit each file to 5MB
    .CreateLogger();

// Configure the application to use Serilog
builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure HttpClient with custom settings
builder.Services.AddHttpClient<IUrlAnalyzerService, UrlAnalyzerService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "UrlAnalyzer/1.0");  // Add user agent for web requests
    client.Timeout = TimeSpan.FromSeconds(30);                          // Set timeout to 30 seconds
});

// Register application services
builder.Services.AddScoped<IUrlAnalyzerService, UrlAnalyzerService>();

var app = builder.Build();

// Configure the HTTP request pipeline environment check
if (app.Environment.IsDevelopment())
{  // Show detailed errors in development
    app.UseDeveloperExceptionPage();  
}
else
{
    app.UseExceptionHandler("/Home/Error");  
    app.UseHsts();                         
}

app.UseHttpsRedirection(); 
app.UseStaticFiles();       

app.UseRouting();          
app.UseAuthorization();   

// Configure default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ensure Logs directory exists for file logging
var logPath = Path.Combine(builder.Environment.ContentRootPath, "Logs");
if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}

// Application startup with proper logging and error handling
try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{   // Ensure all logs are written before shutdown
    Log.CloseAndFlush();  
}
