using System.Xml.Serialization;
using Converter.Shared;

namespace Converter.Legacy;

[XmlRoot("NiDaqSettings", Namespace = "http://symcel.se/CalView")]
public class LegacyNiDaqSettings
{
    public string? DeviceName { get; set; }

    public string? DeviceId { get; set; }

    [XmlArrayItem("Well")]
    public LegacyWellSettings[]? Wells { get; set; }

    public double UToPFactor { get; set; }

    public int? NumberOfSamples { get; set; }

    public int SkipLowCount { get; set; }

    public int SkipHighCount { get; set; }

    public int SamplingInterval { get; set; }

    public int HardwareSwitchTime { get; set; }

    public CalScreenerMeasurementMode MeasurementMode { get; set; }
    

    public bool EnsureIdenticalExceptWells(LegacyNiDaqSettings other)
    {
        return DeviceName == other.DeviceName &&
               DeviceId == other.DeviceId &&
               UToPFactor == other.UToPFactor &&
               NumberOfSamples == other.NumberOfSamples &&
               SkipLowCount == other.SkipLowCount &&
               SkipHighCount == other.SkipHighCount &&
               SamplingInterval == other.SamplingInterval &&
               HardwareSwitchTime == other.HardwareSwitchTime &&
               MeasurementMode == other.MeasurementMode;
    }
}