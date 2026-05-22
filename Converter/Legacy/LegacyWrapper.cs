using System.Globalization;
using System.Text.RegularExpressions;
using Converter.Legacy;

/// <summary>
/// My own wrapper class to wrap the legacy settings, as well to present other data needed that is extracted from the file name
/// </summary>
public class LegacyWrapper
{
    public LegacyNiDaqSettings NidaqSettings { get; set; }

    public double Temperature { get; set; }

    public string? PlateKey { get; set; }

    public LegacyWrapper(LegacyNiDaqSettings nidaqSettings, String fileName)
    {
        NidaqSettings = nidaqSettings;

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Filename cannot be empty.", nameof(fileName));

        //we might have to check if this is the only file name format we have, but for now 
        //I assume it, cause I haven't seen of other naming styles

        var match = Regex.Match(fileName,
            @"NI-DAQmx-\d+\s+(?<temp>\d+(\.\d+)?)\s+(?<plate>\w+)",
            RegexOptions.IgnoreCase);

        if (!match.Success)
            throw new FormatException($"Filename format is invalid: '{fileName}'");

        Temperature = double.Parse(match.Groups["temp"].Value, CultureInfo.InvariantCulture);
        PlateKey = match.Groups["plate"].Value;
    
    }
}