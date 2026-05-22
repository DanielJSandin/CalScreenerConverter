using System.ComponentModel;
using Converter.Shared;

namespace Converter.New;

public class WellSettings
{
    
    private AdCalibrationDataPoint[]? calibrationPoints = Array.Empty<AdCalibrationDataPoint>();
    private CrosstalkCalibrationDataPoint[]? crosstalkCalibrationPoints = Array.Empty<CrosstalkCalibrationDataPoint>();

    [DefaultValue(null)]
    public AdCalibrationDataPoint[] AdCalibrationPoints 
    {
        get
        {
            calibrationPoints ??= Array.Empty<AdCalibrationDataPoint>();
            return calibrationPoints;
        }
        set
        {
            if (calibrationPoints != value)
            {
                calibrationPoints = value;
            }
        }
    }


 
    // Im reusing the same as the legacy class, as they have the same schema.
    [DefaultValue(null)]
    LegacyCalibrationPoint[]? CalibrationPoints {get; set;}
    
    public CrosstalkCalibrationDataPoint[] CrosstalkCalibrationPoints 
    { get
        {
            crosstalkCalibrationPoints ??= Array.Empty<CrosstalkCalibrationDataPoint>();
            return crosstalkCalibrationPoints;
        } 
        set
        {
            if (crosstalkCalibrationPoints != value)
            {
                crosstalkCalibrationPoints = value;
            }
        }
    }

    public int ChannelIndex { get; set; }

    //I dont get the gain here, as multiple calibration points can have different gains, 
    // it doesnt seem to give the average of them either
    public double Gain { get; set; }

    public double Offset { get; set; }

    public int ReferenceWellIndex { get; set; }


    //The should be a CalibrationValidationPoint here, as that exists in 3.1 but its never showed in the jsons, so for now I wont
    //bother with it until further notice
}