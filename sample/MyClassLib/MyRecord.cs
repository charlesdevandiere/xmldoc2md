namespace MyClassLib;

/// <summary>
/// A record class representing a person.
/// </summary>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
public record Person(string FirstName, string LastName)
{
    /// <summary>
    /// Gets the full name.
    /// </summary>
    public string FullName => $"{this.FirstName} {this.LastName}";
}

/// <summary>
/// A record struct representing a 2D point.
/// </summary>
/// <param name="X">The X coordinate.</param>
/// <param name="Y">The Y coordinate.</param>
public readonly record struct Point(int X, int Y);
