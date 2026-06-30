namespace CrossForms.Native.MacOS.Internals;


internal class NsLayoutXAxisAnchor: NsObject, IObjClass<NsLayoutXAxisAnchor> {
	private static readonly IntPtr EqAnchorSel = ObjSelector.Get("constraintEqualToAnchor:constant:");

	public new static NsLayoutXAxisAnchor Borrow (IntPtr ptr) => new(ptr);
	private NsLayoutXAxisAnchor (IntPtr ptr): base(ptr) {}

	public NsLayoutConstraint ConstraintToAnchorAuto (NsLayoutXAxisAnchor anchor, double offset) {
		return NsLayoutConstraint.Borrow(ObjC.SendMessage(inner, EqAnchorSel, anchor.inner, offset));
	}

}
