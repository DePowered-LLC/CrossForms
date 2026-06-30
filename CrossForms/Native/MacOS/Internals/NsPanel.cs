using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public class NsPanel: NsWindow, IObjClass<NsPanel> {
	private new static readonly ObjClass<NsPanel> Proto = ObjClass<NsPanel>.Get("NSPanel");

	private static readonly IntPtr PanelInitSel = ObjSelector.Get("initWithContentRect:styleMask:backing:defer:");
	private static readonly IntPtr SetLevelSel = ObjSelector.Get("setLevel:");
	private static readonly IntPtr ActivateSel = ObjSelector.Get("activateIgnoringOtherApps:");
	private static readonly IntPtr ShowSel = ObjSelector.Get("makeKeyAndOrderFront:");
	private static readonly IntPtr HideSel = ObjSelector.Get("orderOut:");
	private static readonly IntPtr IsVisibleSel = ObjSelector.Get("isVisible");

	// Window loses key status when user clicks another window
	private static readonly NsString ResignKeyNoteName = NsString.CloneOwned("NSWindowDidResignKeyNotification");
	// App deactivates when user switches to another app
	private static readonly NsString AppResignActiveName = NsString.CloneOwned("NSApplicationWillResignActiveNotification");
	
	public static NsPanel CreateOwned (double width, double height) {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, PanelInitSel, new CgRect(0, 0, width, height), 0L /* todo: enum (Borderless) */, 2L, 0);
		
		// todo: enum NSFloatingWindowLevel = 3: above normal windows, below menus.
		ObjC.SendMessage(result.inner, SetLevelSel, (IntPtr) 3);
		result.SubscribeHideNotifications();
		return result;
	}
	
	public new static NsPanel Borrow (IntPtr ptr) => new(ptr);
	protected NsPanel (IntPtr ptr): base(ptr) {}

	// Kept alive to prevent GC collection of the unmanaged function pointer.
	private NsEventDispatcher.DispatchEventFn? _appResignDelegate;

	public bool IsVisible => ObjC.SendMessage(inner, IsVisibleSel) != IntPtr.Zero;

	public void ShowNear (double x, double y, double height) {
		// macOS screen bottom is Y = 0
		SetFrameOrigin(x, y - height);
		// todo: enum
		ObjC.SendMessage(NsApplication.Current.inner, ActivateSel, (IntPtr) 1);
		ObjC.SendMessage(inner, ShowSel, IntPtr.Zero);
	}

	public void Hide () {
		// todo: move to tray logic handler
		ObjC.SendMessage(inner, HideSel, IntPtr.Zero);
		// Return to Accessory (no dock icon, no menu bar) if no form is open.
		if (!ApplicationBase.HasVisibleForm) {
			NsApplication.Current.SetActivationPolicy(NsApplication.ActivationPolicy.Accessory);
		}
	}

	private void SubscribeHideNotifications () {
		// Register a window resign-key handler through the existing AttachEvent/DispatchEvent path.
		// The notification's object is unwrapped to the window pointer, which matches this.inner.
		var resignKeySel = AttachEvent(this, "panelResignKey:", Hide);

		// The app-resign-active notification's object is NSApplication (not our window),
		// so we use a direct delegate that bypasses the source-key map.
		_appResignDelegate = (_, _, _) => Hide();
		var resignActiveSel = DispatcherClass.AddMethod("panelAppResignActive:", _appResignDelegate, "v@:@");

		var center = NsNotificationCenter.BorrowDefault();
		// Observe NSWindowDidResignKeyNotification for our specific window
		center.On(dispatcherInstance, resignKeySel, ResignKeyNoteName, this);
		// Observe NSApplicationWillResignActiveNotification only from our app
		center.On(dispatcherInstance, resignActiveSel, AppResignActiveName, NsApplication.Current);
	}

	public void UnsubscribeHideNotifications () {
		NsNotificationCenter.BorrowDefault().Off(dispatcherInstance);
	}
}
