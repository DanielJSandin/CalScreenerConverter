using Converter.Legacy;
using Converter.Mapping;
using Converter.New;

public static class MainConverter
{
    public static CalScreenerDeviceSettings Convert(LegacyNiDaqSettings[] legacyNiDaqSettingsArray)
    {
        //we are making the assumption that all the shared variables between the merging legacysettings should be the same
        //otherwise we should throw an error

        CalScreenerDeviceSettings calScreenerDeviceSettings = new CalScreenerDeviceSettings
        {
            NidaqSettings = MergeSharedNiDaqSettings(legacyNiDaqSettingsArray),
            ThermostatSettings = new ThermostatSettings(legacyNiDaqSettingsArray[0].SamplingInterval),
            CalibratorSettings = new CalibratorSettings(),
            
        };

    }


    private static NidaqSettings MergeSharedNiDaqSettings(LegacyNiDaqSettings[] legacyNiDaqSettingsArray)
    {
        for (int i = 1; i < legacyNiDaqSettingsArray.Length; i++)
        {
            if (!legacyNiDaqSettingsArray[i].EnsureIdenticalExceptWells(legacyNiDaqSettingsArray[i-1]))
            {
                throw new ArgumentException("Convertation Failed, XML files differ in shared settings");
            }
        }
        return NiDaqSettingMapper.Map(legacyNiDaqSettingsArray[0]);
    }
}