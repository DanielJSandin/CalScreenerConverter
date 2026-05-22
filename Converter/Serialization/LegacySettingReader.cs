using System.Xml.Serialization;
using Converter.Legacy;

public class LegacySettingReader : ISettingsReader<LegacyWrapper>
{
    public LegacyWrapper TryRead(string filePath)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LegacyNiDaqSettings));
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                if (serializer.Deserialize(fileStream) is not LegacyNiDaqSettings nidaqSettings)
                {
                    throw new InvalidDataException($"Deserialization returned null for file: {filePath}");
                }
                return new LegacyWrapper(nidaqSettings, filePath);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to read legacy settings from {filePath}", ex);
        }
    }
}