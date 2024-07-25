using System;
using System.Collections.Generic;

namespace MyClassLib;

/// <summary>
/// My obsolete class.
/// </summary>
/// <remarks>A remark.</remarks>
[Obsolete("Deprecated, use MyClass instead.")]
public class MyObsoleteClass
{
    /// <summary>
    /// My field.
    /// </summary>
    [Obsolete]
    public int myField;

    /// <summary>
    /// My property.
    /// </summary>
    /// <value>The property value.</value>
    [Obsolete]
    public string MyProperty { get; protected set; }


    /// <summary>
    /// Protected property.
    /// </summary>
    /// <value>The property value.</value>
    protected string ProtectedProperty { get; set; }

    /// <summary>
    /// Instances.
    /// </summary>
    public int MyProperty2 { get; set; }

    /// <summary>
    /// My prop1;
    /// </summary>
    private int MyProperty1 { get; set; }


    /// <summary>
    /// This is my Static property.
    /// </summary>
    public static int StaticProperty { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyClassLib.MyClass" /> class.
    /// </summary>
    /// <remarks>
    /// See also <see cref="MyClassLib.MyClass.MyClass(string, int)" />.
    /// <code>
    /// if (true)
    /// {
    ///     var foo = new MyClass("foo", 1);
    ///     Console.WriteLine(foo.ToString());
    /// }
    /// </code>
    /// </remarks>
    [Obsolete]
    public MyObsoleteClass() { }

    /// <summary>
    /// Gets some thing.
    /// </summary>
    /// <param name="param">The param.</param>
    /// <returns>An empty string.</returns>
    /// <exception cref="System.Exception">Thrown when...</exception>
    [Obsolete]
    public string Get(List<string> param) => string.Empty;

    /// <summary>
    /// A static method.
    /// </summary>
    [Obsolete("Deprecated in favor of MyClass.")]
    public static void StaticMethod() { }
}
