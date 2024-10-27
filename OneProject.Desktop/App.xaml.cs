namespace OneProject.Desktop;

using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using OneProject.Desktop.Infrastructures;
using OneProject.Desktop.Theme;
using Serilog;
using Serilog.Core;
using Serilog.Events;

public partial class App : Application
{
    private const string Name = "OneProject.Desktop";
    private const string MemoryMappedFileName = $"Global\\OneProject.Desktop.MemoryMappedFile.ProcessId";

    private const string OutputTemplate =
        "{Timestamp:yyyy-MM-dd HH:mm:ss} {SourceContext}[{Level:u3}]: {Message:lj}{NewLine}{Exception}";

    private static bool IsAlreadyRunning;
    private static Mutex? _mutex;
    private static MemoryMappedFile? _memoryFile;
    public App()
    {
        _mutex = new Mutex(true, Name, out var createdNew);

        IsAlreadyRunning = !createdNew;

        Services = ConfigureServices();

        var resource = Current.Resources;

        InitializeComponent();

        DispatcherUnhandledException += OnUnhandledException;
    }

    public static new App Current => (App)Application.Current;

    public IServiceProvider Services { get; }

    private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        => Log.Logger.Error("{0}", e.Exception);

    protected override void OnStartup(StartupEventArgs e)
    {
        if(IsAlreadyRunning)
        {
            if(TryShowMainWindow())
            {
                return;
            }
        }

        base.OnStartup(e);

        // 默认指定硬件加速
        RenderOptions.ProcessRenderMode = RenderMode.Default;

        GlobalSettings.Load(Log.Logger);

        SaveProcessId();

        var theme = GlobalSettings.Instance.Theme;

        ThemeManager.Initialize(this, theme.Background, theme.IsLight);

        Log.Logger.Information($"Application Start, Args: {e.Args.JoinAsString()}");

        Task.Run(() => Services.GetRequiredService<VersionChecker>().GetLatestVersionAsync());

        Task.Run(() => WindowsThemeListener.Listen(Current));
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _mutex?.ReleaseMutex();

        _memoryFile?.Dispose();

        Log.Logger.Information($"Application Exit, Exit Code: {e.ApplicationExitCode}");

        Log.CloseAndFlush();

        base.OnExit(e);
    }

    /// <summary>
    /// 创建共享内存块，保存进程ID
    /// </summary>
    private static void SaveProcessId()
    {
        var processId = Environment.ProcessId;

        _memoryFile = MemoryMappedFile.CreateNew(MemoryMappedFileName, 32 * 1024);

        using var vs = _memoryFile.CreateViewStream();

        var writer = new BinaryWriter(vs, Encoding.UTF8);

        writer.Write(processId);
    }

    /// <summary>
    /// 读取内存块，获取进程ID，打开主窗口
    /// </summary>
    /// <returns></returns>
    private static bool TryShowMainWindow()
    {
        try
        {
            using var memoryReaeder = MemoryMappedFile.OpenExisting(MemoryMappedFileName);

            var reader = memoryReaeder.CreateViewAccessor();

            var id = reader.ReadInt32(0);

            var process = Process.GetProcessById(id);

            if(process is null)
            {
                return false;
            }

            ShowWindowAsync(process.MainWindowHandle, 1);

            SetForegroundWindow(process.MainWindowHandle);

            Environment.Exit(1);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static ServiceProvider ConfigureServices()
    {
        Log.Logger = ConfigureLogger();

        var services = new ServiceCollection();

        services.AddHttpClient();

        //services.AddLogging(builder =>
        //{
        //    builder.AddSerilog(Log.Logger);
        //});

        //services.AddDownload();

        services.AddTransient<VersionChecker>();

        return services.BuildServiceProvider();
    }

    private static Logger ConfigureLogger()
        => new LoggerConfiguration()
            .Enrich.FromLogContext()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .WriteTo.Async(config =>
            {
                config.Conditional(c => c.Level == LogEventLevel.Debug, sink =>
                {
                    sink.File("Logs/log_debug_.log", outputTemplate: OutputTemplate,
                        rollingInterval: RollingInterval.Day);
                })
                    .WriteTo.Conditional(c => c.Level == LogEventLevel.Information, sink =>
                    {
                        sink.File("Logs/log_information_.log", outputTemplate: OutputTemplate,
                            rollingInterval: RollingInterval.Day);
                    })
                    .WriteTo.Conditional(c => c.Level == LogEventLevel.Warning, sink =>
                    {
                        sink.File("Logs/log_warning_.log", outputTemplate: OutputTemplate,
                            rollingInterval: RollingInterval.Day);
                    })
                    .WriteTo.Conditional(c => c.Level == LogEventLevel.Error, sink =>
                    {
                        sink.File("Logs/log_error_.log", outputTemplate: OutputTemplate,
                            rollingInterval: RollingInterval.Day);
                    })
                    .WriteTo.Conditional(c => c.Level == LogEventLevel.Fatal, sink =>
                    {
                        sink.File("Logs/log_fatal_.log", outputTemplate: OutputTemplate,
                            rollingInterval: RollingInterval.Day);
                    });
            })
            .CreateLogger();

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShowWindowAsync(nint hwnd, int cmdShow);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetForegroundWindow(nint hwnd);
}
