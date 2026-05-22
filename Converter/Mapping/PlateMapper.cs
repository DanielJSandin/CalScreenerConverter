namespace Converter.Mapping;

public static class PlateMapper
{
    public static string MapPlateKey(string legacyPlateName)
    {
        return legacyPlateName switch
        {
            "glass" => "calPlatePlusGlass",
            "titan" => "standard",
            _ => "standard",//standard makes sense as fallback, TODO, go back and look at this later
        };
    }

    public static string MapPlateName(string legacyPlateName)
    {
        return legacyPlateName switch
        {
            "glass" => "calPlate Pro glass",
            "titan" => "standard",
            _ => "standard",//standard makes sense as fallback, TODO, go back and look at this later
        };
    }
}