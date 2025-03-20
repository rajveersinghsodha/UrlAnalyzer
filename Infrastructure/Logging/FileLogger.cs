using System.Collections.Concurrent;

namespace UrlAnalyzer.Infrastructure.Logging;

public class FileLogger : ILogger
{
    private readonly string _name;
    private readonly FileLoggerConfig _config;
    private static readonly ConcurrentDictionary<string, object> _locks = new();

    public FileLogger(string name, FileLoggerConfig config)
    {
        _name = name;
        _config = config;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _config.MinLevel;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var logDirectory = Path.GetDirectoryName(_config.FilePath);
        if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        var lockObject = _locks.GetOrAdd(_config.FilePath, _ => new object());

        var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{logLevel}] [{_name}] {formatter(state, exception)}";
        if (exception != null)
        {
            logEntry += $"\nException: {exception}\nStackTrace: {exception.StackTrace}";
        }
        logEntry += "\n";

        lock (lockObject)
        {
            try
            {
                File.AppendAllText(_config.FilePath, logEntry);
            }
            catch
            {
                // Suppress file write errors
            }
        }
    }
} 