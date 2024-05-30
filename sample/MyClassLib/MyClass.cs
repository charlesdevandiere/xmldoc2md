using System;
using System.Collections.Generic;

namespace MyClassLib
{
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
        /// My private field.
        /// </summary>
        private int myPrivateField;

        /// <summary>
        /// My property.
        /// </summary>
        /// <value>The property value. Used by <see cref="DoGeneric{T}(T)"/>.</value>
        public string MyProperty { get; protected set; }

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
        /// My private event.
        /// </summary>
        private event EventHandler<EventArgs> MyPrivateEvent;

        /// <summary>
        /// Protected property.
        /// </summary>
        /// <value>The property value.</value>
        protected string ProtectedProperty { get; set; }

        /// <summary>
        /// Private property.
        /// </summary>
        /// <value>The property value.</value>
        private string PrivateProperty { get; set; }

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
        public MyClass()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyClassLib.MyClass" /> class.
        /// </summary>
        /// <param name="firstParam">The first param.</param>
        /// <param name="secondParam">The second param.</param>
        public MyClass(string firstParam, int secondParam)
        { }

        /// <summary>
        /// Private initializer for a new instance of the <see cref="MyClassLib.MyClass" /> class.
        /// </summary>
        /// <param name="firstParam">The first param.</param>
        private MyClass(string firstParam)
        {
        }

        /// <summary>
        /// Do some thing.
        /// </summary>
        /// <param name="firstParam">The first param.</param>
        /// <param name="secondParam">The second param.</param>
        /// <exception cref="System.Exception">Thrown when...</exception>
        public void Do(string firstParam, int secondParam)
        { }

        /// <summary>
        /// Do some thing.
        /// </summary>
        /// <typeparam name="T">The type argument. Used by <see cref="DoGeneric{T}(T)"/>.</typeparam>
        /// <param name="value">The param. Used by <see cref="DoGeneric{T}(T)"/>.</param>
        /// <returns>Returns a value <see cref="int"/>.</returns>
        /// <exception cref="ArgumentException">Thrown instead of <see cref="Exception"/>.</exception>
        public int DoGeneric<T>(T value)
        { throw new ArgumentException(); }

        /// <summary>
        /// Gets some thing.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>An empty string.</returns>
        /// <exception cref="System.Exception">Thrown when...</exception>
        public string Get(List<string> param) => string.Empty;

        /// <summary>
        /// A static method.
        /// </summary>
        public static void StaticMethod()
        { }

        /// <summary>
        /// A private method.
        /// </summary>
        private void PrivateWork()
        { }
    }
}
