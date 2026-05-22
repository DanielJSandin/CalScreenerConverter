namespace Converter.Serialization;

public interface ISettingsReader<T>
{
    T TryRead(string filePath);
}