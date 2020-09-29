# MyDelegate

Namespace: MyClassLib



```csharp
 sealed class MyDelegate : System.MulticastDelegate, System.ICloneable, System.Runtime.Serialization.ISerializable
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [Delegate](https://docs.microsoft.com/en-us/dotnet/api/system.delegate) → [MulticastDelegate](https://docs.microsoft.com/en-us/dotnet/api/system.multicastdelegate) → [MyDelegate](../MyClassLib/MyDelegate.md)

Implements [ICloneable](https://docs.microsoft.com/en-us/dotnet/api/system.icloneable), [ISerializable](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.serialization.iserializable)

## Properties

### Target



```csharp
public object Target { get; }
```

#### Property Value

Object<br>

### Method



```csharp
public MethodInfo Method { get; }
```

#### Property Value

MethodInfo<br>

## Constructors

### MyDelegate(Object, IntPtr)



```csharp
public MyDelegate(object object, IntPtr method)
```

#### Parameters

`object` Object<br>

`method` IntPtr<br>

## Methods

### Invoke(String)



```csharp
public void Invoke(string str)
```

#### Parameters

`str` String<br>

### BeginInvoke(String, AsyncCallback, Object)



```csharp
public IAsyncResult BeginInvoke(string str, AsyncCallback callback, object object)
```

#### Parameters

`str` String<br>

`callback` AsyncCallback<br>

`object` Object<br>

#### Returns

IAsyncResult<br>

### EndInvoke(IAsyncResult)



```csharp
public void EndInvoke(IAsyncResult result)
```

#### Parameters

`result` IAsyncResult<br>
