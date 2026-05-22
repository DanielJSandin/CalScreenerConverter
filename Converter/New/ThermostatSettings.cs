namespace Converter.New;

public class ThermostatSettings
{
    public double? SamplingIntervalMs { get; set; }

    public ModbusClientSettings? ModbusClientSettings { get; set; }

    public SerialPortSettings? SerialPortSettings { get; set; }
}