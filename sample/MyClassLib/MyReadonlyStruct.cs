namespace MyClassLib;

/// <summary>
/// A readonly struct sample.
/// </summary>
public readonly struct MyReadonlyStruct
{
    /// <summary>
    /// Initializes a new <see cref="MyReadonlyStruct"/>.
    /// </summary>
    /// <param name="value">The wrapped value.</param>
    public MyReadonlyStruct(int value)
    {
        this.Value = value;
    }

    /// <summary>
    /// The wrapped value.
    /// </summary>
    /// <value>The integer value.</value>
    public int Value { get; }
}
