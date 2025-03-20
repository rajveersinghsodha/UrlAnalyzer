namespace UrlAnalyzer.Infrastructure.Logging;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly FileLoggerConfig _config;

    public FileLoggerProvider(FileLoggerConfig config)
    {
        _config = config;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger(categoryName, _config);
    }

    public void Dispose()
    {
    }
} 