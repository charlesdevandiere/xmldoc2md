[`< Back`](./)

---

# MyClass

Namespace: MyClassLib

My class.

```csharp
public class MyClass : IMyInterface
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MyClass](./myclasslib.myclass)<br>
Implements [IMyInterface](./myclasslib.imyinterface)

**Remarks:**

A remark.

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

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The property value. Used by [MyClass.DoGeneric&lt;T&gt;(T)](./myclasslib.myclass#dogenerictt).

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

[MyEnum](./myclasslib.myenum)<br>
The enum value

## Constructors

### **MyClass()**

Initializes a new instance of the [MyClass](./myclasslib.myclass) class.

```csharp
public MyClass()
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

### **MyClass(String, Int32)**

Initializes a new instance of the [MyClass](./myclasslib.myclass) class.

```csharp
public MyClass(string firstParam, int secondParam)
```

#### Parameters

`firstParam` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The first param.

`secondParam` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The second param.

## Methods

### **Do(String, Int32)**

Do some thing.

```csharp
public void Do(string firstParam, int secondParam)
```

#### Parameters

`firstParam` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The first param.

`secondParam` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The second param.

#### Exceptions

[Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Thrown when...

### **DoGeneric&lt;T&gt;(T)**

Do some thing.

```csharp
public int DoGeneric<T>(T value)
```

#### Type Parameters

`T`<br>
The type argument. Used by [MyClass.DoGeneric&lt;T&gt;(T)](./myclasslib.myclass#dogenerictt).

#### Parameters

`value` T<br>
The param. Used by [MyClass.DoGeneric&lt;T&gt;(T)](./myclasslib.myclass#dogenerictt).

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Returns a value [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32).

#### Exceptions

[ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception)<br>
Thrown instead of [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception).

### **Get(List&lt;String&gt;)**

Gets some thing.

```csharp
public string Get(List<string> param)
```

#### Parameters

`param` [List&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)<br>
The param.

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
An empty string.

#### Exceptions

[Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Thrown when...

#### Example

This example call the `Get` method.

```csharp
var bar = foo.Get("bar");
```

### **StaticMethod()**

A static method.

```csharp
public static void StaticMethod()
```

## Events

### **MyEvent**

My event.

```csharp
public event EventHandler<EventArgs> MyEvent;
```

## Example

```csharp
var foo = new MyClass("one", 2);

foo.Do("one", 2);
```

---

[`< Back`](./)
