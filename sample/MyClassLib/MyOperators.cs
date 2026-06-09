namespace MyClassLib;

/// <summary>
/// A class exercising operator overloads.
/// </summary>
public class MyOperators
{
    /// <summary>
    /// The wrapped value.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Initializes a new <see cref="MyOperators"/>.
    /// </summary>
    /// <param name="value">The wrapped value.</param>
    public MyOperators(int value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Adds two operands.
    /// </summary>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The sum.</returns>
    public static MyOperators operator +(MyOperators a, MyOperators b) => new(a.Value + b.Value);

    /// <summary>
    /// Subtracts two operands.
    /// </summary>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>The difference.</returns>
    public static MyOperators operator -(MyOperators a, MyOperators b) => new(a.Value - b.Value);

    /// <summary>
    /// Equality.
    /// </summary>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>True when values are equal.</returns>
    public static bool operator ==(MyOperators a, MyOperators b) => a.Value == b.Value;

    /// <summary>
    /// Inequality.
    /// </summary>
    /// <param name="a">The left operand.</param>
    /// <param name="b">The right operand.</param>
    /// <returns>True when values are not equal.</returns>
    public static bool operator !=(MyOperators a, MyOperators b) => a.Value != b.Value;

    /// <summary>
    /// Implicit conversion to <see cref="int"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator int(MyOperators value) => value.Value;

    /// <summary>
    /// Explicit conversion from <see cref="int"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static explicit operator MyOperators(int value) => new(value);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is MyOperators other && this.Value == other.Value;

    /// <inheritdoc/>
    public override int GetHashCode() => this.Value.GetHashCode();
}
