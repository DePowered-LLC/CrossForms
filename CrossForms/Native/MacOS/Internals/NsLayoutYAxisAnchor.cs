namespace CrossForms.Native.MacOS.Internals;


internal class NsLayoutYAxisAnchor: NsObject, IObjClass<NsLayoutYAxisAnchor> {
	private static readonly IntPtr EqAnchorSel = ObjSelector.Get("constraintEqualToAnchor:constant:");

	public new static NsLayoutYAxisAnchor Borrow (IntPtr ptr) => new(ptr);
	private NsLayoutYAxisAnchor (IntPtr ptr): base(ptr) {}


	public NsLayoutConstraint ConstraintToAnchorAuto (NsLayoutYAxisAnchor anchor, double offset) {
		return NsLayoutConstraint.Borrow(ObjC.SendMessage(inner, EqAnchorSel, anchor.inner, offset));
	}
}
