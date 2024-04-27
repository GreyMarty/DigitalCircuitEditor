namespace Editor.Core.Extensions;

public static class DictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    {
        if (!dict.TryGetValue(key, out var result))
        {
            result = Activator.CreateInstance<TValue>();
            dict[key] = result;
        }

        return result;
    }
}