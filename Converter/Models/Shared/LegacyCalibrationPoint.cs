namespace Converter.Shared;

public class LegacyCalibrationPoint
{

    /// <summary>
    /// Seems like this class is used both by legacy and new, but it hasnt been shown in neither the legacy xmls nor the new jsons,
    /// so for now I will just assume it is a clean mapping between the two, if they were to appear in an xml
    /// </summary>

    public bool IsIncluded { get; set; }

    public Guid CalibrationId { get; set; }

    //Im unsure if these Gains will reflect what the Wellsetting has directly in the legacy, smae with offset, but we will see I guess
    public double? Gain { get; set; }

    public double? Offset { get; set; }


    //The question is if this comment should be mapped of the CrosstalkCalibrationPoints comment as well.
    public string? Comment { get; set; }

    public DateTime TimeStamp { get; set; }
}