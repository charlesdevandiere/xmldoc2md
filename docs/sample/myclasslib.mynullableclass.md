---
layout: default
title: MyNullableClass
---

[`< Back`](./)

---

# MyNullableClass

Namespace: MyClassLib

Showcases nullable reference types and named-tuple element rendering.

```csharp
public class MyNullableClass
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [MyNullableClass](./myclasslib.mynullableclass)<br>
Attributes [NullableContextAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute), [NullableAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullableattribute)

## Fields

### **OptionalNames**

A field with a nullable generic type argument.

```csharp
public IEnumerable<string?>? OptionalNames;
```

## Properties

### **OptionalName**

A nullable-reference property.

```csharp
public string? OptionalName { get; set; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

### **RequiredName**

A non-nullable-reference property (annotation should NOT add a `?`).

```csharp
public string RequiredName { get; set; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Counter**

A value-type Nullable property — renders as `int?`, not `Nullable<int>`.

```csharp
public int? Counter { get; set; }
```

#### Property Value

[Int32?](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>

## Constructors

### **MyNullableClass()**

```csharp
public MyNullableClass()
```

## Methods

### **Origin()**

Returns a named tuple.

```csharp
public (int X, int Y) Origin()
```

#### Returns

`(Int32 X, Int32 Y)`<br>
A named (X, Y) pair.

### **Describe((String, Int32), (Int32, Int32))**

Accepts a named tuple parameter and an unnamed tuple parameter.

```csharp
public string Describe((string Name, int Age) point, (int, int) pair)
```

#### Parameters

`point` `(String Name, Int32 Age)`<br>
A named pair.

`pair` `(Int32, Int32)`<br>
A positional pair.

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
A descriptive label.

### **Echo(String)**

Nullable-reference parameter and nullable-reference return.

```csharp
public string? Echo(string? input)
```

#### Parameters

`input` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)?<br>
Possibly null input.

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)?<br>
Possibly null output.

---

[`< Back`](./)
