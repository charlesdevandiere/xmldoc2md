namespace MyClassLib;

/// <summary>
/// My interface.
/// </summary>
public interface IMyInterface
{
    /// <summary>
    /// Do some thing.
    /// </summary>
    /// <param name="firstParam">The first param.</param>
    /// <param name="secondParam">The second param.</param>
    void Do(string firstParam, int secondParam);

    /// <summary>
    /// Gets some thing.
    /// </summary>
    /// <param name="param">The param.</param>
    /// <returns>An empty string.</returns>
    string Get(List<string> param);

    /// <summary>
    /// A default interface method — has a body, callers can use it as-is.
    /// </summary>
    /// <returns>A greeting.</returns>
    string Greet() => "hello";
}

/// <summary>
/// An interface with a <c>static abstract</c> member and a default static implementation.
/// </summary>
/// <typeparam name="T">The implementing type.</typeparam>
public interface INumeric<T> where T : INumeric<T>
{
    /// <summary>
    /// A static abstract operator — implementers must provide an addition operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The sum.</returns>
    static abstract T operator +(T left, T right);

    /// <summary>
    /// A static abstract factory.
    /// </summary>
    /// <returns>A zero value.</returns>
    static abstract T Zero();

    /// <summary>
    /// A default static interface method.
    /// </summary>
    /// <param name="x">The value.</param>
    /// <returns>The squared value.</returns>
    static virtual T Square(T x) => x + x;
}
