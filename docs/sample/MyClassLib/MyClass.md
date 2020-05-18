# MyClass

Namespace: MyClassLib

My class.

```csharp
public class MyClass : IMyInterface
```

Inheritance Object → MyClass

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

### MyClass()

Initializes a new instance of the  class.

```csharp
public MyClass()
```

### MyClass(String, Int32)

Initializes a new instance of the  class.

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

### Get(String)

Gets some thing.

```csharp
public string Get(string param)
```

#### Parameters

`param` String<br>The param.

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
public event EventHandler`1 MyEvent;
```
