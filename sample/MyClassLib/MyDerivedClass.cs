namespace MyClassLib;

/// <summary>
/// A concrete class deriving from <see cref="MyAbstractClass" /> to demonstrate
/// <c>override</c>, <c>sealed override</c>, and <c>new</c> modifiers.
/// </summary>
public class MyDerivedClass : MyAbstractClass
{
    /// <inheritdoc/>
    public override int MyProperty { get; set; }

    /// <summary>
    /// A concrete <c>override</c> of the abstract <see cref="MyAbstractClass.Do" />.
    /// </summary>
    public override void Do() { }

    /// <summary>
    /// A virtual hook that subclasses may further override.
    /// </summary>
    /// <returns>A label.</returns>
    public virtual string Label() => "derived";
}

/// <summary>
/// A grandchild that seals one override and hides a base method with <c>new</c>.
/// </summary>
public sealed class MyGrandchildClass : MyDerivedClass
{
    /// <summary>
    /// Seals the <see cref="MyDerivedClass.Label" /> override.
    /// </summary>
    /// <returns>A label.</returns>
    public sealed override string Label() => "grandchild";

    /// <summary>
    /// Hides the base <see cref="MyAbstractClass.Do" /> with a same-named method.
    /// </summary>
    public new void Do() { }
}
