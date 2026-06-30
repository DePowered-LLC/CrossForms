namespace CrossForms.Native.MacOS.Internals;


internal class NsLayoutConstraint: NsObject, IObjClass<NsLayoutConstraint> {
	public new static readonly ObjClass<NsLayoutConstraint> Proto = ObjClass<NsLayoutConstraint>.Get("NSLayoutConstraint");

	private static readonly IntPtr ConstraintWithItemSel =
		ObjSelector.Get("constraintWithItem:attribute:relatedBy:toItem:attribute:multiplier:constant:");

	private static readonly IntPtr IsActiveSel = ObjSelector.Get("isActive");
	private static readonly IntPtr SetActiveSel = ObjSelector.Get("setActive:");

	public static NsLayoutConstraint CreateAuto (
		NsView target, NsLayoutAttribute controlledAttr, NsLayoutRelation relation,
		NsView source, NsLayoutAttribute parameterAttr, float multiplier, float constant
	) {
		var inner = ObjC.SendMessage(
			Proto.inner, 
			ConstraintWithItemSel, 
			target.inner, 
			(int) controlledAttr, 
			(int) relation,
			source.inner, 
			(int) parameterAttr, 
			multiplier, 
			constant
		);
		return Borrow(inner);
	}

	public new static NsLayoutConstraint Borrow (IntPtr ptr) => new(ptr);
	protected NsLayoutConstraint (IntPtr ptr): base(ptr) {}

	public bool Active {
		get => ObjC.SendMessage(inner, IsActiveSel) != IntPtr.Zero;
		set => ObjC.SendMessage(inner, SetActiveSel, (byte) (value ? 1 : 0));
	}
}
