using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


public class NsString: NsObject, IObjClass<NsString> {
	private new static readonly ObjClass<NsString> Proto = ObjClass<NsString>.Get("NSString");
	
	private static readonly IntPtr InitWithUtf8Sel = ObjSelector.Get("initWithUTF8String:");
	private static readonly IntPtr GetUtf8Sel = ObjSelector.Get("UTF8String");

	public static NsString CloneOwned (string value) {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, InitWithUtf8Sel, value); 
		return result;
	}
	
	public static readonly NsString Empty = CloneOwned("");

	public new static NsString Borrow (IntPtr ptr) => new(ptr);
	protected NsString (IntPtr ptr): base(ptr) {}

	public string Value => Marshal.PtrToStringUTF8(ObjC.SendMessage(inner, GetUtf8Sel))!;
}
