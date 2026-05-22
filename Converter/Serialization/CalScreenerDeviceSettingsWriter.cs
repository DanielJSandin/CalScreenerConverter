using System.Text.Json;
using Converter.New;

namespace Converter.Serialization;

public class CalScreenerDeviceSettingsWriter : ISettingsWriter<CalScreenerDeviceSettings>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };
    public void Write(CalScreenerDeviceSettings settings, string filePath)
    {
        try
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                System.Text.Json.JsonSerializer.Serialize(fileStream, settings, JsonOptions);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to write CalScreenerDeviceSettings to {filePath}", ex);
        }
    }
}