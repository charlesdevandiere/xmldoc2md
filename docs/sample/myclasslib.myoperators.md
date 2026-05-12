[`< Back`](./)

---

# MyOperators

Namespace: MyClassLib

A class exercising operator overloads.

```csharp
public class MyOperators
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [MyOperators](./myclasslib.myoperators)<br>
Attributes [NullableContextAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute), [NullableAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullableattribute)

## Properties

### **Value**

The wrapped value.

```csharp
public int Value { get; }
```

#### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

## Constructors

### **MyOperators(Int32)**

Initializes a new [MyOperators](./myclasslib.myoperators).

```csharp
public MyOperators(int value)
```

#### Parameters

`value` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The wrapped value.

## Methods

### **Equals(Object)**

```csharp
public override bool Equals(object? obj)
```

#### Parameters

`obj` [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)?<br>

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **GetHashCode()**

```csharp
public override int GetHashCode()
```

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

## Operators

### **operator +(MyOperators, MyOperators)**

Adds two operands.

```csharp
public static MyOperators operator +(MyOperators a, MyOperators b)
```

#### Parameters

`a` [MyOperators](./myclasslib.myoperators)<br>
The left operand.

`b` [MyOperators](./myclasslib.myoperators)<br>
The right operand.

#### Returns

[MyOperators](./myclasslib.myoperators)<br>
The sum.

### **operator -(MyOperators, MyOperators)**

Subtracts two operands.

```csharp
public static MyOperators operator -(MyOperators a, MyOperators b)
```

#### Parameters

`a` [MyOperators](./myclasslib.myoperators)<br>
The left operand.

`b` [MyOperators](./myclasslib.myoperators)<br>
The right operand.

#### Returns

[MyOperators](./myclasslib.myoperators)<br>
The difference.

### **operator ==(MyOperators, MyOperators)**

Equality.

```csharp
public static bool operator ==(MyOperators a, MyOperators b)
```

#### Parameters

`a` [MyOperators](./myclasslib.myoperators)<br>
The left operand.

`b` [MyOperators](./myclasslib.myoperators)<br>
The right operand.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
True when values are equal.

### **operator !=(MyOperators, MyOperators)**

Inequality.

```csharp
public static bool operator !=(MyOperators a, MyOperators b)
```

#### Parameters

`a` [MyOperators](./myclasslib.myoperators)<br>
The left operand.

`b` [MyOperators](./myclasslib.myoperators)<br>
The right operand.

#### Returns

[Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean)<br>
True when values are not equal.

### **implicit operator int(MyOperators)**

Implicit conversion to [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32).

```csharp
public static implicit operator int(MyOperators value)
```

#### Parameters

`value` [MyOperators](./myclasslib.myoperators)<br>
The value to convert.

#### Returns

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **explicit operator MyOperators(Int32)**

Explicit conversion from [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32).

```csharp
public static explicit operator MyOperators(int value)
```

#### Parameters

`value` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The value to convert.

#### Returns

[MyOperators](./myclasslib.myoperators)<br>

---

[`< Back`](./)
