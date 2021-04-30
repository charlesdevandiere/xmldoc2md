# SubClass

Namespace: MyClassLib.SubNamespace

Sub class from [MyClass](./myclasslib.myclass)

```csharp
public class SubClass : MyClassLib.MyClass, MyClassLib.IMyInterface
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MyClass](./myclasslib.myclass) → [SubClass](./myclasslib.subnamespace.subclass)<br>
Implements [IMyInterface](./myclasslib.imyinterface)

## Fields

### **myField**

My field.

```csharp
public int myField;
```

## Properties

### **MyProperty**

My property.

```csharp
public string MyProperty { get; protected set; }
```

#### Property Value

String<br>
The property value.

#### Example

This example assign `"foo"` to MyProperty.

```csharp
foo.MyProperty = "foo";
```

### **MyEnum**

My enum

```csharp
public MyEnum MyEnum { get; set; }
```

#### Property Value

MyEnum<br>
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
