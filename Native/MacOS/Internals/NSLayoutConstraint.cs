using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

internal class NSLayoutConstraint: NativeManaged<IntPtr> {
	private static readonly ObjClass proto = ObjClass.Get("NSLayoutConstraint");
	public NSLayoutConstraint (IntPtr inner) {
		this.inner = inner;
	}

	private static readonly IntPtr CONSTRAINT_WITH_ITEM = ObjSelector.Get("constraintWithItem:attribute:relatedBy:toItem:attribute:multiplier:constant:");
	public NSLayoutConstraint (NSView target, NSLayoutAttribute controlledAttr, NSLayoutRelation relation, NSView source, NSLayoutAttribute parameterAttr, float multiplier, float constant) {
		inner = ObjC.SendMessage(proto.inner, CONSTRAINT_WITH_ITEM, target.inner, (int) controlledAttr, (int) relation, source.inner, (int) parameterAttr, multiplier, constant);
	}

	private static readonly IntPtr IS_ACTIVE = ObjSelector.Get("isActive");
	private static readonly IntPtr SET_ACTIVE = ObjSelector.Get("setActive:");
	public bool Active {
		get { return ObjC.SendMessage(inner, IS_ACTIVE) != IntPtr.Zero; }
		set { ObjC.SendMessage(inner, SET_ACTIVE, value); }
	}
}
