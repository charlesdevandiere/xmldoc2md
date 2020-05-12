# MyDele&gt;ate

Namespace: MyClassLib



```csharp
 sealed class MyDelegate : System.MulticastDelegate, System.ICloneable, System.Runtime.Serialization.ISerializable
```

Inheritance Object → Dele&gt;ate → MulticastDele&gt;ate → MyDele&gt;ate

Implements ICloneable, ISerializable

## Properties

### Tar&gt;et



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

### MyDele&gt;ate(Object, IntPtr)



```csharp
public MyDelegate(object object, IntPtr method)
```

#### Parameters

`object` Object<br>

`method` IntPtr<br>

## Methods

### Invoke(Strin&gt;)



```csharp
public void Invoke(string str)
```

#### Parameters

`str` String<br>

### Be&gt;inInvoke(Strin&gt;, AsyncCallback, Object)



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
