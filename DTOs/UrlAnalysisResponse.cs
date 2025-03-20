namespace UrlAnalyzer.DTOs;

public class UrlAnalysisResponse
{
    public List<ImageInfo> Images { get; set; } = new();
    public int TotalWordCount { get; set; }
    public List<WordFrequency> TopWords { get; set; } = new();
    public string? Error { get; set; }
}

public class ImageInfo
{
    public string Url { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
}

public class WordFrequency
{
    public string Word { get; set; } = string.Empty;
    public int Count { get; set; }
} 