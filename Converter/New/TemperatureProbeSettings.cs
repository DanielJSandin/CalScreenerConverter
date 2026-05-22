namespace Converter.New;

public class TemperatureProbeSettings
{
    public SerialPortSettings? SerialPortSettings { get; set; }

    public TemperatureProbeSettings()
    {
        SerialPortSettings = new SerialPortSettings();
    }
}