namespace MyClassLib.SubNamespace;

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

    /// <summary>
    /// Stores a value indexed by a comparable, non-null key.
    /// </summary>
    /// <typeparam name="TKey">A non-nullable comparable key.</typeparam>
    /// <typeparam name="TValue">A reference type implementing <see cref="System.IDisposable" />.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void StoreOrdered<TKey, TValue>(TKey key, TValue value)
        where TKey : notnull, System.IComparable<TKey>
        where TValue : class, System.IDisposable
    {
    }

    /// <summary>
    /// Writes an unmanaged value to a sink.
    /// </summary>
    /// <typeparam name="TBlittable">An unmanaged value type.</typeparam>
    /// <param name="value">The unmanaged value.</param>
    public void WriteRaw<TBlittable>(TBlittable value) where TBlittable : unmanaged
    {
    }
}

/// <summary>
/// A repository abstraction with class + new() constraints on its type parameter.
/// </summary>
/// <typeparam name="TEntity">A reference-type entity with a parameterless ctor.</typeparam>
public class Repository<TEntity> where TEntity : class, new()
{
    /// <summary>
    /// Creates a fresh entity.
    /// </summary>
    /// <returns>A new entity instance.</returns>
    public TEntity Create() => new();
}
