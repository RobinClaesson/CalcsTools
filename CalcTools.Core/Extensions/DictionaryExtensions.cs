namespace CalcTools.Core.Extensions;

public static class DictionaryExtensions
{
    public static void UpdateKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey oldKey, TKey newKey)
        where TKey : notnull
    {
        if (oldKey.Equals(newKey))
            return;

        var value = dictionary[oldKey];
        dictionary.Remove(oldKey);
        dictionary[newKey] = value;
    }
}