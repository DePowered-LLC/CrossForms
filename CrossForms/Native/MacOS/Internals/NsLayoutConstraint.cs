using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;

internal class NsLayoutConstraint: NativeManaged<IntPtr> {
	private static readonly ObjClass proto = ObjClass.Get("NSLayoutConstraint");
	public NsLayoutConstraint (IntPtr inner) {
		this.inner = inner;
	}

	private static readonly IntPtr CONSTRAINT_WITH_ITEM = ObjSelector.Get("constraintWithItem:attribute:relatedBy:toItem:attribute:multiplier:constant:");
	public NsLayoutConstraint (NsView target, NsLayoutAttribute controlledAttr, NsLayoutRelation relation, NsView source, NsLayoutAttribute parameterAttr, float multiplier, float constant) {
		inner = ObjC.SendMessage(proto.inner, CONSTRAINT_WITH_ITEM, target.inner, (int) controlledAttr, (int) relation, source.inner, (int) parameterAttr, multiplier, constant);
	}

	private static readonly IntPtr IS_ACTIVE = ObjSelector.Get("isActive");
	private static readonly IntPtr SET_ACTIVE = ObjSelector.Get("setActive:");
	public bool Active {
		get { return ObjC.SendMessage(inner, IS_ACTIVE) != IntPtr.Zero; }
		set { ObjC.SendMessage(inner, SET_ACTIVE, value); }
	}
}
