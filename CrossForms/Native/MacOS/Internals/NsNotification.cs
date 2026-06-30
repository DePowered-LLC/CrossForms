namespace CrossForms.Native.MacOS.Internals;


internal class NsNotification: NsObject, IObjClass<NsNotification> {
	public new static readonly ObjClass<NsNotification> Proto = ObjClass<NsNotification>.Get("NSNotification");

	public new static NsNotification Borrow (IntPtr ptr) => new(ptr);
	protected NsNotification (IntPtr ptr): base(ptr) {}
	
	public IntPtr Object => ObjC.SendMessage(inner, ObjSelector.Get("object"));
}
