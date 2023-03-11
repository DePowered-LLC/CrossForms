using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

public class NSApplication: NativeManaged<IntPtr> {
	public enum ActivationPolicy { Regular, Accessory, Prohibited }
	private static ObjClass proto = ObjClass.Get("NSApplication");

	public static NSApplication current;
	private static readonly IntPtr SHARED_APP = ObjSelector.Get("sharedApplication");
	public NSApplication () {
		current = this;
		proto.Construct(this, SHARED_APP);
		appDelegate = new AppDelegate();
	}

	private AppDelegate _appDelegate;
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
		ObjC.SendMessage(inner, ACTIVATE_IGNORING_OTHER_APPS, 1);
		ObjC.SendMessage(inner, "run");
	}
}
