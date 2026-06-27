using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


internal class NsLayoutConstraint: NativeManaged<IntPtr> {
	private static readonly ObjClass Proto = ObjClass.Get("NSLayoutConstraint");

	private static readonly IntPtr ConstraintWithItemSel =
		ObjSelector.Get("constraintWithItem:attribute:relatedBy:toItem:attribute:multiplier:constant:");

	private static readonly IntPtr IsActiveSel = ObjSelector.Get("isActive");
	private static readonly IntPtr SetActiveSel = ObjSelector.Get("setActive:");

	public NsLayoutConstraint (IntPtr inner) {
		this.inner = inner;
	}

	public NsLayoutConstraint (
		NsView target, NsLayoutAttribute controlledAttr, NsLayoutRelation relation,
		NsView source, NsLayoutAttribute parameterAttr, float multiplier, float constant
	) {
		inner = ObjC.SendMessage(
			Proto.inner, ConstraintWithItemSel, target.inner, (int) controlledAttr, (int) relation,
			source.inner, (int) parameterAttr, multiplier, constant
		);
	}

	public bool Active {
		get => ObjC.SendMessage(inner, IsActiveSel) != IntPtr.Zero;
		set => ObjC.SendMessage(inner, SetActiveSel, (byte) (value ? 1 : 0));
	}
}
