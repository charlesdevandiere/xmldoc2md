# <img align="left" width="100" height="100" src="icon.png">Custom Title :MyObsoleteClass 
[![Build Status](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_apis/build/status/charlesdevandiere.xmldoc2md?branchName=master)](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_build/latest?definitionId=2&branchName=master)
[![Nuget](https://img.shields.io/nuget/v/XMLDoc2Markdown.svg?color=blue&logo=nuget)](https://www.nuget.org/packages/XMLDoc2Markdown)

[**Back to Index**](index.md)
- - -

# MyObsoleteClass

Namespace: MyClassLib

#### Caution

Deprecated, use MyClass instead.

---

My obsolete class.

```csharp
public class MyObsoleteClass
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MyObsoleteClass](./myclasslib.myobsoleteclass.md)

**Remarks:**

A remark.

## Fields

### <a id="fields-myfield"/>**myField**

#### Caution

This member is obsolete.

---

My field.

```csharp
public int myField;
```

## Properties

### <a id="properties-myproperty"/>**MyProperty**

#### Caution

This member is obsolete.

---

My property.

```csharp
public string MyProperty { get; protected set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The property value.

### <a id="properties-myproperty2"/>**MyProperty2**

Instances.

```csharp
public int MyProperty2 { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### <a id="properties-staticproperty"/>**StaticProperty**

This is my Static property.

```csharp
public static int StaticProperty { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

## Constructors

### <a id="constructors-.ctor"/>**MyObsoleteClass()**

#### Caution

This member is obsolete.

---

Initializes a new instance of the [MyClass](./myclasslib.myclass.md) class.

```csharp
public MyObsoleteClass()
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

## Methods

### <a id="methods-get"/>**Get(List&lt;String&gt;)**

#### Caution

This member is obsolete.

---

Gets some thing.

```csharp
public string Get(List<String> param)
```

#### Parameters

`param` [List&lt;String&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)<br>
The param.

#### Returns

An empty string.

#### Exceptions

[Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Thrown when...

### <a id="methods-staticmethod"/>**StaticMethod()**

#### Caution

Deprecated in favor of MyClass.

---

A static method.

```csharp
public static void StaticMethod()
```


- - -
[**Back to Index**](index.md)
