using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public class NsEvent: NativeManaged<IntPtr> {
	private static readonly IntPtr LocationInWindowSel = ObjSelector.Get("locationInWindow");
	public CgPoint LocationInWindow => ObjC.SendMessage<CgPoint>(inner, LocationInWindowSel);
}
