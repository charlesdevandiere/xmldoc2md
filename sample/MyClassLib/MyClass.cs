using System;
using System.Collections.Generic;

namespace MyClassLib;

/// <summary>
/// My class.
/// </summary>
/// <remarks>A remark.</remarks>
public class MyClass : IMyInterface
{
    /// <summary>
    /// My field.
    /// </summary>
    public int myField;

    /// <summary>
    /// My property.
    /// </summary>
    /// <value>The property value. Used by <see cref="DoGeneric{T}(T)"/>.</value>
    public string MyProperty { get; protected set; }

    /// <summary>
    /// My nullable property
    /// </summary>
    /// <value>The nullable property value.</value>
    public int? MyNullableProperty { get; set; }

    /// <summary>
    /// My enum
    /// </summary>
    /// <value>The enum value</value>
    public MyEnum MyEnum { get; set; }

    /// <summary>
    /// My delegate.
    /// </summary>
    /// <param name="str">The string param.</param>
    public delegate void MyDelegate(string str);

    /// <summary>
    /// My event.
    /// </summary>
    public event EventHandler<EventArgs> MyEvent;

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
    /// <list type="bullet">
    ///     <item>
    ///         <term>item 1</term>
    ///         <description>The first item. <c>1</c></description>
    ///     </item>
    ///     <item>
    ///         <term>item 2</term>
    ///         <description>The second item. <c>2</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public MyClass() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyClassLib.MyClass" /> class.<br/>
    /// Use <paramref name="firstParam"/> and <paramref name="secondParam"/>.
    /// </summary>
    /// <param name="firstParam">The first param.</param>
    /// <param name="secondParam">The second param.</param>
    public MyClass(string firstParam, int secondParam) { }

    /// <summary>
    /// Do some thing.
    /// </summary>
    /// <param name="firstParam">The first param.</param>
    /// <param name="secondParam">The second param.</param>
    /// <exception cref="System.Exception">Thrown when...</exception>
    public void Do(string firstParam, int secondParam) { }

    /// <summary>
    /// Do some thing.
    /// </summary>
    /// <typeparam name="T">The type argument. Used by <see cref="DoGeneric{T}(T)"/>.</typeparam>
    /// <param name="value">The param. Used by <see cref="DoGeneric{T}(T)"/>.</param>
    /// <returns>Returns a value <see cref="int"/>.</returns>
    /// <exception cref="ArgumentException">Thrown instead of <see cref="Exception"/>.</exception>
    public int DoGeneric<T>(T value) { throw new ArgumentException(); }

    /// <summary>
    /// Gets some thing.
    /// </summary>
    /// <param name="param">The param.</param>
    /// <returns>An empty string.</returns>
    /// <exception cref="System.Exception">Thrown when...</exception>
    public string Get(List<string> param) => string.Empty;

    /// <summary>
    /// Try to get some thing.
    /// </summary>
    /// <param name="param">The param.</param>
    /// <param name="result">The output param.</param>
    /// <returns> true if the function succeeded false otherwise</returns>
    public bool TryGet(long param, out string result)
    {
        result = string.Empty;
        return true;
    }

    /// <summary>
    /// A static method.
    /// </summary>
    public static void StaticMethod() { }

    #region private members

    /// <summary>
    /// My field.
    /// </summary>
    private int myPrivateField;

    /// <summary>
    /// My property.
    /// </summary>
    /// <value>The property value. Used by <see cref="PrivateDoGeneric{T}(T)"/>.</value>
    private string MyPrivateProperty { get; set; }

    /// <summary>
    /// My enum
    /// </summary>
    /// <value>The enum value</value>
    private MyEnum MyPrivateEnum { get; set; }

    /// <summary>
    /// My delegate.
    /// </summary>
    /// <param name="str">The string param.</param>
    private delegate void MyPrivateDelegate(string str);

    /// <summary>
    /// My event.
    /// </summary>
    private event EventHandler<EventArgs> MyPrivateEvent;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyClassLib.MyClass" /> class.
    /// </summary>
    private MyClass(short @short) { }

    /// <summary>
    /// Do some thing.
    /// </summary>
    /// <param name="firstParam">The first param.</param>
    /// <param name="secondParam">The second param.</param>
    /// <exception cref="System.Exception">Thrown when...</exception>
    private void PrivateDo(string firstParam, int secondParam) { }

    /// <summary>
    /// Do some thing.
    /// </summary>
    /// <typeparam name="T">The type argument. Used by <see cref="PrivateDoGeneric{T}(T)"/>.</typeparam>
    /// <param name="value">The param. Used by <see cref="PrivateDoGeneric{T}(T)"/>.</param>
    /// <returns>Returns a value <see cref="int"/>.</returns>
    /// <exception cref="ArgumentException">Thrown instead of <see cref="Exception"/>.</exception>
    private int PrivateDoGeneric<T>(T value) { throw new ArgumentException(); }

