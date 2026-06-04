public class ConverterOptions
{
    public string InputFolder { get; set; } = Path.Combine(AppContext.BaseDirectory, "Input");

    public string OutputFolder { get; set; } = Path.Combine(AppContext.BaseDirectory, "Output");

    public string? Variant { get; set; }

    public bool IgnoreEmptyVariant { get; set; }

    public bool DeleteSuccessfulInputFiles { get; set; }

    public string FileNamePrefix { get; set; } = "Symcel.CalSys.CalScreener-";

    public static ConverterOptions CreateFromArgs(string[] args)
    {
        ConverterOptions options = new ConverterOptions();

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i].ToLowerInvariant())
            {
                case "--input":
                    options.InputFolder = GetValue(args, ref i);
                    break;
                case "--output":
                    options.OutputFolder = GetValue(args, ref i);
                    break;
                case "--variant":
                    options.Variant = GetValue(args, ref i);
                    break;
                case "--ignore-empty-variant":
                    options.IgnoreEmptyVariant = true;
                    break;
                case "--delete-successful-input-files":
                    options.DeleteSuccessfulInputFiles = true;
                    break;
                case "--file-name-prefix":
                    options.FileNamePrefix = GetValue(args, ref i);
                    break;
                default:
                    throw new ArgumentException($"Unknown argument: {args[i]}");
            }
        }
        if (options.Variant is not null && options.IgnoreEmptyVariant)
            throw new ArgumentException("--variant and --ignore-empty-variant cannot be used together.");

        return options;
    }

    private static string GetValue(string[] args, ref int i)
    {
        if (i + 1 >= args.Length)
        throw new ArgumentException($"Missing value for {args[i]}");
        return args[++i];
    }
}