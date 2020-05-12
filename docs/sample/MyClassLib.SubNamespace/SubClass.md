# SubClass

Namespace: MyClassLib.SubNamespace

Sub class from

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

## Constructors

### SubClass()



```csharp
public SubClass()
```

## Methods

### ToStrin&gt;()

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
