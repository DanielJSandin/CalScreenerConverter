using System.ComponentModel;
using Converter.Shared;

namespace Converter.New;

public class NidaqSettings
{
    public WellSettings[]? Wells { get; set; }

    public double UToPFactor { get; set; }

    [DefaultValue(120)]
    public int NumberOfSamples { get; set; }

    public int SkipLowCount { get; set; }

    public int SkipHighCount { get; set; }

    [DefaultValue(1000)]
    public int SamplingInterval { get; set; }

    public int HardwareSwitchTime { get; set; }

    public CalScreenerMeasurementMode MeasurementMode { get; set; }
}