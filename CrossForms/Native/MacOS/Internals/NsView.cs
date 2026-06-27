using System.Runtime.InteropServices;

using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public partial class NsView: NsNested {
	// private static ObjClass proto = ObjClass.Get("NSView");

	private static readonly IntPtr AddSubviewSel = ObjSelector.Get("addSubview:");

	private static readonly IntPtr LeadingAnchorSel = ObjSelector.Get("leadingAnchor");

	private static readonly IntPtr TopAnchorSel = ObjSelector.Get("topAnchor");

	private static readonly IntPtr BottomAnchorSel = ObjSelector.Get("bottomAnchor");

	private static readonly IntPtr WidthAnchorSel = ObjSelector.Get("widthAnchor");

	private static readonly IntPtr HeightAnchorSel = ObjSelector.Get("heightAnchor");

	private static readonly IntPtr WindowSel = ObjSelector.Get("window");

	private static readonly IntPtr SuperviewSel = ObjSelector.Get("superview");

	private static readonly IntPtr FrameSel = ObjSelector.Get("frame");

	private static readonly IntPtr SetFrameOriginSel = ObjSelector.Get("setFrameOrigin:");

	private static readonly IntPtr SetFrameSizeSel = ObjSelector.Get("setFrameSize:");

	private static readonly IntPtr SetNextKeyViewSel = ObjSelector.Get("setNextKeyView:");

	private static readonly IntPtr GetAutoresizeConstrainingSel =
		ObjSelector.Get("translatesAutoresizingMaskIntoConstraints");

	private static readonly IntPtr SetAutoresizeConstrainingSel =
		ObjSelector.Get("setTranslatesAutoresizingMaskIntoConstraints:");

	internal NsLayoutXAxisAnchor LeadingAnchor => new(ObjC.SendMessage(inner, LeadingAnchorSel));
	internal NsLayoutYAxisAnchor TopAnchor => new(ObjC.SendMessage(inner, TopAnchorSel));
	internal NsLayoutYAxisAnchor BottomAnchor => new(ObjC.SendMessage(inner, BottomAnchorSel));
	internal NsLayoutDimension WidthAnchor => new(ObjC.SendMessage(inner, WidthAnchorSel));
	internal NsLayoutDimension HeightAnchor => new(ObjC.SendMessage(inner, HeightAnchorSel));

	public NsWindow? Window {
		get {
			var ptr = ObjC.SendMessage(inner, WindowSel);
			return ptr == IntPtr.Zero ? null : new NsWindow { inner = ptr };
		}
	}

	public NsView? SuperView {
		get {
			var id = ObjC.SendMessage(inner, SuperviewSel);
			return id == IntPtr.Zero ? null : new NsView { inner = id };
		}
	}

	public CgRect Frame => ObjC.SendMessage<CgRect>(inner, FrameSel);

	public bool TranslatesAutoresizingMaskIntoConstraints {
		get => ObjC.SendMessage(inner, GetAutoresizeConstrainingSel) != IntPtr.Zero;
		set => ObjC.SendMessage(inner, SetAutoresizeConstrainingSel, value ? 1 : 0);
	}

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, CgPoint position);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, CgSize size);

	public void AddSubview (NsView child) {
		ObjC.SendMessage(inner, AddSubviewSel, child.inner);
	}

	public void SetFrameOrigin (double x, double y) {
		SendMessage(inner, SetFrameOriginSel, new CgPoint { x = x, y = y });
	}

	public void SetFrameOrigin (CgPoint point) {
		SendMessage(inner, SetFrameOriginSel, point);
	}

	public void SetFrameSize (double width, double height) {
		SendMessage(inner, SetFrameSizeSel, new CgSize { width = width, height = height });
	}

	public void SetFrameSize (CgSize size) {
		SendMessage(inner, SetFrameSizeSel, size);
	}

	public void SetNextKeyView (NsView view) {
		ObjC.SendMessage(inner, SetNextKeyViewSel, view.inner);
	}

	internal void ApplyConstraints (NsWindow window, int x, int y, double width, double height) {
		var view = window.ContentView;
		LeadingAnchor.ConstraintToAnchor(view.LeadingAnchor, x).Active = true;
		TopAnchor.ConstraintToAnchor(view.TopAnchor, y).Active = true;
		WidthAnchor.ConstraintToConstant(width).Active = true;
		HeightAnchor.ConstraintToConstant(height).Active = true;
	}
}

internal class NsLayoutYAxisAnchor: NativeManaged<IntPtr> {
	private static readonly IntPtr EqAnchorSel = ObjSelector.Get("constraintEqualToAnchor:constant:");

	public NsLayoutYAxisAnchor (IntPtr inner) {
		this.inner = inner;
	}

	public NsLayoutConstraint ConstraintToAnchor (NsLayoutYAxisAnchor anchor, double offset) {
		return new NsLayoutConstraint(ObjC.SendMessage(inner, EqAnchorSel, anchor.inner, offset));
	}
}

internal class NsLayoutXAxisAnchor: NativeManaged<IntPtr> {
	private static readonly IntPtr EqAnchorSel = ObjSelector.Get("constraintEqualToAnchor:constant:");

	public NsLayoutXAxisAnchor (IntPtr inner) {
		this.inner = inner;
	}

	public NsLayoutConstraint ConstraintToAnchor (NsLayoutXAxisAnchor anchor, double offset) {
		return new NsLayoutConstraint(ObjC.SendMessage(inner, EqAnchorSel, anchor.inner, offset));
	}
}

internal class NsLayoutDimension: NativeManaged<IntPtr> {
	private static readonly IntPtr EqConstantSel = ObjSelector.Get("constraintEqualToConstant:");

	public NsLayoutDimension (IntPtr inner) {
		this.inner = inner;
	}

	public NsLayoutConstraint ConstraintToConstant (double constant) {
		return new NsLayoutConstraint(ObjC.SendMessage(inner, EqConstantSel, constant));
	}
}
