using System.Runtime.InteropServices;

using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public class NsString: NativeManaged<IntPtr> {
	private static readonly ObjClass Proto = ObjClass.Get("NSString");
	private static readonly IntPtr FromUtf8Sel = ObjSelector.Get("stringWithUTF8String:");

	private static readonly IntPtr Utf8StringSel = ObjSelector.Get("UTF8String");

	public NsString (string value) {
		inner = ObjC.SendMessage(Proto.inner, FromUtf8Sel, value);
	}

	public NsString (IntPtr ptr) {
		inner = ptr;
	}

	public string Value => Marshal.PtrToStringUTF8(ObjC.SendMessage(inner, Utf8StringSel))!;
}
