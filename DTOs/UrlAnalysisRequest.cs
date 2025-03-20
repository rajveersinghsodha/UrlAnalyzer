using System.ComponentModel.DataAnnotations;

namespace UrlAnalyzer.DTOs;

public class UrlAnalysisRequest
{
    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;
} 