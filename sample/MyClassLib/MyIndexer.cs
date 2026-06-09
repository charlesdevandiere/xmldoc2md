namespace MyClassLib;

/// <summary>
/// A class with indexers.
/// </summary>
public class MyIndexer
{
    private readonly Dictionary<string, int> store = new();

    /// <summary>
    /// Gets or sets a value by integer index.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <value>The value at <paramref name="index"/>.</value>
    public int this[int index]
    {
        get => index;
        set => _ = value;
    }

    /// <summary>
    /// Gets a value by string key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <value>The stored value.</value>
    public int this[string key] => this.store.TryGetValue(key, out int v) ? v : 0;
}
