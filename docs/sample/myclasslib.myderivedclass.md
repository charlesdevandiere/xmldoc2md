[`< Back`](./)

---

# MyDerivedClass

Namespace: MyClassLib

A concrete class deriving from [MyAbstractClass](./myclasslib.myabstractclass) to demonstrate
 `override`, `sealed override`, and `new` modifiers.

```csharp
public class MyDerivedClass : MyAbstractClass
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [MyAbstractClass](./myclasslib.myabstractclass) → [MyDerivedClass](./myclasslib.myderivedclass)

## Properties

### **MyProperty**

My abstract property.

```csharp
public override int MyProperty { get; set; }
```

#### Property Value

[Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32)<br>
The property value.

## Constructors

### **MyDerivedClass()**

```csharp
public MyDerivedClass()
```

## Methods

### **Do()**

A concrete `override` of the abstract [MyAbstractClass.Do()](./myclasslib.myabstractclass#do).

```csharp
public override void Do()
```

### **Label()**

A virtual hook that subclasses may further override.

```csharp
public virtual string Label()
```

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
A label.

---

[`< Back`](./)
