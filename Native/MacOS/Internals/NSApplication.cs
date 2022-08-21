using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

public class NSApplication: NativeManaged<IntPtr> {
	public enum ActivationPolicy { Regular, Accessory, Prohibited }
	private static ObjClass proto = ObjClass.Get("NSApplication");

	public static NSApplication current;
	public NSApplication () {
		current = this;
		proto.Construct(this, "sharedApplication");
		appDelegate = new AppDelegate();
	}

	private AppDelegate _appDelegate;
	public AppDelegate appDelegate {
		get => _appDelegate;
		set {
			ObjC.SendMessage(inner, "setDelegate:", value);
			_appDelegate = value;
		}
	}

	public void SetActivationPolicy (ActivationPolicy value) {
		ObjC.SendMessage(inner, "setActivationPolicy:", (int) value);
	}

	public void Run () {
		ObjC.SendMessage(inner, "activateIgnoringOtherApps:", true);
		ObjC.SendMessage(inner, "run");
	}
}
