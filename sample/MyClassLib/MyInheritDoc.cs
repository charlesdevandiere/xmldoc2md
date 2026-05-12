namespace MyClassLib;

/// <summary>
/// Base type for the <c>&lt;inheritdoc/&gt;</c> demos.
/// </summary>
/// <remarks>Defines the documented members the derived types inherit from.</remarks>
public abstract class InheritDocBase
{
    /// <summary>
    /// Processes an input value.
    /// </summary>
    /// <param name="value">The input value.</param>
    /// <returns>The processed value.</returns>
    /// <exception cref="System.ArgumentException">Thrown when <paramref name="value"/> is negative.</exception>
    public abstract int Process(int value);

    /// <summary>
    /// The configured name.
    /// </summary>
    /// <value>The display name.</value>
    public abstract string Name { get; set; }
}

/// <summary>
/// Demonstrates <c>&lt;inheritdoc/&gt;</c> on overridden members.
/// </summary>
public class InheritDocDerived : InheritDocBase
{
    /// <inheritdoc/>
    public override int Process(int value) => value * 2;

    /// <inheritdoc/>
    public override string Name { get; set; } = string.Empty;

    /// <summary>
    /// Keeps its own summary but inherits the rest from <see cref="InheritDocBase.Process(int)"/>.
    /// </summary>
    /// <inheritdoc cref="InheritDocBase.Process(System.Int32)"/>
    public int ProcessTwice(int value) => value * 4;
}

/// <summary>
/// Interface used for <c>&lt;inheritdoc/&gt;</c> implementation demos.
/// </summary>
public interface IInheritDocSource
{
    /// <summary>
    /// Looks up an item by key.
    /// </summary>
    /// <param name="key">The lookup key.</param>
    /// <returns>The matched item, or an empty string.</returns>
    string Lookup(string key);
}

/// <summary>
/// Demonstrates <c>&lt;inheritdoc/&gt;</c> on an interface implementation and an
/// explicit <c>&lt;inheritdoc cref="..."/&gt;</c> pointing at an unrelated member.
/// </summary>
public class InheritDocImpl : IInheritDocSource
{
    /// <inheritdoc/>
    public string Lookup(string key) => key;

    /// <inheritdoc cref="InheritDocBase.Process(System.Int32)"/>
    public int ReuseFromCref(int value) => value;
}
