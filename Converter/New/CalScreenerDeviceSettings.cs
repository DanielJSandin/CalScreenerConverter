namespace Converter.New;

/// <summary>
/// Supposedly from what I understand, this is the root of the json file for 3.1.7 format of the CalScreener file
/// </summary>
public class CalScreenerDeviceSettings
{
    public NidaqSettings? NidaqSettings { get; set; }

    public ThermostatSettings? ThermostatSettings { get; set; }

    public CalibratorSettings? CalibratorSettings { get; set; }

    public TemperatureProbeSettings? TemperatureProbeSettings { get; set; }

    public string? ActivePlateKey { get; set; }

    public string? ActivePlateName { get; set; }

    public string[]? AvailablePlates { get; set; }



    public Guid ObjectId { get; set; }

    public string? DeviceId { get; set; }

    public string? DeviceName { get; set; }

}