[`< Back`](./)

---

# InheritDocBase

Namespace: MyClassLib

Base type for the `<inheritdoc/>` demos.

```csharp
public abstract class InheritDocBase
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [InheritDocBase](./myclasslib.inheritdocbase)<br>
Attributes [NullableContextAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute), [NullableAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullableattribute)

**Remarks:**

Defines the documented members the derived types inherit from.

## Properties

### **Name**

The configured name.

```csharp
public abstract string Name { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The display name.

## Constructors

### **InheritDocBase()**

```csharp
protected InheritDocBase()
```

## Methods

### **Process(Int32)**

Processes an input value.

```csharp
public abstract int Process(int value)
```

#### Parameters

`value` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The input value.

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The processed value.

#### Exceptions

[ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception)<br>
Thrown when `value` is negative.

---

[`< Back`](./)
