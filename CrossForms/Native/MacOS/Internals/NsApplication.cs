using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;

public class NsApplication: NativeManaged<IntPtr> {
	public enum ActivationPolicy { Regular, Accessory, Prohibited }
	private static ObjClass proto = ObjClass.Get("NSApplication");

	public static NsApplication current = null!;
	private static readonly IntPtr SHARED_APP = ObjSelector.Get("sharedApplication");
	public NsApplication () {
		current = this;
		proto.Construct(this, SHARED_APP);
		appDelegate = new AppDelegate();
	}

	public bool isRunning => ObjC.SendMessage(inner, ObjSelector.Get("isRunning")) != IntPtr.Zero;

	private AppDelegate _appDelegate = null!;
	private readonly IntPtr SET_DELEGATE = ObjSelector.Get("setDelegate:");
	internal AppDelegate appDelegate {
		get => _appDelegate;
		set {
			ObjC.SendMessage(inner, SET_DELEGATE, value.inner);
			_appDelegate = value;
		}
	}

	private readonly IntPtr SET_ACTIVATION_POLICY = ObjSelector.Get("setActivationPolicy:");
	public void SetActivationPolicy (ActivationPolicy value) {
		ObjC.SendMessage(inner, SET_ACTIVATION_POLICY, (int) value);
	}

	private readonly IntPtr ACTIVATE_IGNORING_OTHER_APPS = ObjSelector.Get("activateIgnoringOtherApps:");
	public void Run () {
		CocoaSynchronizationContext.Install();
		ObjC.SendMessage(inner, ACTIVATE_IGNORING_OTHER_APPS, 1);
		ObjC.SendMessage(inner, "run");
	}

	private static readonly IntPtr TERMINATE = ObjSelector.Get("terminate:");
	public void Terminate () {
		ObjC.SendMessage(inner, TERMINATE, IntPtr.Zero);
	}

	private static readonly IntPtr CURRENT_EVENT = ObjSelector.Get("currentEvent");
	public NsEvent? CurrentEvent {
		get {
			var ptr = ObjC.SendMessage(inner, CURRENT_EVENT);
			return ptr == IntPtr.Zero ? null : new NsEvent { inner = ptr };
		}
	}
}
