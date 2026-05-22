using Converter.New;

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
            calibrationIds[i] = new Guid(); //dont know if i should use potential existing calibrationIds
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
                CrosstalkCalibrationPoints = CreateCrosstalkCalibrationDataPoints(legacyWrappers, i, calibrationIds)
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
            double[] gainFactors = new double[48];
            gainFactors[channelIndex] = legacyWrappers[i].NidaqSettings.Wells[channelIndex].Gain;
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
}