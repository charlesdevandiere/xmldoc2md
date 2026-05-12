[`< Back`](./)

---

# IMyInterface

Namespace: MyClassLib

My interface.

```csharp
public interface IMyInterface
```

Attributes [NullableContextAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.nullablecontextattribute)

## Methods

### **Do(String, Int32)**

Do some thing.

```csharp
void Do(string firstParam, int secondParam)
```

#### Parameters

`firstParam` [String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
The first param.

`secondParam` [Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The second param.

### **Get(List&lt;String&gt;)**

Gets some thing.

```csharp
string Get(List<string> param)
```

#### Parameters

`param` [List&lt;String&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)<br>
The param.

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
An empty string.

### **Greet()**

A default interface method — has a body, callers can use it as-is.

```csharp
virtual string Greet()
```

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
A greeting.

---

[`< Back`](./)
