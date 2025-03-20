using Microsoft.AspNetCore.Mvc;
using UrlAnalyzer.DTOs;
using UrlAnalyzer.Services;

namespace UrlAnalyzer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlAnalyzerController : ControllerBase
{
    private readonly IUrlAnalyzerService _analyzerService;
    private readonly ILogger<UrlAnalyzerController> _logger;

    public UrlAnalyzerController(
        IUrlAnalyzerService analyzerService,
        ILogger<UrlAnalyzerController> logger)
    {
        _analyzerService = analyzerService;
        _logger = logger;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<UrlAnalysisResponse>> AnalyzeUrl(
        [FromBody] UrlAnalysisRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received analysis request for URL: {Url}", request.Url);
        
        var result = await _analyzerService.AnalyzeUrlAsync(request.Url, cancellationToken);
        
        if (!string.IsNullOrEmpty(result.Error))
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }
} 