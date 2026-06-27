using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public class NsApplication: NativeManaged<IntPtr> {
	public enum ActivationPolicy {
		Regular,
		Accessory,
		Prohibited
	}

	private static readonly ObjClass Proto = ObjClass.Get("NSApplication");

	public static NsApplication Current = null!;
	private static readonly IntPtr SharedApp = ObjSelector.Get("sharedApplication");

	private static readonly IntPtr TerminateSel = ObjSelector.Get("terminate:");

	private static readonly IntPtr CurrentEventSel = ObjSelector.Get("currentEvent");

	private readonly IntPtr _activateIgnoringOtherAppsSel = ObjSelector.Get("activateIgnoringOtherApps:");

	private readonly IntPtr _setActivationPolicySel = ObjSelector.Get("setActivationPolicy:");
	private readonly IntPtr _setDelegateSel = ObjSelector.Get("setDelegate:");

	private AppDelegate _appDelegate = null!;

	public NsApplication () {
		Current = this;
		Proto.Construct(this, SharedApp);
		AppDelegate = new AppDelegate();
	}

	public bool IsRunning => ObjC.SendMessage(inner, ObjSelector.Get("isRunning")) != IntPtr.Zero;

	internal AppDelegate AppDelegate {
		get => _appDelegate;
		set {
			ObjC.SendMessage(inner, _setDelegateSel, value.inner);
			_appDelegate = value;
		}
	}

	public NsEvent? CurrentEvent {
		get {
			var ptr = ObjC.SendMessage(inner, CurrentEventSel);
			return ptr == IntPtr.Zero ? null : new NsEvent { inner = ptr };
		}
	}

	public void SetActivationPolicy (ActivationPolicy value) {
		ObjC.SendMessage(inner, _setActivationPolicySel, (int) value);
	}

	public void Run () {
		CocoaSynchronizationContext.Install();
		ObjC.SendMessage(inner, _activateIgnoringOtherAppsSel, 1);
		ObjC.SendMessage(inner, "run");
	}

	public void Terminate () {
		ObjC.SendMessage(inner, TerminateSel, IntPtr.Zero);
	}
}
