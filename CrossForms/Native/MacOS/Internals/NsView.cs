using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;

public class NsView: NsNested {
	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, CGPoint position);

	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, CGSize size);

	// private static ObjClass proto = ObjClass.Get("NSView");

	private static readonly IntPtr ADD_SUBVIEW = ObjSelector.Get("addSubview:");
	public void AddSubview (NsView child) {
		ObjC.SendMessage(inner, ADD_SUBVIEW, child.inner);
	}

	private static readonly IntPtr LEADING_ANCHOR = ObjSelector.Get("leadingAnchor");
	internal NsLayoutXAxisAnchor LeadingAnchor => new(ObjC.SendMessage(inner, LEADING_ANCHOR));

	private static readonly IntPtr TOP_ANCHOR = ObjSelector.Get("topAnchor");
	internal NsLayoutYAxisAnchor TopAnchor => new(ObjC.SendMessage(inner, TOP_ANCHOR));

	private static readonly IntPtr BOTTOM_ANCHOR = ObjSelector.Get("bottomAnchor");
	internal NsLayoutYAxisAnchor BottomAnchor => new(ObjC.SendMessage(inner, BOTTOM_ANCHOR));

	private static readonly IntPtr WIDTH_ANCHOR = ObjSelector.Get("widthAnchor");
	internal NsLayoutDimension WidthAnchor => new(ObjC.SendMessage(inner, WIDTH_ANCHOR));

	private static readonly IntPtr HEIGHT_ANCHOR = ObjSelector.Get("heightAnchor");
	internal NsLayoutDimension HeightAnchor => new(ObjC.SendMessage(inner, HEIGHT_ANCHOR));

	private static readonly IntPtr WINDOW = ObjSelector.Get("window");
	public NsWindow? Window {
		get {
			var ptr = ObjC.SendMessage(inner, WINDOW);
			return ptr == IntPtr.Zero ? null : new NsWindow { inner = ptr };
		}
	}

	private static readonly IntPtr SUPERVIEW = ObjSelector.Get("superview");
	public NsView? SuperView {
		get {
			var id = ObjC.SendMessage(inner, SUPERVIEW);
			if (id == IntPtr.Zero) return null;
			else return new () { inner = id };
		}
	}

	private static readonly IntPtr FRAME = ObjSelector.Get("frame");
	public CGRect Frame => ObjC.SendMessage<CGRect>(inner, FRAME);

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
		get => ObjC.SendMessage(inner, GET_AUTORESIZE_CONSTRAINING) != IntPtr.Zero;
		set => ObjC.SendMessage(inner, SET_AUTORESIZE_CONSTRAINING, value);
	}
}

internal class NsLayoutYAxisAnchor: NativeManaged<IntPtr> {
	public NsLayoutYAxisAnchor (IntPtr inner) { this.inner = inner; }

	private static readonly IntPtr EQ_ANCHOR = ObjSelector.Get("constraintEqualToAnchor:constant:");
	public NsLayoutConstraint ConstraintToAnchor (NsLayoutYAxisAnchor anchor, double offset) {
		return new(ObjC.SendMessage(inner, EQ_ANCHOR, anchor.inner, offset));
	}
}

internal class NsLayoutXAxisAnchor: NativeManaged<IntPtr> {
	public NsLayoutXAxisAnchor (IntPtr inner) { this.inner = inner; }

	private static readonly IntPtr EQ_ANCHOR = ObjSelector.Get("constraintEqualToAnchor:constant:");
	public NsLayoutConstraint ConstraintToAnchor (NsLayoutXAxisAnchor anchor, double offset) {
		return new(ObjC.SendMessage(inner, EQ_ANCHOR, anchor.inner, offset));
	}
}

internal class NsLayoutDimension: NativeManaged<IntPtr> {
	public NsLayoutDimension (IntPtr inner) { this.inner = inner; }

	private static readonly IntPtr EQ_CONSTANT = ObjSelector.Get("constraintEqualToConstant:");
	public NsLayoutConstraint ConstraintToConstant (double constant) {
		return new(ObjC.SendMessage(inner, EQ_CONSTANT, constant));
	}
}
