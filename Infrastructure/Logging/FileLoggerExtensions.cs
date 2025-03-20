namespace UrlAnalyzer.Infrastructure.Logging;

public static class FileLoggerExtensions
{
    public static ILoggingBuilder AddFile(this ILoggingBuilder builder, Action<FileLoggerConfig> configure)
    {
        var config = new FileLoggerConfig();
        configure(config);
        builder.AddProvider(new FileLoggerProvider(config));
        return builder;
    }
} 