namespace Converter.Mapping;

public static class PlateMapper
{

    //lazy array mapping for {key, name}
    private static string[] glass = {"calPlatePlusGlass", "calPlate Pro glass"};
    private static string[] titan = {"standard", "Standard" };
    private static string[] titanxTalk = {"calPlatePlus", "calPlate Pro"};

    public static string MapPlateKey(string legacyPlateName)
    {
        legacyPlateName = legacyPlateName.ToLower();
        string? earlyReturn = CheckMatchingReturnName(legacyPlateName, 0);
        if (earlyReturn != null) return earlyReturn;

        return legacyPlateName switch
        {
            "glass" => glass[0],
            "titan" => titan[0],
            "placeholder" => titanxTalk[0],
            _ => throw new ArgumentException("Cant recognice plate name: " + legacyPlateName)
        };
    }

    public static string MapPlateName(string legacyPlateName)
    {   
        legacyPlateName = legacyPlateName.ToLower();
        string? earlyReturn = CheckMatchingReturnName(legacyPlateName, 1);
        if (earlyReturn != null) return earlyReturn;

        
        return legacyPlateName switch
        {
            "glass" => glass[1],
            "titan" => titan[1],
            "placeholder" => titanxTalk[1],
            _ => throw new ArgumentException("Cant recognice plate name: " + legacyPlateName)
        };
    }

    private static string? CheckMatchingReturnName(string legacyPlateName, int indexToReturn)
    {
        if (titan[0].ToLower() == legacyPlateName)
            return titan[indexToReturn];
        if (glass[0].ToLower() == legacyPlateName)
            return glass[indexToReturn];
        if (titanxTalk[0].ToLower() == legacyPlateName)
            return titanxTalk[indexToReturn];
        return null;
    }
}