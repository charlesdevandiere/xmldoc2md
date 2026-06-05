---
layout: default
title: MyParameterModifiers
---

[`< Back`](./)

---

# MyParameterModifiers

Namespace: MyClassLib

Showcases `ref`, `in`, `out`, `params` parameter modifiers
 and default parameter values.

```csharp
public class MyParameterModifiers
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [MyParameterModifiers](./myclasslib.myparametermodifiers)<br>
Attributes [NullableContextAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute), [NullableAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullableattribute)

## Constructors

### **MyParameterModifiers()**

```csharp
public MyParameterModifiers()
```

## Methods

### **RefIncrement(ref Int32)**

Increments a value by reference.

```csharp
public void RefIncrement(ref int value)
```

#### Parameters

`ref` `value` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The reference-passed value.

### **Read(in Int32)**

Reads a value via `in`.

```csharp
public int Read(in int value)
```

#### Parameters

`in` `value` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The read-only reference.

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The value squared.

### **TryParse(String, out Int32)**

Tries to parse a value, writing the parsed result via `out`.

```csharp
public bool TryParse(string input, out int value)
```

#### Parameters

`input` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The text to parse.

`out` `value` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The parsed value.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
on success.

### **Join(params String[])**

Concatenates a variable number of items.

```csharp
public string Join(params String[] items)
```

#### Parameters

`params` `items` [String[]](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The items to join.

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The joined string.

### **Defaults(String, Int32, Double, Boolean, MyEnum, String)**

Shows several default-value flavours.

```csharp
public void Defaults(string name = "anakin", int count = 42, double ratio = 1.5d, bool flag = true, MyEnum kind = MyEnum.First, string? note = null)
```

#### Parameters

`name` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
A string default.

`count` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
A numeric default.

`ratio` [Double](https://learn.microsoft.com/en-us/dotnet/api/system.double)<br>
A double default.

`flag` [Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
A bool default.

`kind` [MyEnum](./myclasslib.myenum)<br>
An enum default.

`note` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)?<br>
A nullable-reference default of .

---

[`< Back`](./)
