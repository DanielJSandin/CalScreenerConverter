using Converter.Legacy;
using Converter.Mapping;
using Converter.New;

public static class MainConverter
{
    public static CalScreenerDeviceSettings Convert(LegacyWrapper[] legacyWrappers, ConverterOptions converterOptions)
    {
        if (legacyWrappers == null || legacyWrappers.Length == 0)
        {
            throw new ArgumentException("legacyWrappers cannot be null or empty");
        }

        //we are making the assumption that all the shared variables between the merging legacysettings should be the same
        //otherwise we should throw an error

        var firstWrapper = legacyWrappers[0];
        if (firstWrapper?.NidaqSettings == null)
        {
            throw new InvalidDataException("First wrapper or its NidaqSettings is null");
        }

        var deviceId = firstWrapper.NidaqSettings.DeviceId ?? throw new InvalidDataException("DeviceId is null");
        var deviceName = firstWrapper.NidaqSettings.DeviceName ?? throw new InvalidDataException("DeviceName is null");

        CalScreenerDeviceSettings calScreenerDeviceSettings = new CalScreenerDeviceSettings
        {
            NiDaqSettings = MergeAndConvertNiDaqSettings(legacyWrappers),
            ThermostatSettings = new ThermostatSettings(),
            CalibratorSettings = new CalibratorSettings(),
            TemperatureProbeSettings = new TemperatureProbeSettings(),
            ActivePlateKey = null, //CalView can set it up
            ActivePlateName = null, //CalView can set it up
            AvailablePlates = null, //If i read the calsys code correct it should initialize it correctly itself 
            //["standard", "calPlatePlusGlass"], //I think that is the only possible ones, but not sure what to write yet
            ObjectId = Guid.NewGuid(), // we can just set our own
            DeviceId = CreateNewDeviceId(deviceId, converterOptions),
            DeviceName = deviceName
        };



        return calScreenerDeviceSettings;
    }


    private static NiDaqSettings MergeAndConvertNiDaqSettings(LegacyWrapper[] legacyWrappers)
    {
        for (int i = 1; i < legacyWrappers.Length; i++)
        {
            if (!legacyWrappers[i].NidaqSettings.EnsureIdenticalExceptWells(legacyWrappers[i-1].NidaqSettings))
            {
                throw new ArgumentException($"Convertation Failed for files with id '{legacyWrappers[i].NidaqSettings.DeviceId}', XML files differ in shared settings");
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