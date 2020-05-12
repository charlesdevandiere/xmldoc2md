# MyEventHandler

Namespace: MyClassLib



```csharp
 sealed class MyEventHandler : System.MulticastDelegate, System.ICloneable, System.Runtime.Serialization.ISerializable
```

Inheritance Object → Delegate → MulticastDelegate → MyEventHandler

Implements ICloneable, ISerializable

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

### MyEventHandler(Object, IntPtr)



```csharp
public MyEventHandler(object object, IntPtr method)
```

#### Parameters

`object` Object<br>

`method` IntPtr<br>

## Methods

### Invoke(Object, EventArgs)



```csharp
public void Invoke(object sender, EventArgs args)
```

#### Parameters

`sender` Object<br>

`args` EventArgs<br>

### BeginInvoke(Object, EventArgs, AsyncCallback, Object)



```csharp
public IAsyncResult BeginInvoke(object sender, EventArgs args, AsyncCallback callback, object object)
```

#### Parameters

`sender` Object<br>

`args` EventArgs<br>

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
