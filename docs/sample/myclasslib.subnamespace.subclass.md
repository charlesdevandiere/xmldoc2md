# <img align="left" width="100" height="100" src="icon.png">Custom Title :SubClass 
[![Build Status](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_apis/build/status/charlesdevandiere.xmldoc2md?branchName=master)](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_build/latest?definitionId=2&branchName=master)
[![Nuget](https://img.shields.io/nuget/v/XMLDoc2Markdown.svg?color=blue&logo=nuget)](https://www.nuget.org/packages/XMLDoc2Markdown)

[**Back to Index**](index.md)
- - -

# SubClass

Namespace: MyClassLib.SubNamespace

Sub class from [MyClass](./myclasslib.myclass.md)

```csharp
public class SubClass : MyClassLib.MyClass, MyClassLib.IMyInterface
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MyClass](./myclasslib.myclass.md) → [SubClass](./myclasslib.subnamespace.subclass.md)<br>
Implements [IMyInterface](./myclasslib.imyinterface.md)

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

### <a id="constructors-.ctor"/>**SubClass()**

```csharp
public SubClass()
```

## Methods

### <a id="methods-tostring"/>**ToString()**

Convert instance to string.

```csharp
public string ToString()
```

#### Returns

A string.

## Events

### <a id="events-myevent"/>**MyEvent**

My event.

```csharp
public event EventHandler<EventArgs> MyEvent;
```


- - -
[**Back to Index**](index.md)
