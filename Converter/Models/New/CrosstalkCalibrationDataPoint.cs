using System.Xml.Serialization;

namespace Converter.New;

public record CrosstalkCalibrationDataPoint(
        string PlateKey,
        Guid CalibrationId,
        double Temperature,
        [property:XmlArrayItem( ElementName="Value")]
        double[]? GainFactors,
        [property:XmlArrayItem(ElementName ="Value")]
        double[]? TimeConstants,
        string Comment,
        bool IsIncluded,
        DateTimeOffset TimeStamp)
{
    
}