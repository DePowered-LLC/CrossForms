namespace CrossForms.Native.MacOS;

public class NSView: NSNested {
	private static ObjClass proto = ObjClass.Get("NSView");

	public void AddSubview (NSView child) {
		ObjC.SendMessage(inner, "addSubview:", child);
	}

	public void SetFrameOrigin (double x, double y) {
		ObjC.SendMessage(inner, "setFrameOrigin:", new CGPoint { x = x, y = y });
	}
}
