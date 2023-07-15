# <img align="left" width="100" height="100" src="icon.png">Custom Title :GenericClass<T> 
[![Build Status](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_apis/build/status/charlesdevandiere.xmldoc2md?branchName=master)](https://dev.azure.com/charlesdevandiere/charlesdevandiere/_build/latest?definitionId=2&branchName=master)
[![Nuget](https://img.shields.io/nuget/v/XMLDoc2Markdown.svg?color=blue&logo=nuget)](https://www.nuget.org/packages/XMLDoc2Markdown)

[**Back to Index**](index.md)
- - -

# GenericClass&lt;T&gt;

Namespace: MyClassLib.SubNamespace

Generic class.

```csharp
public class GenericClass<T>
```

#### Type Parameters

`T`<br>
The type param.

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [GenericClass&lt;T&gt;](./myclasslib.subnamespace.genericclass-1.md)

## Constructors

### <a id="constructors-.ctor"/>**GenericClass()**

Initializes a new instance of the [GenericClass&lt;T&gt;](./myclasslib.subnamespace.genericclass-1.md) class.

```csharp
public GenericClass()
```

### <a id="constructors-.ctor"/>**GenericClass(T)**

Initializes a new instance of the [GenericClass&lt;T&gt;](./myclasslib.subnamespace.genericclass-1.md) class.

```csharp
public GenericClass(T param)
```

#### Parameters

`param` T<br>
The generic parameter.

## Methods

### <a id="methods-getgenericinstance"/>**GetGenericInstance&lt;TSource&gt;()**

Gets a new instance of generic param.

```csharp
public TSource GetGenericInstance<TSource>()
```

#### Type Parameters

`TSource`<br>
The generic param.

#### Returns

The new instance.

### <a id="methods-getgenericinstance"/>**GetGenericInstance&lt;TSource&gt;(TSource)**

Gets a new instance of generic param.

```csharp
public TSource GetGenericInstance<TSource>(TSource source)
```

#### Type Parameters

`TSource`<br>
The generic param.

#### Parameters

`source` TSource<br>
The object source.

#### Returns

The new instance.

### <a id="methods-map"/>**Map&lt;TSource, TTarget&gt;(TSource, TTarget)**

Map object.

```csharp
public TTarget Map<TSource, TTarget>(TSource source, TTarget target)
```

#### Type Parameters

`TSource`<br>
The source type.

`TTarget`<br>
The source target.

#### Parameters

`source` TSource<br>
The object source.

`target` TTarget<br>
The target

#### Returns

The mapped object.


- - -
[**Back to Index**](index.md)
