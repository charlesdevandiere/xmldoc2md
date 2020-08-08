# SubClass

Namespace: MyClassLib.SubNamespace

Sub class from [MyClassLib.MyClass](../MyClassLib/MyClass.md)

```csharp
public class SubClass : MyClassLib.MyClass, MyClassLib.IMyInterface
```

Inheritance Object → MyClass → SubClass

Implements IMyInterface

## Properties

### MyProperty

My property.

```csharp
public string MyProperty { get; protected set; }
```

#### Property Value

String<br>The property value.

### MyEnum

My enum [MyClassLib.MyEnum](../MyClassLib/MyEnum.md)

```csharp
public MyEnum MyEnum { get; set; }
```

#### Property Value

MyEnum<br>The enum value

## Constructors

### SubClass()



```csharp
public SubClass()
```

## Methods

### ToString()

Convert instance to string.

```csharp
public string ToString()
```

#### Returns

String<br>A string.

## Events

### MyEvent

My event.

```csharp
public event EventHandler`1 MyEvent;
```
