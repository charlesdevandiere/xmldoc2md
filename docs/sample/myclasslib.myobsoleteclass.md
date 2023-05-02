[`< Back`](./)

---

# MyObsoleteClass

Namespace: MyClassLib

#### Caution

Deprecated, use MyClass instead.

---

My obsolete class.

```csharp
public class MyObsoleteClass
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [MyObsoleteClass](./myclasslib.myobsoleteclass)

**Remarks:**

A remark.

## Fields

### **myField**

#### Caution

This member is obsolete.

---

My field.

```csharp
public int myField;
```

## Properties

### **MyProperty**

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

### **MyProperty2**

Instances.

```csharp
public int MyProperty2 { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **StaticProperty**

This is my Static property.

```csharp
public static int StaticProperty { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

## Constructors

### **MyObsoleteClass()**

#### Caution

This member is obsolete.

---

Initializes a new instance of the [MyClass](./myclasslib.myclass) class.

```csharp
public MyObsoleteClass()
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

## Methods

### **Get(List&lt;String&gt;)**

#### Caution

This member is obsolete.

---

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

### **StaticMethod()**

#### Caution

Deprecated in favor of MyClass.

---

A static method.

```csharp
public static void StaticMethod()
```

---

[`< Back`](./)
