---
layout: default
title: InheritDocDerived
---

[`< Back`](./)

---

# InheritDocDerived

Namespace: MyClassLib

Demonstrates `<inheritdoc/>` on overridden members.

```csharp
public class InheritDocDerived : InheritDocBase
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [InheritDocBase](./myclasslib.inheritdocbase) → [InheritDocDerived](./myclasslib.inheritdocderived)<br>
Attributes [NullableContextAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute), [NullableAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullableattribute)

## Properties

### **Name**

The configured name.

```csharp
public override string Name { get; set; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The display name.

## Constructors

### **InheritDocDerived()**

```csharp
public InheritDocDerived()
```

## Methods

### **Process(Int32)**

Processes an input value.

```csharp
public override int Process(int value)
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

### **ProcessTwice(Int32)**

Keeps its own summary but inherits the rest from [InheritDocBase.Process(Int32)](./myclasslib.inheritdocbase#processint32).

```csharp
public int ProcessTwice(int value)
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
