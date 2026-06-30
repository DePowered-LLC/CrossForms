namespace CrossForms.Native.MacOS.Internals;


internal class NsLayoutDimension: NsObject, IObjClass<NsLayoutDimension> {
	private static readonly IntPtr EqConstantSel = ObjSelector.Get("constraintEqualToConstant:");

	public new static NsLayoutDimension Borrow (IntPtr ptr) => new(ptr);
	private NsLayoutDimension (IntPtr ptr): base(ptr) {}

	public NsLayoutConstraint ConstraintToConstantAuto (double constant) {
		return NsLayoutConstraint.Borrow(ObjC.SendMessage(inner, EqConstantSel, constant));
	}
}
