using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS;

public class NSButton: NSControl {
	private static ObjClass proto = ObjClass.Get("NSButton");

	public NSButton (string title) {
		proto.Construct(this, "buttonWithTitle:target:action:", new NSString(title), NSApplication.current.appDelegate, AppDelegate.noOpSel);
	}

	public void OnClick (Action handler) {
		PreRegisterEvent("click", handler, (dispatcher, selector) => {
			ObjC.SendMessage(inner, "setTarget:", dispatcher.dispatcherInstance);
			ObjC.SendMessage(inner, "setAction:", selector);
		});
	}

	public string title {
		get { return null; }
		set { ObjC.SendMessage(inner, "setTitle:", new NSString(value)); }
	}
}
