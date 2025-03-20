using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UrlAnalyzer.DTOs;
using UrlAnalyzer.Services;

namespace UrlAnalyzer.Controllers;

public class HomeController : Controller
{
    private readonly IUrlAnalyzerService _analyzerService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IUrlAnalyzerService analyzerService, ILogger<HomeController> logger)
    {
        _analyzerService = analyzerService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Analyze([FromForm] string url, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return BadRequest("URL is required");
        }

        try
        {
            var result = await _analyzerService.AnalyzeUrlAsync(url, cancellationToken);
            return View("Result", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing URL: {Url}", url);
            return View("Error", "Failed to analyze URL. Please ensure the URL is valid and accessible.");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
