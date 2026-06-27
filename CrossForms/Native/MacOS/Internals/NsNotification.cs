namespace CrossForms.Native.MacOS.Internals;


internal class NsNotification: NsObject {
	public new static ObjClass proto = ObjClass.Get("NSNotification");

//todo:commonize
	public IntPtr inner;
	public IntPtr Object => ObjC.SendMessage(inner, ObjSelector.Get("object"));
}
