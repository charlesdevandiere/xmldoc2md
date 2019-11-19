namespace MyClassLib
{
    /// <summary>
    /// Class 1
    /// </summary>
    public class Class1
    {
        /// <summary>
        /// The foo.
        /// </summary>
        /// <value>The foo.</value>
        public string Foo { get; set; }

        /// <summary>
        /// The bar.
        /// </summary>
        /// <value>The bar.</value>
        public int Bar { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Class1" /> class.
        /// </summary>
        public Class1() { }

        /// <summary>
        /// Do some thing.
        /// </summary>
        /// <param name="paramOne">The param one.</param>
        /// <param name="paramTwo">The param two</param>
        public void DoSomeThing(string paramOne, int paramTwo) { }

        /// <summary>
        /// Gets some thing else.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>An empty string.</returns>
        public string GetSomeThingElse(string param)
        {
            return string.Empty;
        }
    }
}
