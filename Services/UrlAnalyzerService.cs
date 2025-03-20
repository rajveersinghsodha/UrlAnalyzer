using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using UrlAnalyzer.DTOs;

namespace UrlAnalyzer.Services;
/// <summary>
/// Service for analyzing a URL, extracting images, and determining word frequency from the given URL
/// </summary>
public class UrlAnalyzerService : IUrlAnalyzerService
{
    private readonly ILogger<UrlAnalyzerService> _logger;
    private readonly HttpClient _httpClient;

    // Define the HTML tags we want to extract text from
    private static readonly HashSet<string> ContentTags = new(StringComparer.OrdinalIgnoreCase)
    {
        "p", "h1", "h2", "h3", "h4", "h5", "h6",
        "div", "span", "article", "section", "main",
        "li", "td", "th", "caption", "label", "button"
    };


    public UrlAnalyzerService(ILogger<UrlAnalyzerService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<UrlAnalysisResponse> AnalyzeUrlAsync(string url)
    {
        try
        {
            _logger.LogInformation("Starting analysis of URL: {Url}", url);


            // Sends an HTTP GET request to the specified URL and waits for the response
            var response = await _httpClient.GetAsync(url);

            // Ensures the HTTP response is successful (status code 200-299)
            // If the response indicates failure (e.g., 404 Not Found, 500 Server Error),
            // this method will throw an exception.
            response.EnsureSuccessStatusCode();


            // Reads the HTTP response content as a string (HTML content of the requested page)
            var html = await response.Content.ReadAsStringAsync();

            // Creates a new instance of HtmlDocument (from HtmlAgilityPack) to parse the HTML
            var doc = new HtmlDocument();

            // Loads the retrieved HTML content into the HtmlDocument object for further processing
            doc.LoadHtml(html);

            // Extracts all image URLs from the parsed HTML document using the provided base URL
            var images = ExtractImages(doc, url);

         
            // Returns the total word count and a list of the most frequently used words
            var (wordCount, topWords) = AnalyzeText(doc);

            _logger.LogInformation("Successfully analyzed URL: {Url}", url);

            return new UrlAnalysisResponse
            {
                Images = images,
                TotalWordCount = wordCount,
                TopWords = topWords
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing URL: {Url}", url);
            return new UrlAnalysisResponse
            {
                Error = "Failed to analyze URL. Please ensure the URL is valid and accessible."
            };
        }
    }

    private List<ImageInfo> ExtractImages(HtmlDocument doc, string baseUrl)
    {
        var images = new List<ImageInfo>();
        // nodes which have img
        var imgNodes = doc.DocumentNode.SelectNodes("//img");

        if (imgNodes == null) return images;

        foreach (var img in imgNodes)
        {
            var src = img.GetAttributeValue("src", "");
            if (string.IsNullOrWhiteSpace(src)) continue;

            // Convert relative URLs to absolute, getting image with alt tag
            if (Uri.TryCreate(new Uri(baseUrl), src, out var absoluteUri))
            {
                images.Add(new ImageInfo
                {
                    Url = absoluteUri.ToString(),
                    AltText = img.GetAttributeValue("alt", "")
                });
            }
        }

        return images;
    }

    private (int TotalCount, List<WordFrequency> TopWords) AnalyzeText(HtmlDocument doc)
    {
        // Remove script and style tags first
        var scriptsAndStyles = doc.DocumentNode.SelectNodes("//script|//style");
        if (scriptsAndStyles != null)
        {
            foreach (var node in scriptsAndStyles)
            {
                node.Remove();
            }
        }

        // Build XPath to select only content tags
        var xPath = string.Join("|", ContentTags.Select(tag => $"//{tag}"));
        var contentNodes = doc.DocumentNode.SelectNodes(xPath);

        if (contentNodes == null)
        {
            return (0, new List<WordFrequency>());
        }

        var words = contentNodes
            .Select(node => node.InnerText)
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .SelectMany(text => Regex.Split(text.ToLower(), @"\W+"))
            .Where(word =>
                !string.IsNullOrWhiteSpace(word) &&
                word.Length > 1 &&
               !word.All(char.IsDigit)) // Exclude numbers
            .ToList();

        var wordFrequencies = words
            .GroupBy(w => w)
            .Select(g => new WordFrequency { Word = g.Key, Count = g.Count() })
            .OrderByDescending(wf => wf.Count)
            .Take(10)
            .ToList();

        return (words.Count, wordFrequencies);
    }
}