    /// <summary>
    /// Gets some thing.
    /// </summary>
    /// <param name="param">The param.</param>
    /// <returns>An empty string.</returns>
    /// <exception cref="System.Exception">Thrown when...</exception>
    private string PrivateGet(List<string> param) => string.Empty;

    /// <summary>
    /// A static method.
    /// </summary>
    private static void PrivateStaticMethod() { }

    #endregion

    #region internal members

    /// <summary>
    /// My field.
    /// </summary>
    internal int myInternalField;

    /// <summary>
    /// My property.
    /// </summary>
    /// <value>The property value. Used by <see cref="InternalDoGeneric{T}(T)"/>.</value>
    internal string MyInternalProperty { get; set; }

    /// <summary>
    /// My enum
    /// </summary>
    /// <value>The enum value</value>
    internal MyEnum MyInternalEnum { get; set; }

    /// <summary>
    /// My delegate.
    /// </summary>
    /// <param name="str">The string param.</param>
    internal delegate void MyInternalDelegate(string str);

    /// <summary>
    /// My event.
    /// </summary>
    internal event EventHandler<EventArgs> MyInternalEvent;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyClassLib.MyClass" /> class.
    /// </summary>
    internal MyClass(long @long) { }

    /// <summary>
    /// Do some thing.
    /// </summary>
    /// <param name="firstParam">The first param.</param>
    /// <param name="secondParam">The second param.</param>
    /// <exception cref="System.Exception">Thrown when...</exception>
    internal void InternalDo(string firstParam, int secondParam) { }

    /// <summary>
    /// Do some thing.
    /// </summary>
    /// <typeparam name="T">The type argument. Used by <see cref="InternalDoGeneric{T}(T)"/>.</typeparam>
    /// <param name="value">The param. Used by <see cref="InternalDoGeneric{T}(T)"/>.</param>
    /// <returns>Returns a value <see cref="int"/>.</returns>
    /// <exception cref="ArgumentException">Thrown instead of <see cref="Exception"/>.</exception>
    internal int InternalDoGeneric<T>(T value) { throw new ArgumentException(); }

    /// <summary>
    /// Gets some thing.
    /// </summary>
    /// <param name="param">The param.</param>
    /// <returns>An empty string.</returns>
    /// <exception cref="System.Exception">Thrown when...</exception>
    internal string InternalGet(List<string> param) => string.Empty;

    /// <summary>
    /// A static method.
    /// </summary>
    internal static void InternalStaticMethod() { }

    #endregion

    #region protected members

    /// <summary>
    /// My field.
    /// </summary>
    protected int myProtectedField;

    /// <summary>
    /// My property.
    /// </summary>
    /// <value>The property value. Used by <see cref="ProtectedDoGeneric{T}(T)"/>.</value>
    protected string MyProtectedProperty { get; set; }

    /// <summary>
    /// My enum
    /// </summary>
    /// <value>The enum value</value>
    protected MyEnum MyProtectedEnum { get; set; }

    /// <summary>
    /// My delegate.
    /// </summary>
    /// <param name="str">The string param.</param>
    protected delegate void MyProtectedDelegate(string str);

    /// <summary>
    /// My event.
    /// </summary>
    protected event EventHandler<EventArgs> MyProtectedEvent;

    /// <summary>
    /// Initializes a new instance of the <see cref="MyClassLib.MyClass" /> class.
    /// </summary>
    protected MyClass(int @int) { }

    /// <summary>
    /// Do some thing.
    /// </summary>
    /// <param name="firstParam">The first param.</param>
    /// <param name="secondParam">The second param.</param>
    /// <exception cref="System.Exception">Thrown when...</exception>
    protected void ProtectedDo(string firstParam, int secondParam) { }

    /// <summary>
    /// Do some thing.
    /// </summary>
    /// <typeparam name="T">The type argument. Used by <see cref="ProtectedDoGeneric{T}(T)"/>.</typeparam>
    /// <param name="value">The param. Used by <see cref="ProtectedDoGeneric{T}(T)"/>.</param>
    /// <returns>Returns a value <see cref="int"/>.</returns>
    /// <exception cref="ArgumentException">Thrown instead of <see cref="Exception"/>.</exception>
    protected int ProtectedDoGeneric<T>(T value) { throw new ArgumentException(); }

    /// <summary>
    /// Gets some thing.
    /// </summary>
    /// <param name="param">The param.</param>
    /// <returns>An empty string.</returns>
    /// <exception cref="System.Exception">Thrown when...</exception>
    protected string ProtectedGet(List<string> param) => string.Empty;

    /// <summary>
    /// A static method.
    /// </summary>
    protected static void ProtectedStaticMethod() { }

    #endregion
}
