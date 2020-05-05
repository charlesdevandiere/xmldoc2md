namespace MyClassLib
{
    /// <summary>
    /// My class.
    /// </summary>
    public class MyClass : IMyInterface
    {
        /// <summary>
        /// My property.
        /// </summary>
        /// <value>The property value.</value>
        public string MyProperty { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyClass" /> class.
        /// </summary>
        public MyClass() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyClass" /> class.
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
        /// Gets some thing.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>An empty string.</returns>
        /// <exception cref="System.Exception">Thrown when...</exception>
        public string Get(string param) => string.Empty;
    }
}
