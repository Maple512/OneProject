namespace OneProject.Desktop;

using System.Runtime.InteropServices;

[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public record GlobalSettings
{
    private const string FileName = "settings.json";

    public static GlobalSettings Instance { get; private set; } = null!;
    //public static string FullFileName = Path.Combine(Directory, "settings.json");

    //public static string DownloadPath => Path.Combine(Directory, "Downloads");
    [JsonIgnore]
    public bool IsLoadFile { get; private set; }

    public static string TempPath => Path.Combine(Root, "Temps");

    public static string Root = null!;

    public static string FilePath = null!;

    public static string Version = null!;

    public WindowSettings Windows { get; set; } = new();

    public ThemeSettings Theme { get; set; } = new();

    public void Save()
    {
        var content = JsonSerializer.Serialize(this, GlobalSettingsContext.Default.GlobalSettings);

        File.WriteAllText(FilePath, content);
    }

    public static void Load(ILogger logger, string? directory = null)
    {
        Root = directory ?? AppDomain.CurrentDomain.BaseDirectory;
        Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion!;
        FilePath = Path.Combine(Root, FileName);

        logger.Information($"Root: {Root}");
        logger.Information($"Version: {Version}");
        logger.Information($"OS: {Environment.OSVersion}, {RuntimeInformation.OSArchitecture}, {Environment.MachineName}");
        logger.Information($"Process: {Environment.ProcessId}, {Environment.ProcessorCount}");
        logger.Information($"NET: {RuntimeInformation.FrameworkDescription}, {RuntimeInformation.ProcessArchitecture}");

        if(File.Exists(FilePath) == false)
        {
            Instance = new();

            Instance.Save();

            return;
        }

        var json = File.ReadAllText(FilePath);

        Instance = JsonSerializer.Deserialize(json, GlobalSettingsContext.Default.GlobalSettings)
                   ?? new GlobalSettings();

        Instance.IsLoadFile = true;
    }

    public class WindowSettings
    {
        public WindowState MainWindowState { get; set; }

        public double MainWindowHeight { get; set; } = 600D;

        public double MainWindowWidth { get; set; } = 1000D;

        public double MainWindowLeft { get; set; }

        public double MainWindowTop { get; set; }

        public string? FontFamily { get; set; }
    }

    public class ThemeSettings
    {
        public bool IsLight { get; set; }

        public string? Background { get; set; }
    }
}

[JsonSourceGenerationOptions(WriteIndented = true, PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(GlobalSettings))]
public partial class GlobalSettingsContext : JsonSerializerContext
{
}
