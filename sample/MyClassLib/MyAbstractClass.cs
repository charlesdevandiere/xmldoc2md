using System.Collections.Generic;

namespace MyClassLib
{
    /// <summary>
    /// My abstract class.
    /// </summary>
    public abstract class MyAbstractClass
    {
        /// <summary>
        /// My abstract property.
        /// </summary>
        /// <value>The property value.</value>
        public abstract int MyProperty { get; set; }

        /// <summary>
        /// Do something.
        /// </summary>
        public abstract void Do();

        /// <summary>
        /// Gets something.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns>A string.</returns>
        protected string Get(List<string> param) => string.Empty;
    }
}
