using System.Xml.Serialization;

namespace Converter.Legacy.Model;

public class LegacyWellSettings
{
    public int ChannelIndex { get; set; }

    public double Gain { get; set; }

    public double Offset { get; set; }

    public int ReferenceWellIndex { get; set; }

    [XmlArrayItem("CalibrationPoint")]
    public LegacyCalibrationPoint[]? CalibrationPoints { get; set; }
}