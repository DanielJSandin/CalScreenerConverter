namespace Converter.Serialization;

public interface ISettingsWriter<T>
{
    void Write(T settings, string filePath);
}