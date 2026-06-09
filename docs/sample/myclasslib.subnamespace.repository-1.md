---
layout: default
title: "Repository<TEntity>"
---

[`< Back`](./)

---

# Repository&lt;TEntity&gt;

Namespace: MyClassLib.SubNamespace

A repository abstraction with class + new() constraints on its type parameter.

```csharp
public class Repository<TEntity> where TEntity : class, new()
```

#### Type Parameters

`TEntity`<br>
A reference-type entity with a parameterless ctor.

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [Repository&lt;TEntity&gt;](./myclasslib.subnamespace.repository-1)<br>
Attributes [NullableContextAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute), [NullableAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullableattribute)

## Constructors

### **Repository()**

```csharp
public Repository()
```

## Methods

### **Create()**

Creates a fresh entity.

```csharp
public TEntity Create()
```

#### Returns

TEntity<br>
A new entity instance.

---

[`< Back`](./)
