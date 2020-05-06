namespace MyClassLib
{
    /// <summary>
    /// My abstract class.
    /// </summary>
    public abstract class MyAbstractClass
    {
        /// <summary>
        /// Do something.
        /// </summary>
        public abstract void Do();

        /// <summary>
        /// Gets something.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>A string.</returns>
        protected string Get(string param) => string.Empty;
    }
}
