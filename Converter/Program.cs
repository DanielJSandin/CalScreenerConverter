public class Program
{
    public static void Main(string[] args)
    {
        String inputDirPath = "Input";
        
        List<LegacyWrapper[]> legacyWrapperGroups = LoadLegacyGroups(Path.Combine(AppContext.BaseDirectory, inputDirPath));
        System.Console.Out.WriteLine($"Loaded {legacyWrapperGroups.Count} groups of legacy settings for conversion.");

    }


    /// <summary>
    /// This method is supposed to read the input directory and group the legacy files into groups that should be converted together,
    /// They are grouped by the device name in the file name, this expects that all files follow the same pattern
    /// </summary>
    private static List<List<string>> GetConvertGroups(String inputDirectory)
    { 
        var groups = new Dictionary<string, List<string>>();

        var filepaths = Directory.GetFiles(inputDirectory, "*.xml").ToList();
        foreach (string filepath in filepaths)
        {
            string deviceId = GetDeviceId(filepath);

            if (!groups.ContainsKey(deviceId))
            {
                groups.Add(deviceId, new List<string>());
            }
            groups[deviceId].Add(filepath);
        }
        return groups.Values.ToList();
    }

    private static string GetDeviceId(String filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
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
