namespace MyClassLib;

/// <summary>
/// Showcases nullable reference types and named-tuple element rendering.
/// </summary>
public class MyNullableClass
{
    /// <summary>
    /// A nullable-reference property.
    /// </summary>
    public string? OptionalName { get; set; }

    /// <summary>
    /// A non-nullable-reference property (annotation should NOT add a <c>?</c>).
    /// </summary>
    public string RequiredName { get; set; } = string.Empty;

    /// <summary>
    /// A value-type Nullable property — renders as <c>int?</c>, not <c>Nullable&lt;int&gt;</c>.
    /// </summary>
    public int? Counter { get; set; }

    /// <summary>
    /// A field with a nullable generic type argument.
    /// </summary>
    public IEnumerable<string?>? OptionalNames;

    /// <summary>
    /// Returns a named tuple.
    /// </summary>
    /// <returns>A named (X, Y) pair.</returns>
    public (int X, int Y) Origin() => (0, 0);

    /// <summary>
    /// Accepts a named tuple parameter and an unnamed tuple parameter.
    /// </summary>
    /// <param name="point">A named pair.</param>
    /// <param name="pair">A positional pair.</param>
    /// <returns>A descriptive label.</returns>
    public string Describe((string Name, int Age) point, (int, int) pair) => point.Name;

    /// <summary>
    /// Nullable-reference parameter and nullable-reference return.
    /// </summary>
    /// <param name="input">Possibly null input.</param>
    /// <returns>Possibly null output.</returns>
    public string? Echo(string? input) => input;
}
