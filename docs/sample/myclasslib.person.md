---
layout: default
title: Person
---

[`< Back`](./)

---

# Person

Namespace: MyClassLib

A record class representing a person.

```csharp
public record class Person : System.IEquatable`1[[MyClassLib.Person, MyClassLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [Person](./myclasslib.person)<br>
Implements [IEquatable&lt;Person&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)<br>
Attributes [NullableContextAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute), [NullableAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullableattribute)

## Properties

### **FirstName**

The first name.

```csharp
public string FirstName { get; init; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

### **LastName**

The last name.

```csharp
public string LastName { get; init; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

### **FullName**

Gets the full name.

```csharp
public string FullName { get; }
```

#### Property Value

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>

## Constructors

### **Person(String, String)**

A record class representing a person.

```csharp
public Person(string FirstName, string LastName)
```

#### Parameters

`FirstName` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The first name.

`LastName` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The last name.

### **Person(Person)**

```csharp
protected Person(Person original)
```

#### Parameters

`original` [Person](./myclasslib.person)<br>

---

[`< Back`](./)
