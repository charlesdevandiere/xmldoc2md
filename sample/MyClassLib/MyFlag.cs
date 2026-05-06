namespace MyClassLib;

/// <summary>
/// My enum.
/// </summary>
[Flags]
public enum MyFlag
{
    /// <summary>
    /// The default
    /// </summary>
    Default = 0,

    /// <summary>
    /// The first.
    /// </summary>
    First = 1,

    /// <summary>
    /// The second.
    /// </summary>
    Second = 2,

    /// <summary>
    /// The Third. Combines First | Second semantics.
    /// </summary>
    Third = 4
}
