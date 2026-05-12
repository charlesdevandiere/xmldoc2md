[`< Back`](./)

---

# SubClass

Namespace: MyClassLib.SubNamespace

Sub class from [MyClass](./myclasslib.myclass)

```csharp
public sealed class SubClass : MyClassLib.MyClass, MyClassLib.IMyInterface
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [MyClass](./myclasslib.myclass) → [SubClass](./myclasslib.subnamespace.subclass)<br>
Implements [IMyInterface](./myclasslib.imyinterface)

**Remarks:**

A remark.

## Fields

### **myField**

My field.

```csharp
public int myField;
```

### **myRequiredField**

A required field.

```csharp
public required int myRequiredField;
```

### **myProtectedField**

My field.

```csharp
protected int myProtectedField;
```

## Properties

### **MyProperty**

My property.

```csharp
public string MyProperty { get; protected set; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The property value. Used by [MyClass.DoGeneric&lt;T&gt;(T)](./myclasslib.myclass#dogenerictt).

#### Example

This example assign `"foo"` to MyProperty.

```csharp
foo.MyProperty = "foo";
```

### **MyNullableProperty**

My nullable property

```csharp
public int? MyNullableProperty { get; set; }
```

#### Property Value

[Int32?](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The nullable property value.

### **MyEnum**

My enum

```csharp
public MyEnum MyEnum { get; set; }
```

#### Property Value

[MyEnum](./myclasslib.myenum)<br>
The enum value

### **MyInitProperty**

An init-only property.

```csharp
public string MyInitProperty { get; init; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The initialization value.

### **MyRequiredProperty**

A required property.

```csharp
public required string MyRequiredProperty { get; set; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The required value.

### **MyProtectedProperty**

My property.

```csharp
protected string MyProtectedProperty { get; set; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The property value. Used by [MyClass.ProtectedDoGeneric&lt;T&gt;(T)](./myclasslib.myclass#protecteddogenerictt).

### **MyProtectedEnum**

My enum

```csharp
protected MyEnum MyProtectedEnum { get; set; }
```

#### Property Value

[MyEnum](./myclasslib.myenum)<br>
The enum value

## Constructors

### **SubClass()**

Sub class from [MyClass](./myclasslib.myclass)

```csharp
public SubClass()
```

**Remarks:**

See also [MyClass.MyClass(String, Int32)](./myclasslib.myclass#myclassstring-int32).

```csharp
if (true)
{
    var foo = new MyClass("foo", 1);
    Console.WriteLine(foo.ToString());
}
```

- **item 1** - The first item. `1`
- **item 2** - The second item. `2`

## Methods

### **ToString()**

Convert instance to string.

```csharp
public override string ToString()
```

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
A string.

## Events

### **MyEvent**

My event.

```csharp
public event EventHandler<EventArgs>? MyEvent;
```

### **MyProtectedEvent**

My event.

```csharp
protected event EventHandler<EventArgs>? MyProtectedEvent;
```

---

[`< Back`](./)
