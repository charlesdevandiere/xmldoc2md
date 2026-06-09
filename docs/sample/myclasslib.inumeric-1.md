---
layout: default
title: "INumeric<T>"
---

[`< Back`](./)

---

# INumeric&lt;T&gt;

Namespace: MyClassLib

An interface with a `static abstract` member and a default static implementation.

```csharp
public interface INumeric<T> where T : INumeric<T>
```

#### Type Parameters

`T`<br>
The implementing type.

Attributes [NullableContextAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute)

## Methods

### **Zero()**

A static abstract factory.

```csharp
static abstract T Zero()
```

#### Returns

T<br>
A zero value.

### **Square(T)**

A default static interface method.

```csharp
static virtual T Square(T x)
```

#### Parameters

`x` T<br>
The value.

#### Returns

T<br>
The squared value.

## Operators

### **operator +(T, T)**

A static abstract operator — implementers must provide an addition operator.

```csharp
static abstract T operator +(T left, T right)
```

#### Parameters

`left` T<br>
The left operand.

`right` T<br>
The right operand.

#### Returns

T<br>
The sum.

---

[`< Back`](./)
