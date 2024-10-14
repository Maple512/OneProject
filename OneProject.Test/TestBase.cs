namespace OneProject;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

public abstract class TestBase<TLogger>
{
    private const string OutputTemlpate =
        "{Timestamp:yyyy-MM-dd HH:mm:ss} {SourceContext}[{Level:u3}]: {Message:lj}{NewLine}{Exception}";

    protected TestBase()
    {
        var services = new ServiceCollection();

        services.AddLogging(builder =>
        {
            builder.AddDebug()
                .AddSerilog(ConfigureLogger());
        });

        ConfigureServices(services);

        var configurationBuilder = new ConfigurationBuilder();

        ConfigureConfiguration(configurationBuilder);

        Configuration = configurationBuilder.Build();

        Services = services.BuildServiceProvider();

        Logger = Services.GetRequiredService<ILogger<TLogger>>();
    }

    protected IServiceProvider Services { get; }

    protected IConfiguration Configuration { get; }

    protected ILogger Logger { get; }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
    }

    protected virtual void ConfigureConfiguration(IConfigurationBuilder builder)
    {
    }

    protected IEnumerable<T> GetServices<T>() => Services.GetServices<T>();

    protected T? GetService<T>() => Services.GetService<T>();

    protected T GetRequiredService<T>()
        where T : notnull
        => Services.GetRequiredService<T>();

    private static Logger ConfigureLogger()
        => new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Debug()
            .WriteTo.Async(config =>
            {
                config.Conditional(c => c.Level == LogEventLevel.Debug, sink =>
                {
                    sink.File("Logs/log_debug.log", outputTemplate: OutputTemlpate,
                        rollingInterval: RollingInterval.Day);
                })
                    .WriteTo.Conditional(c => c.Level == LogEventLevel.Information, sink =>
                    {
                        sink.File("Logs/log_information.log", outputTemplate: OutputTemlpate,
                            rollingInterval: RollingInterval.Day);
                    })
                    .WriteTo.Conditional(c => c.Level == LogEventLevel.Warning, sink =>
                    {
                        sink.File("Logs/log_warning.log", outputTemplate: OutputTemlpate,
                            rollingInterval: RollingInterval.Day);
                    })
                    .WriteTo.Conditional(c => c.Level == LogEventLevel.Error, sink =>
                    {
                        sink.File("Logs/log_error.log", outputTemplate: OutputTemlpate,
                            rollingInterval: RollingInterval.Day);
                    })
                    .WriteTo.Conditional(c => c.Level == LogEventLevel.Fatal, sink =>
                    {
                        sink.File("Logs/log_fatal.log", outputTemplate: OutputTemlpate,
                            rollingInterval: RollingInterval.Day);
                    });
            })
            .CreateLogger();
}
