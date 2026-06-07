using Converter.New;
using Converter.Shared;

namespace Converter.Mapping;

public static class WellSettingsMapper
{
    public static WellSettings[] Map(LegacyWrapper[] legacyWrappers)
    {
        int wellAmount = 48;

        WellSettings[] wellSettingsArray = new WellSettings[wellAmount];

        Guid[] calibrationIds = new Guid[legacyWrappers.Length];
        for (int i = 0; i < legacyWrappers.Length; i++)
        {
            calibrationIds[i] = Guid.NewGuid(); //dont know if i should use potential existing calibrationIds
            //in the legacy well settings calibrationpoints, so for now i just make a new one
        }

        for (int i = 0; i < wellAmount; i++)
        {
            wellSettingsArray[i] = new WellSettings
            {
                ChannelIndex = i,
                Offset = 0, //dont know how to convert this, maybe it should be 0
                Gain = 1, //we do them like they seem to be
                ReferenceWellIndex = -1,
                CrosstalkCalibrationPoints = CreateCrosstalkCalibrationDataPoints(legacyWrappers, i, calibrationIds),
                CalibrationPoints = CreateCalibrationPoints(legacyWrappers, i)
            };
        }


        return wellSettingsArray;
    }

    private static CrosstalkCalibrationDataPoint[] CreateCrosstalkCalibrationDataPoints(
        LegacyWrapper[] legacyWrappers, int channelIndex, Guid[] calibrationIds)
    {
        CrosstalkCalibrationDataPoint[] crosstalkCalibrations = new CrosstalkCalibrationDataPoint[legacyWrappers.Length];

        for (int i = 0; i < legacyWrappers.Length; i++)
        {
            var nidaqSettings = legacyWrappers[i].NidaqSettings ?? throw new InvalidDataException($"NidaqSettings is null for a wrapper with" 
            + "device id {legacyWrappers[i].NidaqSettings.DeviceId}");
            var wells = nidaqSettings.Wells ?? throw new InvalidDataException($"Wells is null for wrapper with device id "+
            "{legacyWrappers[i].NidaqSettings.DeviceId}");
            
            if (channelIndex >= wells.Length)
            {
                throw new InvalidDataException($"Channel index {channelIndex} out of bounds for wrapper with device id " + 
                " {legacyWrappers[i].NidaqSettings.DeviceId}. Wells length: {wells.Length}");
            }

            double[] gainFactors = new double[48];
            gainFactors[channelIndex] = wells[channelIndex].Gain;
            crosstalkCalibrations[i] = new CrosstalkCalibrationDataPoint(
                PlateMapper.MapPlateKey(legacyWrappers[i].PlateKey),
                calibrationIds[i], //for now we assume all are the same for all wells, but the json i have suggest different on 
                // standard plates
                legacyWrappers[i].Temperature,
                gainFactors,
                new double[40],
                "AUTO Generated Comment: This CrosstalkCalibrationPoint was generated from a 2.0 version calibrationfile",
                true, //dont really know exactly what that refers to
                DateTimeOffset.Now
            );
        }

        return crosstalkCalibrations;
    }

    private static LegacyCalibrationPoint[]? CreateCalibrationPoints(LegacyWrapper[] legacyWrappers, int channelIndex)
    {
        List<LegacyCalibrationPoint> calibrationPoints = new List<LegacyCalibrationPoint>();


        foreach (LegacyWrapper legacyWrapper in legacyWrappers)
        {
            var nidaqSettings = legacyWrapper.NidaqSettings ?? throw new InvalidDataException($"NidaqSettings is null for a wrapper with" 
            + "device id {legacyWrapper.NidaqSettings.DeviceId}");
            var wells = nidaqSettings.Wells ?? throw new InvalidDataException($"Wells is null for wrapper with device id "+
            "{legacyWrapper.NidaqSettings.DeviceId}");
            
            if (channelIndex >= wells.Length)
            {
                throw new InvalidDataException($"Channel index {channelIndex} out of bounds for wrapper with device id " + 
                " {legacyWrapper.NidaqSettings.DeviceId}. Wells length: {wells.Length}");
            }

            var legacyCalibrationPoints = wells[channelIndex].CalibrationPoints;
            if (legacyCalibrationPoints != null)
            {
                calibrationPoints.AddRange(legacyCalibrationPoints);
            }
        }
        return calibrationPoints.Count > 0 ? calibrationPoints.ToArray() : null;
    }
}