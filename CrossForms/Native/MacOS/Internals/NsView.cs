namespace CrossForms.Native.MacOS.Internals;


public class NsView: NsNested, IObjClass<NsView> {
	public new static ObjClass<NsView> Proto = ObjClass<NsView>.Get("NSView");

	private static readonly IntPtr AddSubviewSel = ObjSelector.Get("addSubview:");
	private static readonly IntPtr LeadingAnchorSel = ObjSelector.Get("leadingAnchor");
	private static readonly IntPtr TopAnchorSel = ObjSelector.Get("topAnchor");
	private static readonly IntPtr BottomAnchorSel = ObjSelector.Get("bottomAnchor");
	private static readonly IntPtr GetWidthAnchorSel = ObjSelector.Get("widthAnchor");
	private static readonly IntPtr GetHeightAnchorSel = ObjSelector.Get("heightAnchor");
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

	private NsLayoutXAxisAnchor BorrowLeadingAnchor () {
		return NsLayoutXAxisAnchor.Borrow(ObjC.SendMessage(inner, LeadingAnchorSel));
	}
	
	private NsLayoutYAxisAnchor BorrowTopAnchor () {
		return NsLayoutYAxisAnchor.Borrow(ObjC.SendMessage(inner, TopAnchorSel));
	}
	
	private NsLayoutYAxisAnchor BorrowBottomAnchor () {
		return NsLayoutYAxisAnchor.Borrow(ObjC.SendMessage(inner, BottomAnchorSel));
	}
	
	private NsLayoutDimension BorrowWidthAnchor () {
		return NsLayoutDimension.Borrow(ObjC.SendMessage(inner, GetWidthAnchorSel));
	}
	
	private NsLayoutDimension BorrowHeightAnchor () {
		return NsLayoutDimension.Borrow(ObjC.SendMessage(inner, GetHeightAnchorSel));
	}
	

	public new static NsView Borrow (IntPtr ptr) => new(ptr);
	protected NsView (IntPtr ptr): base(ptr) {}

	public NsWindow? BorrowWindow () {
		var ptr = ObjC.SendMessage(inner, WindowSel);
		return ptr == IntPtr.Zero ? null : NsWindow.Borrow(ptr);
	}

	public NsView? BorrowSuperView () {
		var ptr = ObjC.SendMessage(inner, SuperviewSel);
		return ptr == IntPtr.Zero ? null : Borrow(ptr);
	}

	public CgRect Frame => ObjC.SendMessage<CgRect>(inner, FrameSel);

	public bool TranslatesAutoresizingMaskIntoConstraints {
		get => ObjC.SendMessage(inner, GetAutoresizeConstrainingSel) != IntPtr.Zero;
		set => ObjC.SendMessage(inner, SetAutoresizeConstrainingSel, value ? 1 : 0);
	}

	public void AddSubview (NsView child) {
		ObjC.SendMessage(inner, AddSubviewSel, child.inner);
	}

	public void SetFrameOrigin (double x, double y) {
		ObjC.SendMessage(inner, SetFrameOriginSel, new CgPoint { x = x, y = y });
	}

	public void SetFrameOrigin (CgPoint point) {
		ObjC.SendMessage(inner, SetFrameOriginSel, point);
	}

	public void SetFrameSize (double width, double height) {
		ObjC.SendMessage(inner, SetFrameSizeSel, new CgSize { width = width, height = height });
	}

	public void SetFrameSize (CgSize size) {
		ObjC.SendMessage(inner, SetFrameSizeSel, size);
	}

	public void SetNextKeyView (NsView view) {
		ObjC.SendMessage(inner, SetNextKeyViewSel, view.inner);
	}

	internal void ApplyConstraints (NsWindow window, int x, int y, double width, double height) {
		var view = window.BorrowContentView();
		BorrowLeadingAnchor().ConstraintToAnchorAuto(view.BorrowLeadingAnchor(), x).Active = true;
		BorrowTopAnchor().ConstraintToAnchorAuto(view.BorrowTopAnchor(), y).Active = true;
		BorrowWidthAnchor().ConstraintToConstantAuto(width).Active = true;
		BorrowHeightAnchor().ConstraintToConstantAuto(height).Active = true;
	}
}