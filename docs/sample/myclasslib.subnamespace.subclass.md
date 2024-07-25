[`< Back`](./)

---

# SubClass

Namespace: MyClassLib.SubNamespace

Sub class from [MyClass](./myclasslib.myclass)

```csharp
public sealed class SubClass : MyClassLib.MyClass, MyClassLib.IMyInterface
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MyClass](./myclasslib.myclass) → [SubClass](./myclasslib.subnamespace.subclass)<br>
Implements [IMyInterface](./myclasslib.imyinterface)

## Fields

### **myField**

My field.

```csharp
public int myField;
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

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The property value. Used by [MyClass.DoGeneric&lt;T&gt;(T)](./myclasslib.myclass#dogenerictt).

#### Example

This example assign `"foo"` to MyProperty.

```csharp
foo.MyProperty = "foo";
```

### **MyNullableProperty**

My nullable property

```csharp
public Nullable<int> MyNullableProperty { get; set; }
```

#### Property Value

[Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The nullable property value.

### **MyEnum**

My enum

```csharp
public MyEnum MyEnum { get; set; }
```

#### Property Value

[MyEnum](./myclasslib.myenum)<br>
The enum value

### **MyProtectedProperty**

My property.

```csharp
protected string MyProtectedProperty { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The property value. Used by .

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

```csharp
public SubClass()
```

## Methods

### **ToString()**

Convert instance to string.

```csharp
public string ToString()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A string.

## Events

### **MyEvent**

My event.

```csharp
public event EventHandler<EventArgs> MyEvent;
```

### **MyProtectedEvent**

My event.

```csharp
protected event EventHandler<EventArgs> MyProtectedEvent;
```

---

[`< Back`](./)
