---
layout: default
title: Point
---

[`< Back`](../)

---

# Point

Namespace: MyClassLib

A record struct representing a 2D point.

```csharp
public readonly record struct Point
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [ValueType](https://learn.microsoft.com/en-us/dotnet/api/system.valuetype) → [Point](./myclasslib/point)<br>
Implements [IEquatable&lt;Point&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<br>
Attributes [IsReadOnlyAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.isreadonlyattribute)

## Properties

### **X**

The X coordinate.

```csharp
public int X { get; init; }
```

#### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **Y**

The Y coordinate.

```csharp
public int Y { get; init; }
```

#### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

## Constructors

### **Point(Int32, Int32)**

A record struct representing a 2D point.

```csharp
public Point(int X, int Y)
```

#### Parameters

`X` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The X coordinate.

`Y` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The Y coordinate.

---

[`< Back`](../)
