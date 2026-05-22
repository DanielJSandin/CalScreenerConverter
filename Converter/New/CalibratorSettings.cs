namespace Converter.New;

public class CalibratorSettings
{
    public SerialPortSettings? SerialPortSettings { get; set; }

    public CalibratorSettings()
    {
        SerialPortSettings = new SerialPortSettings();
    }
}