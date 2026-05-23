namespace Converter.New;

public class ThermostatSettings
{
    public double? SamplingIntervalMs { get; set; }

    public ModbusClientSettings? ModbusClientSettings { get; set; }

    public SerialPortSettings? SerialPortSettings { get; set; }

    public ThermostatSettings(double samplingIntervalMs)
    {
        SamplingIntervalMs = samplingIntervalMs;
        ModbusClientSettings = new ModbusClientSettings();
        SerialPortSettings = new SerialPortSettings();
    }
    public ThermostatSettings()
    {
        ModbusClientSettings = new ModbusClientSettings();
        SerialPortSettings = new SerialPortSettings();
    }
}