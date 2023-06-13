using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

public class NSView: NSNested {
	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, CGPoint position);

	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, CGSize size);

	// private static ObjClass proto = ObjClass.Get("NSView");

	private static readonly IntPtr ADD_SUBVIEW = ObjSelector.Get("addSubview:");
	public void AddSubview (NSView child) {
		ObjC.SendMessage(inner, ADD_SUBVIEW, child.inner);
	}

	private static readonly IntPtr TOP_ANCHOR = ObjSelector.Get("topAnchor");
	internal NSLayoutYAxisAnchor TopAnchor => new(ObjC.SendMessage(inner, TOP_ANCHOR));

	private static readonly IntPtr BOTTOM_ANCHOR = ObjSelector.Get("bottomAnchor");
	internal NSLayoutYAxisAnchor BottomAnchor => new(ObjC.SendMessage(inner, BOTTOM_ANCHOR));

	private static readonly IntPtr HEIGHT_ANCHOR = ObjSelector.Get("heightAnchor");
	internal NSLayoutYAxisAnchor HeightAnchor => new(ObjC.SendMessage(inner, HEIGHT_ANCHOR));

	private static readonly IntPtr SUPERVIEW = ObjSelector.Get("superview");
	public NSView? SuperView {
		get {
			var id = ObjC.SendMessage(inner, SUPERVIEW);
			if (id == IntPtr.Zero) return null;
			else return new () { inner = id };
		}
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

	private static readonly IntPtr SET_FRAME_SIZE = ObjSelector.Get("setFrameSize:");
	public void SetFrameSize (double width, double height) {
		SendMessage(inner, SET_FRAME_SIZE, new CGSize { width = width, height = height });
	}
	public void SetFrameSize (CGSize size) {
		SendMessage(inner, SET_FRAME_SIZE, size);
	}

	private static readonly IntPtr GET_AUTORESIZE_CONSTRAINING = ObjSelector.Get("translatesAutoresizingMaskIntoConstraints");
	private static readonly IntPtr SET_AUTORESIZE_CONSTRAINING = ObjSelector.Get("setTranslatesAutoresizingMaskIntoConstraints:");
	public bool TranslatesAutoresizingMaskIntoConstraints {
		get { return ObjC.SendMessage(inner, GET_AUTORESIZE_CONSTRAINING) != IntPtr.Zero; }
		set { ObjC.SendMessage(inner, SET_AUTORESIZE_CONSTRAINING, value); }
	}
}

internal class NSLayoutYAxisAnchor: NativeManaged<IntPtr> {
	public NSLayoutYAxisAnchor (IntPtr inner) {
		this.inner = inner;
	}

	private static readonly IntPtr EQ_ANCHOR = ObjSelector.Get("constraintEqualToAnchor:constant:");
	public NSLayoutConstraint ConstraintToAnchor (NSLayoutYAxisAnchor anchor, double offset) {
		return new(ObjC.SendMessage(inner, EQ_ANCHOR, anchor.inner, offset));
	}
}
