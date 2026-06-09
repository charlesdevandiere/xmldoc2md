using System.Reflection;
using MyClassLib.SubNamespace;
using XMLDoc2Markdown.Signatures;

namespace XMLDoc2Markdown.Tests.Signatures.Modifiers;

[Collection(nameof(SampleAssemblyCollection))]
public class GenericConstraintsModifierTests
{
    [Fact]
    public void Type_signature_emits_class_and_new_constraint()
    {
        string sig = typeof(Repository<>).GetSignature(full: true);
        Assert.Contains("where TEntity : class, new()", sig);
    }

    [Fact]
    public void Method_signature_emits_new_constraint()
    {
        MethodInfo m = typeof(GenericClass<>).GetMethod(nameof(GenericClass<object>.GetGenericInstance), Type.EmptyTypes)!;
        Assert.Contains("where TSource : new()", m.GetSignature(full: true));
    }

    [Fact]
    public void Method_signature_emits_unmanaged_constraint()
    {
        MethodInfo m = typeof(GenericClass<>).GetMethod(nameof(GenericClass<object>.WriteRaw))!;
        Assert.Contains("where TBlittable : unmanaged", m.GetSignature(full: true));
    }

    [Fact]
    public void Method_signature_emits_base_type_and_interface_constraints()
    {
        MethodInfo m = typeof(GenericClass<>).GetMethod(nameof(GenericClass<object>.StoreOrdered))!;
        string sig = m.GetSignature(full: true);
        Assert.Contains("where TKey : IComparable<TKey>", sig);
        Assert.Contains("where TValue : class, IDisposable", sig);
    }

    [Fact]
    public void No_constraint_clause_when_type_has_no_constraints()
    {
        string sig = typeof(MyClassLib.MyClass).GetSignature(full: true);
        Assert.DoesNotContain("where ", sig);
    }
}
