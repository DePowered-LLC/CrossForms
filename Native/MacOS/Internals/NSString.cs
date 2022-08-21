using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

public class NSString: NativeManaged<IntPtr> {
	private static ObjClass proto;
	static NSString () {
		var proto = ObjClass.TryGet("NSString");
		if (proto == null) throw new Exception("Class NSString not registered");
		else NSString.proto = proto;
	}

	public NSString (string value) {
		proto.Construct(this);
		unsafe {
			fixed (char* valuePtr = value) {
				inner = ObjC.SendMessage(inner, "initWithCharacters:length:", (IntPtr) valuePtr, (IntPtr) value.Length);

				// if (autorelease)
				// 	NSObject.DangerousAutorelease (handle);

				// return handle;
			}
		}
		// inner = proto.CallRaw("stringWithUTF8String:", value);
	}
}
