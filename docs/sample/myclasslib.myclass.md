# <img align="left" width="100" height="100" src="icon.png">Custom Title :MyClass 
[![Build Status](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_apis/build/status/charlesdevandiere.xmldoc2md?branchName=master)](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_build/latest?definitionId=2&branchName=master)
[![Nuget](https://img.shields.io/nuget/v/XMLDoc2Markdown.svg?color=blue&logo=nuget)](https://www.nuget.org/packages/XMLDoc2Markdown)

[**Back to Index**](index.md)
- - -

# MyClass

Namespace: MyClassLib

My class.

```csharp
public class MyClass : IMyInterface
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MyClass](./myclasslib.myclass.md)<br>
Implements [IMyInterface](./myclasslib.imyinterface.md)

**Remarks:**

A remark.

## Fields

### <a id="fields-myfield"/>**myField**

My field.

```csharp
public int myField;
```

## Properties

### <a id="properties-myenum"/>**MyEnum**

My enum

```csharp
public MyEnum MyEnum { get; set; }
```

#### Property Value

[MyEnum](./myclasslib.myenum.md)<br>
The enum value

### <a id="properties-myproperty"/>**MyProperty**

My property.
 <br>Teste

```csharp
public string MyProperty { get; protected set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The property value. Used by [MyClass.DoGeneric&lt;T&gt;(T)](./myclasslib.myclass.md#dogenerictt).

#### Example

This example assign `"foo"` to MyProperty.

```csharp
foo.MyProperty = "foo";
```

## Constructors

### <a id="constructors-.ctor"/>**MyClass()**

Ctor MyClass.
 <br>Line1<br>Line2<br>Line3<br>Line4<br>Line5

```csharp
public MyClass()
```

**Remarks:**

See also [MyClass.MyClass(String, Int32)](./myclasslib.myclass.md#myclassstring-int32).

```csharp
if (true)
{
    var foo = new MyClass("foo", 1);
    Console.WriteLine(foo.ToString());
}
```

### <a id="constructors-.ctor"/>**MyClass(String, Int32)**

Initializes a new instance of the [MyClass](./myclasslib.myclass.md) class.

```csharp
public MyClass(string firstParam, int secondParam)
```

#### Parameters

`firstParam` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The first param.

`secondParam` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The second param.

## Methods

### <a id="methods-do"/>**Do(String, Int32)**

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

### <a id="methods-dogeneric"/>**DoGeneric&lt;T&gt;(T)**

Do some thing.

```csharp
public int DoGeneric<T>(T value)
```

#### Type Parameters

`T`<br>
The type argument. Used by [MyClass.DoGeneric&lt;T&gt;(T)](./myclasslib.myclass.md#dogenerictt).

#### Parameters

`value` T<br>
The param. Used by [MyClass.DoGeneric&lt;T&gt;(T)](./myclasslib.myclass.md#dogenerictt).

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)

#### Exceptions

[ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception)<br>
Thrown instead of [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception).

### <a id="methods-get"/>**Get(List&lt;String&gt;)**

Gets some thing.

```csharp
public string Get(List<String> param)
```

#### Parameters

`param` [List&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)<br>
The param.

#### Returns

An empty [String](https://docs.microsoft.com/en-us/dotnet/api/system.string).

#### Exceptions

[Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Thrown when...

#### Example

This example call the `Get` method.

```csharp
var bar = foo.Get("bar");
```

### <a id="methods-staticmethod"/>**StaticMethod()**

A static method.

```csharp
public static void StaticMethod()
```

## Events

### <a id="events-myevent"/>**MyEvent**

My event.

```csharp
public event EventHandler<EventArgs> MyEvent;
```

## Example

```csharp
var foo = new MyClass("one", 2);

foo.Do("one", 2);
```


- - -
[**Back to Index**](index.md)
