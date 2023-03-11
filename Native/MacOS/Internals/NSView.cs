using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS;

public class NSView: NSNested {
	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, CGPoint position);

	// private static ObjClass proto = ObjClass.Get("NSView");

	private static readonly IntPtr ADD_SUBVIEW = ObjSelector.Get("addSubview:");
	public void AddSubview (NSView child) {
		ObjC.SendMessage(inner, ADD_SUBVIEW, child.inner);
	}

	private static readonly IntPtr FRAME = ObjSelector.Get("frame");
	public CGRect Frame => ObjC.SendMessage<CGRect>(inner, FRAME).value;

	private static readonly IntPtr SET_FRAME_ORIGIN = ObjSelector.Get("setFrameOrigin:");
	public void SetFrameOrigin (double x, double y) {
		SendMessage(inner, SET_FRAME_ORIGIN, new CGPoint { x = x, y = y });
	}
	public void SetFrameOrigin (CGPoint point) {
		SendMessage(inner, SET_FRAME_ORIGIN, point);
	}
}
