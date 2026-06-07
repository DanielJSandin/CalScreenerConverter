using Converter.New;
using Converter.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        ConverterOptions converterOptions = ConverterOptions.CreateFromArgs(args);
        
        if (!Directory.Exists(converterOptions.InputFolder))
        {
            System.Console.Error.WriteLine($"Error: Input folder does not exist: {converterOptions.InputFolder}");
            Exit(1);
        }

        if (!Directory.Exists(converterOptions.OutputFolder))
        {
            Directory.CreateDirectory(converterOptions.OutputFolder);
        }

        List<LegacyWrapper[]> legacyWrapperGroups = LoadLegacyGroups(converterOptions.InputFolder);
        
        if (legacyWrapperGroups.Count == 0)
        {
            System.Console.Error.WriteLine($"Error: No XML files found in: {converterOptions.InputFolder}");

            Exit(1);
        }

        System.Console.Out.WriteLine($"Loaded {legacyWrapperGroups.Count} groups of legacy settings for conversion.");

        List<CalScreenerDeviceSettings> convertedSettings = new List<CalScreenerDeviceSettings>();

        if (converterOptions.Variant is not null && legacyWrapperGroups.Count > 1)
        {
            System.Console.Out.WriteLine("Variant is set and will be applied for multiple groups.");
        }
        foreach (LegacyWrapper[] legacyWrapperGroup in legacyWrapperGroups)
        {
            convertedSettings.Add(MainConverter.Convert(legacyWrapperGroup, converterOptions));
        }
        System.Console.Out.WriteLine($"Converted {convertedSettings.Count} groups of legacy settings.");

        foreach (CalScreenerDeviceSettings calScreenerDeviceSettings in convertedSettings)
        {
            string outputFilePath = Path.Combine(converterOptions.OutputFolder, $"{calScreenerDeviceSettings.DeviceId}.json");
            CalScreenerDeviceSettingsWriter writer = new CalScreenerDeviceSettingsWriter();
            writer.Write(calScreenerDeviceSettings, outputFilePath);
            System.Console.Out.WriteLine($"Written converted settings to {outputFilePath}");
        }

        Exit(0);

    }

    private static void Exit(int exitCode)
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        Environment.Exit(exitCode);
    }

    /// <summary>
    /// This method is supposed to read the input directory and group the legacy files into groups that should be converted together,
    /// They are grouped by the device name in the file name, this expects that all files follow the same pattern
    /// </summary>
    private static List<List<string>> GetConvertGroups(String inputDirectory)
    { 
        var groups = new Dictionary<string, List<string>>();

        var filepaths = Directory.GetFiles(inputDirectory, "*.xml",  SearchOption.AllDirectories).ToList();
        foreach (string filepath in filepaths)
        {
            string deviceId = CreateDeviceId(filepath);

            if (!groups.ContainsKey(deviceId))
            {
                groups.Add(deviceId, new List<string>());
            }
            groups[deviceId].Add(filepath);
        }
        return groups.Values.ToList();
    }

    private static string CreateDeviceId(String filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        //we assume that the legacy files are start with Ni-DAQmx-
        return fileName.Split(' ')[0].Replace("NI-DAQmx-", "");
    }

    private static List<LegacyWrapper[]> LoadLegacyGroups(string inputDirectory)
    {
        LegacySettingReader legacySettingReader = new LegacySettingReader();
        List<List<string>> groups = GetConvertGroups(inputDirectory);

        List<LegacyWrapper[]> legacyConvertGroups = new List<LegacyWrapper[]>();

        foreach (List<string> group in groups)
        {
            try
            {
                int groupSize = group.Count;

                LegacyWrapper[] legacyConvertGroup = new LegacyWrapper[groupSize];

                for (int i = 0; i < groupSize; i++)
                {
                    legacyConvertGroup[i] = legacySettingReader.TryRead(group[i]);
                }
                legacyConvertGroups.Add(legacyConvertGroup);
            } catch (Exception ex)
            {
                System.Console.Out.WriteLine(ex.Message);
                System.Console.Out.WriteLine("Skipping group");
            }
        }
        return legacyConvertGroups;
    }
}
