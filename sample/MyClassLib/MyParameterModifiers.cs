namespace MyClassLib;

/// <summary>
/// Showcases <c>ref</c>, <c>in</c>, <c>out</c>, <c>params</c> parameter modifiers
/// and default parameter values.
/// </summary>
public class MyParameterModifiers
{
    /// <summary>
    /// Increments a value by reference.
    /// </summary>
    /// <param name="value">The reference-passed value.</param>
    public void RefIncrement(ref int value)
    {
        value++;
    }

    /// <summary>
    /// Reads a value via <c>in</c>.
    /// </summary>
    /// <param name="value">The read-only reference.</param>
    /// <returns>The value squared.</returns>
    public int Read(in int value) => value * value;

    /// <summary>
    /// Tries to parse a value, writing the parsed result via <c>out</c>.
    /// </summary>
    /// <param name="input">The text to parse.</param>
    /// <param name="value">The parsed value.</param>
    /// <returns><see langword="true"/> on success.</returns>
    public bool TryParse(string input, out int value) => int.TryParse(input, out value);

    /// <summary>
    /// Concatenates a variable number of items.
    /// </summary>
    /// <param name="items">The items to join.</param>
    /// <returns>The joined string.</returns>
    public string Join(params string[] items) => string.Join(", ", items);

    /// <summary>
    /// Shows several default-value flavours.
    /// </summary>
    /// <param name="name">A string default.</param>
    /// <param name="count">A numeric default.</param>
    /// <param name="ratio">A double default.</param>
    /// <param name="flag">A bool default.</param>
    /// <param name="kind">An enum default.</param>
    /// <param name="note">A nullable-reference default of <see langword="null"/>.</param>
    public void Defaults(
        string name = "anakin",
        int count = 42,
        double ratio = 1.5,
        bool flag = true,
        MyEnum kind = MyEnum.First,
        string? note = null)
    {
    }
}
