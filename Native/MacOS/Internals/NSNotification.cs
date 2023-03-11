namespace CrossForms.Native.MacOS;

internal class NSNotification: NSObject {
	public static ObjClass proto = ObjClass.Get("NSNotification");

//todo:commonize
	public IntPtr inner;
	public IntPtr Object => ObjC.SendMessage(inner, ObjSelector.Get("object"));
}
