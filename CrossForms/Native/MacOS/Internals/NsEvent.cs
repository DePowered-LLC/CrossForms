using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public class NsEvent: NsObject, IObjClass<NsEvent> {
	public new static readonly ObjClass<NsEvent> Proto = ObjClass<NsEvent>.Get("NSEvent");

	private static readonly IntPtr LocationInWindowSel = ObjSelector.Get("locationInWindow");
	private static readonly IntPtr TypeSel = ObjSelector.Get("type");
	private static readonly IntPtr MouseLocationSel = ObjSelector.Get("mouseLocation");
	
	public new static NsEvent Borrow (IntPtr ptr) => new(ptr);
	protected NsEvent (IntPtr ptr): base(ptr) {}

	public CgPoint LocationInWindow => ObjC.SendMessage<CgPoint>(inner, LocationInWindowSel);

	// todo: enum NSEventType: LeftMouseDown=1, LeftMouseUp=2, RightMouseDown=3, RightMouseUp=4
	public long Type => (long) ObjC.SendMessage(inner, TypeSel);

	public static CgPoint MouseLocation => ObjC.SendMessage<CgPoint>(Proto.inner, MouseLocationSel);
}
