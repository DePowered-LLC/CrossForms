namespace CrossForms.Native.MacOS.Internals;


public class NsStatusBarButton: NsButton, IObjClass<NsStatusBarButton> {
	public new static readonly ObjClass<NsStatusBarButton> Proto = ObjClass<NsStatusBarButton>.Get("NSStatusBarButton");

	public new static NsStatusBarButton Borrow (IntPtr ptr) => new(ptr);
	protected NsStatusBarButton (IntPtr ptr): base(ptr) {}
}
