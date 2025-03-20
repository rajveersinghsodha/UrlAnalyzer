using UrlAnalyzer.DTOs;

namespace UrlAnalyzer.Services;

public interface IUrlAnalyzerService
{
    Task<UrlAnalysisResponse> AnalyzeUrlAsync(string url, CancellationToken cancellationToken = default);
} 