[`< Back`](./)

---

# InheritDocImpl

Namespace: MyClassLib

Demonstrates `<inheritdoc/>` on an interface implementation and an
 explicit `<inheritdoc cref="..."/>` pointing at an unrelated member.

```csharp
public class InheritDocImpl : IInheritDocSource
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [InheritDocImpl](./myclasslib.inheritdocimpl)<br>
Implements [IInheritDocSource](./myclasslib.iinheritdocsource)

## Constructors

### **InheritDocImpl()**

```csharp
public InheritDocImpl()
```

## Methods

### **Lookup(String)**

Looks up an item by key.

```csharp
public string Lookup(string key)
```

#### Parameters

`key` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The lookup key.

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The matched item, or an empty string.

### **ReuseFromCref(Int32)**

Processes an input value.

```csharp
public int ReuseFromCref(int value)
```

#### Parameters

`value` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The input value.

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The processed value.

#### Exceptions

[ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception)<br>
Thrown when `value` is negative.

---

[`< Back`](./)
