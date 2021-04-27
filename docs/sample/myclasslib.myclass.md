# MyClass

Namespace: MyClassLib

My class.

```csharp
public class MyClass : IMyInterface
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MyClass](./myclasslib.myclass.md)

Implements [IMyInterface](./myclasslib.imyinterface.md)

## Properties

### MyProperty

My property ([String](https://docs.microsoft.com/en-us/dotnet/api/system.string)).

```csharp
public string MyProperty { get; protected set; }
```

#### Property Value

String<br>The property value.

#### Example

This example assign `"foo"` to MyProperty.

```csharp
foo.MyProperty = "foo";
```

### MyEnum

My enum [MyEnum](./myclasslib.myenum.md)

```csharp
public MyEnum MyEnum { get; set; }
```

#### Property Value

MyEnum<br>The enum value

## Constructors

### MyClass()

Initializes a new instance of the [MyClass](./myclasslib.myclass.md) class.

```csharp
public MyClass()
```

### MyClass(String, Int32)

Initializes a new instance of the [MyClass](./myclasslib.myclass.md) class.

```csharp
public MyClass(string firstParam, int secondParam)
```

#### Parameters

`firstParam` String<br>The first param.

`secondParam` Int32<br>The second param.

## Methods

### Do(String, Int32)

Do some thing.

```csharp
public void Do(string firstParam, int secondParam)
```

#### Parameters

`firstParam` String<br>The first param.

`secondParam` Int32<br>The second param.

#### Exceptions

Exception<br>Thrown when...

### Get(List&lt;String&gt;)

Gets some thing.

```csharp
public string Get(List<string> param)
```

#### Parameters

`param` List&lt;String&gt;<br>The param.

#### Returns

String<br>An empty string.

#### Exceptions

Exception<br>Thrown when...

### StaticMethod()

A static method.

```csharp
public static void StaticMethod()
```

## Events

### MyEvent

My event.

```csharp
public event EventHandler<EventArgs> MyEvent;
```

## Example

```csharp
var foo = new MyClass("one", 2);

foo.Do("one", 2);
```
