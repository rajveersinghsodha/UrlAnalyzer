using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrlAnalyzer.DTOs;
using UrlAnalyzer.Services;

namespace UrlAnalyzer.Controllers;

/// <summary>
/// Controller responsible for handling the main application views and URL analysis functionality.
/// Manages the user interface interactions and coordinates with the URL analyzer service.
/// </summary>
public class HomeController : Controller
{
    private readonly IUrlAnalyzerService _analyzerService;
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Initializes a new instance of the HomeController.
    /// </summary>
   
    public HomeController(IUrlAnalyzerService analyzerService, ILogger<HomeController> logger)
    {
        _analyzerService = analyzerService;
        _logger = logger;
    }

    /// <summary>
    /// Displays the main page with the URL analysis form.
    /// </summary>
    /// <returns>The Index view containing the URL input form</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Handles the URL analysis request from the form submission.
    /// </summary>
    /// <param name="url">The URL to analyze, submitted from the form</param>
    /// <param name="cancellationToken">Token for cancelling the operation</param>
    /// <returns>
    /// - View("Result") with analysis results if successful
    /// - BadRequest if URL is empty
    /// - View("Error") if analysis fails
    /// </returns>
    [HttpPost]
    public async Task<IActionResult> Analyze([FromForm] string url)
    {
        // Validate URL input
        if (string.IsNullOrWhiteSpace(url))
        {
            _logger.LogWarning("Empty URL received in Analyze action");
            return BadRequest("URL is required");
        }

        try
        {
            _logger.LogInformation("Starting URL analysis for: {Url}", url);
            
            // Perform URL analysis using the service
            var result = await _analyzerService.AnalyzeUrlAsync(url);
            
            _logger.LogInformation("Successfully analyzed URL: {Url}", url);
            return View("Result", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing URL: {Url}", url);
            return View("Error", "Failed to analyze URL. Please ensure the URL is valid and accessible.");
        }
    }

    /// <summary>
    /// Displays the error page when an unhandled exception occurs.
    /// </summary>
    /// <returns>The Error view with error details</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
