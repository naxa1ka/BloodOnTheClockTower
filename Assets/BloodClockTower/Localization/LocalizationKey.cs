public class LocalizationKey : ILocalizationKey
{
    public string Key { get; }

    public LocalizationKey(string key) => Key = key;
}