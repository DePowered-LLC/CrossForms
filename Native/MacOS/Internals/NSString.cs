using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

public class NSString: NativeManaged<IntPtr> {
	private static readonly ObjClass proto = ObjClass.Get("NSString");
	private static readonly IntPtr FROM_UTF8 = ObjSelector.Get("stringWithUTF8String:");
	public NSString (string value) {
		inner = ObjC.SendMessage(proto.inner, FROM_UTF8, value);
	}
}
