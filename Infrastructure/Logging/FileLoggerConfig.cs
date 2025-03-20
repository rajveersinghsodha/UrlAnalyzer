namespace UrlAnalyzer.Infrastructure.Logging;

public class FileLoggerConfig
{
    public string FilePath { get; set; } = "Logs/app.log";
    public LogLevel MinLevel { get; set; } = LogLevel.Information;
} 