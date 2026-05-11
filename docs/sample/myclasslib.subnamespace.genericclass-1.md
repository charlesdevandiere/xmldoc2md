[`< Back`](./)

---

# GenericClass&lt;T&gt;

Namespace: MyClassLib.SubNamespace

Generic class.

```csharp
public class GenericClass<T> where T : new()
```

#### Type Parameters

`T`<br>
The type param.

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [GenericClass&lt;T&gt;](./myclasslib.subnamespace.genericclass-1)<br>
Attributes [NullableContextAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute), [NullableAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullableattribute)

## Constructors

### **GenericClass()**

Initializes a new instance of the [GenericClass&lt;T&gt;](./myclasslib.subnamespace.genericclass-1) class.

```csharp
public GenericClass()
```

### **GenericClass(T)**

Initializes a new instance of the [GenericClass&lt;T&gt;](./myclasslib.subnamespace.genericclass-1) class.

```csharp
public GenericClass(T param)
```

#### Parameters

`param` T<br>
The generic parameter.

## Methods

### **GetGenericInstance&lt;TSource&gt;()**

Gets a new instance of generic param.

```csharp
public TSource GetGenericInstance<TSource>() where TSource : new()
```

#### Type Parameters

`TSource`<br>
The generic param.

#### Returns

TSource<br>
The new instance.

### **GetGenericInstance&lt;TSource&gt;(TSource)**

Gets a new instance of generic param.

```csharp
public TSource GetGenericInstance<TSource>(TSource source) where TSource : new()
```

#### Type Parameters

`TSource`<br>
The generic param.

#### Parameters

`source` TSource<br>
The object source.

#### Returns

TSource<br>
The new instance.

### **Map&lt;TSource, TTarget&gt;(TSource, TTarget)**

Map object.

```csharp
public TTarget Map<TSource, TTarget>(TSource source, TTarget target) where TTarget : new()
```

#### Type Parameters

`TSource`<br>
The source type.

`TTarget`<br>
The source target.

#### Parameters

`source` TSource<br>
The object source.

`target` TTarget<br>
The target

#### Returns

TTarget<br>
The mapped object.

### **StoreOrdered&lt;TKey, TValue&gt;(TKey, TValue)**

Stores a value indexed by a comparable, non-null key.

```csharp
public void StoreOrdered<TKey, TValue>(TKey key, TValue value) where TKey : IComparable<TKey> where TValue : class, IDisposable
```

#### Type Parameters

`TKey`<br>
A non-nullable comparable key.

`TValue`<br>
A reference type implementing [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable).

#### Parameters

`key` TKey<br>
The key.

`value` TValue<br>
The value.

### **WriteRaw&lt;TBlittable&gt;(TBlittable)**

Writes an unmanaged value to a sink.

```csharp
public void WriteRaw<TBlittable>(TBlittable value) where TBlittable : unmanaged
```

#### Type Parameters

`TBlittable`<br>
An unmanaged value type.

#### Parameters

`value` TBlittable<br>
The unmanaged value.

---

[`< Back`](./)
