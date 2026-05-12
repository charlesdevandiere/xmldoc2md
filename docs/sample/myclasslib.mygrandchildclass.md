[`< Back`](./)

---

# MyGrandchildClass

Namespace: MyClassLib

A grandchild that seals one override and hides a base method with `new`.

```csharp
public sealed class MyGrandchildClass : MyDerivedClass
```

Inheritance [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object) → [MyAbstractClass](./myclasslib.myabstractclass) → [MyDerivedClass](./myclasslib.myderivedclass) → [MyGrandchildClass](./myclasslib.mygrandchildclass)

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

### **MyGrandchildClass()**

```csharp
public MyGrandchildClass()
```

## Methods

### **Label()**

Seals the [MyDerivedClass.Label()](./myclasslib.myderivedclass#label) override.

```csharp
public sealed override string Label()
```

#### Returns

[String](https://learn.microsoft.com/en-us/dotnet/api/system.string)<br>
A label.

### **Do()**

Hides the base [MyAbstractClass.Do()](./myclasslib.myabstractclass#do) with a same-named method.

```csharp
public new void Do()
```

---

[`< Back`](./)
