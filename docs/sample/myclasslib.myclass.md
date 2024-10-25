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

### **myProtectedField**

My field.

```csharp
protected int myProtectedField;
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

### **MyNullableProperty**

My nullable property

```csharp
public Nullable<int> MyNullableProperty { get; set; }
```

#### Property Value

[Nullable&lt;Int32&gt;](https://docs.microsoft.com/en-us/dotnet/api/system.nullable-1)<br>
The nullable property value.

### **MyEnum**

My enum

```csharp
public MyEnum MyEnum { get; set; }
```

#### Property Value

[MyEnum](./myclasslib.myenum)<br>
The enum value

### **MyProtectedProperty**

My property.

```csharp
protected string MyProtectedProperty { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The property value. Used by .

### **MyProtectedEnum**

My enum

```csharp
protected MyEnum MyProtectedEnum { get; set; }
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

- **item 1** - The first item. `1`
- **item 2** - The second item. `2`

### **MyClass(String, Int32)**

Initializes a new instance of the [MyClass](./myclasslib.myclass) class.<br>
 Use `firstParam` and `secondParam`.

```csharp
public MyClass(string firstParam, int secondParam)
```

#### Parameters

`firstParam` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The first param.

`secondParam` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The second param.

### **MyClass(Int32)**

Initializes a new instance of the [MyClass](./myclasslib.myclass) class.

```csharp
protected MyClass(int int)
```

#### Parameters

`int` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

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

### **TryGet(Int64, String)**

Try to get some thing.

```csharp
public bool TryGet(long param, out string result)
```

#### Parameters

`param` [Int64](https://docs.microsoft.com/en-us/dotnet/api/system.int64)<br>
The param.

`result` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The output param.

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
true if the function succeeded false otherwise

### **StaticMethod()**

A static method.

```csharp
public static void StaticMethod()
```

### **ProtectedDo(String, Int32)**

Do some thing.

```csharp
protected void ProtectedDo(string firstParam, int secondParam)
```

#### Parameters

`firstParam` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
The first param.

`secondParam` [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
The second param.

#### Exceptions

[Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception)<br>
Thrown when...

### **ProtectedDoGeneric&lt;T&gt;(T)**

Do some thing.

```csharp
protected int ProtectedDoGeneric<T>(T value)
```

#### Type Parameters

`T`<br>
The type argument. Used by .

#### Parameters

`value` T<br>
The param. Used by .

#### Returns

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>
Returns a value [Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32).

#### Exceptions

[ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception)<br>
Thrown instead of [Exception](https://docs.microsoft.com/en-us/dotnet/api/system.exception).

### **ProtectedGet(List&lt;String&gt;)**

Gets some thing.

```csharp
protected string ProtectedGet(List<string> param)
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

### **ProtectedStaticMethod()**

A static method.

```csharp
protected static void ProtectedStaticMethod()
```

## Events

### **MyEvent**

My event.

```csharp
public event EventHandler<EventArgs> MyEvent;
```

### **MyProtectedEvent**

My event.

```csharp
protected event EventHandler<EventArgs> MyProtectedEvent;
```

## Example

```csharp
var foo = new MyClass("one", 2);

foo.Do("one", 2);
```

---

[`< Back`](./)
