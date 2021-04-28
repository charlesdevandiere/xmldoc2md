namespace MyClassLib.SubNamespace
{
    /// <summary>
    /// Generic class.
    /// </summary>
    /// <typeparam name="T">The type param.</typeparam>
    public class GenericClass<T> where T : new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericClass{T}" /> class.
        /// </summary>
        public GenericClass() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericClass{T}" /> class.
        /// </summary>
        /// <param name="param">The generic parameter.</param>
        public GenericClass(T param) { }

        /// <summary>
        /// Gets a new instance of generic param.
        /// </summary>
        /// <typeparam name="TSource">The generic param.</typeparam>
        /// <returns>The new instance.</returns>
        public TSource GetGenericInstance<TSource>() where TSource : new()
        {
            return new TSource();
        }
        
        /// <summary>
        /// Gets a new instance of generic param.
        /// </summary>
        /// <param name="source">The object source.</param>
        /// <typeparam name="TSource">The generic param.</typeparam>
        /// <returns>The new instance.</returns>
        public TSource GetGenericInstance<TSource>(TSource source) where TSource : new()
        {
            return source;
        }

        /// <summary>
        /// Map object.
        /// </summary>
        /// <param name="source">The object source.</param>
        /// <param name="target">The target</param>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The source target.</typeparam>
        /// <returns>The mapped object.</returns>
        public TTarget Map<TSource, TTarget>(TSource source, TTarget target) where TTarget : new()
        {
            return target;
        }
    }
}
