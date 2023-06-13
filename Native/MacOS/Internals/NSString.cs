using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

public class NSString: NativeManaged<IntPtr> {
	private static readonly ObjClass proto = ObjClass.Get("NSString");
	private static readonly IntPtr FROM_UTF8 = ObjSelector.Get("stringWithUTF8String:");
	public NSString (string value) {
		inner = ObjC.SendMessage(proto.inner, FROM_UTF8, value);
	}

	public NSString (IntPtr ptr) {
		inner = ptr;
	}

	private static readonly IntPtr UTF8_STRING = ObjSelector.Get("UTF8String");
	public string Value => Marshal.PtrToStringUTF8(ObjC.SendMessage(inner, UTF8_STRING));
}
