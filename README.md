# CrossForms

Cross-platform UI framework for .NET with native macOS and Windows backends.

## Objective-C (macOS) notes

The macOS backend wraps Objective-C objects via P/Invoke. Three naming conventions express memory ownership at the call
site:

### `*Auto(...)` — autoreleased pointer

The returned object comes from an ObjC convenience factory (e.g. `buttonWithTitle:target:action:`) and is placed in the
current autorelease pool. It will be released when the pool drains (typically at the end of the run-loop iteration).

**Caller must ensure the object is retained before the pool drains**, either by:

- passing it to `addSubview:` / another ObjC method that retains it, or
- calling `.Retain()` explicitly if the lifetime must outlive the immediate call chain.

```csharp
var btn = NsButton.CreateAuto(title, handler);  // autoreleased
btn.Retain();                                   // take ownership if storing long-term
// ...
btn.Release();                                  // when done
```

### `CreateOwned` / `CloneOwned` — owned pointer

The returned object was created via `[[Class alloc] init...]` and transferred in full ownership to the C# side (retain
count = 1, no autorelease). **Caller is responsible for calling `.Release()`** when the object is no longer needed.

```csharp
var str = NsString.CloneOwned("hello");
ObjC.SendMessage(target, SomeSel, str.inner);
str.Release();  // required
```

### `Borrow*(...)` — borrowed pointer

The returned wrapper does not transfer ownership. The underlying ObjC object is alive only as long as its owner (parent
view, window, status bar, etc.) keeps it. **Do not call `.Release()` on a borrowed value.**

```csharp
var view = window.BorrowContentView();  // valid while window is alive
view.AddSubview(child);
```
