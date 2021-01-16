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
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TTarget">The source target.</typeparam>
        /// <returns>The mapped object.</returns>
        public TTarget Map<TSource, TTarget>(TSource source, TTarget target) where TTarget : new()
        {
            return target;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericClass{T}" /> class.
    /// </summary>
    /// <typeparam name="T1">A generic type parameter.</typeparam>
    /// <typeparam name="T2">A generic type parameter.</typeparam>
    public class GenericClass<T1, T2> 
        : IMyInterface
        where T1 : new()
        where T2 : new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericClass{T1,T2}" /> class.
        /// </summary>
        public GenericClass() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericClass{T1,T2}" /> class.
        /// </summary>
        /// <param name="param">The first generic parameter.</param>
        /// <param name="param2">The second generic parameter.</param>
        public GenericClass(T1 param, T2 param2) { }

        /// <summary>
        /// Returns a new instance of <see cref="GenericClass{T1}"/>.
        /// </summary>
        private GenericClass<T1> GenericClassWithFirstParameter => new GenericClass<T1>();
        
        /// <summary>
        /// Returns a new instance of <see cref="GenericClass{T2}"/>.
        /// </summary>
        private GenericClass<T2> GenericClassWithSecondParameter => new GenericClass<T2>();

        /// <inheritdoc />
        public void Do(string firstParam, int secondParam) => throw new System.NotImplementedException();

        /// <inheritdoc />
        public string Get(string param) => throw new System.NotImplementedException();
    }
}
