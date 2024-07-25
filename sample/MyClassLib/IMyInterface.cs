using System.Collections.Generic;

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
}
