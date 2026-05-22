using Converter.Legacy;
using Converter.New;

namespace Converter.Mapping;


public static class NiDaqSettingMapper
{
    public static NiDaqSettings Map(LegacyNiDaqSettings legacyNiDaqSettings)
    {
        return new NiDaqSettings
        {
            //Wells = legacyNiDaqSettings.Wells?.Select(WellSettingsMapper.Map).ToArray() ?? [],
            UToPFactor = legacyNiDaqSettings.UToPFactor,
            NumberOfSamples = legacyNiDaqSettings.NumberOfSamples ?? 120, //maybe it shouldn't assume deafult
            SkipLowCount = legacyNiDaqSettings.SkipLowCount,
            SkipHighCount = legacyNiDaqSettings.SkipHighCount,
            SamplingInterval = legacyNiDaqSettings.SamplingInterval,
            HardwareSwitchTime = legacyNiDaqSettings.HardwareSwitchTime,
            MeasurementMode = legacyNiDaqSettings.MeasurementMode
        };
    }
}