namespace Converter.Legacy;

public class LegacyCalibrationPoint
{
    public bool IsIncluded { get; set; }

    public Guid CalibrationId { get; set; }

    public double? Gain { get; set; }

    public double? Offset { get; set; }

    public string Comment { get; set; }

    public DateTime TimeStamp { get; set; }
}