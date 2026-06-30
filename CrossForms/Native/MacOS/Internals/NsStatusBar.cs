namespace CrossForms.Native.MacOS.Internals;


public class NsStatusBar: NsObject, IObjClass<NsStatusBar> {
	public const double SquareItemLength = -2;
	public const double VariableItemLength = -1;
	
	public new static readonly ObjClass<NsStatusBar> Proto = ObjClass<NsStatusBar>.Get("NSStatusBar");
	
	private static readonly IntPtr SystemStatusBarSel = ObjSelector.Get("systemStatusBar");
	private static readonly IntPtr StatusItemWithLengthSel = ObjSelector.Get("statusItemWithLength:");
	
	public new static NsStatusBar Borrow (IntPtr ptr) => new(ptr);
	protected NsStatusBar (IntPtr ptr): base(ptr) {}

	public static readonly NsStatusBar System = Borrow(ObjC.SendMessage(Proto.inner, SystemStatusBarSel));

	public NsStatusItem CreateItemAuto (double itemLength) {
		return NsStatusItem.Borrow(ObjC.SendMessage(inner, StatusItemWithLengthSel, itemLength));
	}
}
