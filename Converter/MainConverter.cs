using Converter.Legacy;
using Converter.Mapping;
using Converter.New;

public static class MainConverter
{
    public static CalScreenerDeviceSettings Convert(LegacyWrapper[] legacyWrappers, ConverterOptions converterOptions)
    {
        //we are making the assumption that all the shared variables between the merging legacysettings should be the same
        //otherwise we should throw an error

        CalScreenerDeviceSettings calScreenerDeviceSettings = new CalScreenerDeviceSettings
        {
            NiDaqSettings = MergeAndConvertNiDaqSettings(legacyWrappers),
            ThermostatSettings = new ThermostatSettings(),
            CalibratorSettings = new CalibratorSettings(),
            TemperatureProbeSettings = new TemperatureProbeSettings(),
            ActivePlateKey = null, //dont know yet
            ActivePlateName = null, //dont know yet
            AvailablePlates = ["standard", "calPlatePlusGlass"], //I think that is the only possible ones, but not sure what to write yet
            ObjectId = Guid.NewGuid(), // we can just set our own
            //todo: device id should be the same as filename we create
            DeviceId = CreateNewDeviceId(legacyWrappers[0].NidaqSettings.DeviceId, converterOptions),
            DeviceName = legacyWrappers[0].NidaqSettings.DeviceName // we can just take the first one since they should all be the same
        };



        return calScreenerDeviceSettings;
    }


    private static NiDaqSettings MergeAndConvertNiDaqSettings(LegacyWrapper[] legacyWrappers)
    {
        for (int i = 1; i < legacyWrappers.Length; i++)
        {
            if (!legacyWrappers[i].NidaqSettings.EnsureIdenticalExceptWells(legacyWrappers[i-1].NidaqSettings))
            {
                throw new ArgumentException("Convertation Failed, XML files differ in shared settings");
            }
        }
        NiDaqSettings nidaqSettings =  NiDaqSettingMapper.Map(legacyWrappers[0].NidaqSettings);

        nidaqSettings.Wells = WellSettingsMapper.Map(legacyWrappers);

        return nidaqSettings;
    }

    private static string GetVariantFromUserInputIfNeeded(string deviceId, ConverterOptions converterOptions)
    {

        if (converterOptions.IgnoreEmptyVariant)
            return string.Empty;

        Console.Write($"Variant for group '{deviceId}' (blank allowed): ");
        return Console.ReadLine() ?? "";
    }

    private static string CreateNewDeviceId(string deviceId, ConverterOptions converterOptions)
    {
        string variant = converterOptions.Variant ?? GetVariantFromUserInputIfNeeded(deviceId, converterOptions);
        return converterOptions.FileNamePrefix + variant + "-" + deviceId.Split('-').Last(); //we want just the serialnumber 
    }
    
}