namespace MyClassLib.SubNamespace
{
    /// <summary>
    /// Generic class.
    /// </summary>
    /// <typeparam name="T">The type param.</typeparam>
    public class GenericClass<T> where T : new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericClass" /> class.
        /// </summary>
        public GenericClass() { }

        /// <summary>
        /// Gets a new instance of generic param.
        /// </summary>
        /// <typeparam name="T">The generic param.</typeparam>
        /// <returns>The new instance.</returns>
        public T GetGenericInstance<T>() where T : new()
        {
            return new T();
        }
    }
}