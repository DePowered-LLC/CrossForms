using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public class NsObject: NativeManaged<IntPtr>, IObjClass<NsObject> {
	public static readonly ObjClass<NsObject> Proto = ObjClass<NsObject>.Get("NSObject");
	
	public static NsObject Borrow (IntPtr ptr) => new(ptr);
	protected NsObject (IntPtr ptr) => inner = ptr;

	public void Retain () {
		ObjC.SendMessage(inner, ObjSelector.Get("retain"));
	}

	public void Release () {
		ObjC.ReleaseNsObject(inner);
	}
}
