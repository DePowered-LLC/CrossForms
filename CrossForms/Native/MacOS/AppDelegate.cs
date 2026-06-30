using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


internal class AppDelegate: NsObject, IObjClass<AppDelegate> {
	public new static readonly ObjClass<AppDelegate> Proto = NsObject.Proto.NewSubClass<AppDelegate>(
		"AppDelegate",
		cls => {
			NoOpFnPtr = cls.AddMethod(
				"noop", 
				(NsEventDispatcher.DispatchEventFn) ((_, _, _) => {}), 
				"v@:@"
			);
		}
	);

	public static IntPtr NoOpFnPtr { get; private set; }
	public MethodRef RefNoOp => new(inner, NoOpFnPtr);
	
	public static AppDelegate CreateOwned () {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, "init");
		return result;
	}
	
	public new static AppDelegate Borrow (IntPtr ptr) => new(ptr);
	protected AppDelegate (IntPtr ptr): base(ptr) {}
}

public record struct MethodRef (IntPtr DelegatePtr, IntPtr MethodPtr);
