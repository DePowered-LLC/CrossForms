namespace CrossForms.Native.MacOS.Internals;


public class NsWindow: NsEventDispatcher, IObjClass<NsWindow> {
	public delegate void WindowWillCloseFn (IntPtr self, IntPtr sel, IntPtr notification);

	[Flags]
	public enum StyleMask: long {
		Borderless = 0L,
		Titled = 1L,
		Closable = 2L,
		Miniaturizable = 4L,
		Resizable = 8L
	}

	public new static readonly ObjClass<NsWindow> Proto = ObjClass<NsWindow>.Get("NSWindow");

	private static readonly IntPtr InitContentFrameSel = ObjSelector.Get("initWithContentRect:styleMask:backing:defer:");
	private static readonly IntPtr MakeKeyAndOrderFrontSel = ObjSelector.Get("makeKeyAndOrderFront:");

	private static readonly IntPtr GetTitleSel = ObjSelector.Get("title");
	private static readonly IntPtr SetTitleSel = ObjSelector.Get("setTitle:");

	private static readonly IntPtr FrameSel = ObjSelector.Get("frame");
	private static readonly IntPtr SetFrameOriginSel = ObjSelector.Get("setFrameOrigin:");
	private static readonly IntPtr SetInitialFirstResponderSel = ObjSelector.Get("setInitialFirstResponder:");
	
	public static NsWindow CreateOwned (CgRect frame, StyleMask style, long backingStore, bool defer = false) {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, InitContentFrameSel, frame, (long) style, backingStore, defer ? 1 : 0);
		
		result.MakeKeyAndOrderFront();
		return result;
	}
	
	public new static NsWindow Borrow (IntPtr ptr) => new(ptr);
	protected NsWindow (IntPtr ptr): base(ptr) {}

	// todo: enum
	public void MakeKeyAndOrderFront () {
		ObjC.SendMessage(inner, MakeKeyAndOrderFrontSel, IntPtr.Zero);
	}

	public string Title {
		get => NsString.Borrow(ObjC.SendMessage(inner, GetTitleSel)).Value;
		set {
			var nsTitle = NsString.CloneOwned(value);
			ObjC.SendMessage(inner, SetTitleSel, nsTitle.inner);
			nsTitle.Release();
		}
	}

	public CgRect Frame => ObjC.SendMessage<CgRect>(inner, FrameSel);

	public NsView BorrowContentView () {
		return NsView.Borrow(ObjC.SendMessage(inner, "contentView"));
	}


	public void SetFrameOrigin (double x, double y) {
		ObjC.SendMessage(inner, SetFrameOriginSel, new CgPoint { x = x, y = y });
	}

	public void SetFrameOrigin (CgPoint point) {
		ObjC.SendMessage(inner, SetFrameOriginSel, point);
	}

	public void SetInitialFirstResponder (NsView view) {
		ObjC.SendMessage(inner, SetInitialFirstResponderSel, view.inner);
	}

	public void Append (NsControl child) {
		child.parent = this;
		BorrowContentView().AddSubview(child);
		child.OnAttach();
	}

	public void OnResignKey (Action handle) {
		AttachEvent(this, "windowDidResignKey:", handle);
		ObjC.SendMessage(inner, ObjSelector.Get("setDelegate:"), dispatcherInstance);
	}

	public void OnClose (Action handle) {
		// https://github.com/ritalin/osx_app_in_plain_ziglang/blob/dc03a6884b193828a0545b6c4c502e49ddcd313f/examples/hand_made_binding/src/appKit/widget/NSWindow.zig#L150
		// https://github.com/ritalin/osx_app_in_plain_ziglang/blob/dc03a6884b193828a0545b6c4c502e49ddcd313f/examples/hand_made_binding/src/foundation/runtime/backend_helpers.c#L36
		AttachEvent(this, "windowWillClose:", handle);
		ObjC.SendMessage(inner, ObjSelector.Get("setDelegate:"), dispatcherInstance);
	}
}
