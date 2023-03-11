using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS;

public class NSButton: NSControl {
	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr title, IntPtr appDelegate, IntPtr handler);

	private static readonly ObjClass proto = ObjClass.Get("NSButton");
	private static readonly IntPtr TITLED_BUTTON = ObjSelector.Get("buttonWithTitle:target:action:");
	public NSButton (string title) {
		inner = SendMessage(proto.inner, TITLED_BUTTON, new NSString(title).inner, NSApplication.current.appDelegate.inner, AppDelegate.NO_OP);
	}

	private static readonly IntPtr SET_TARGET = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SET_ACTION = ObjSelector.Get("setAction:");
	public void OnClick (Action handler) {
		PreRegisterEvent("click", handler, (dispatcher, selector) => {
			ObjC.SendMessage(inner, SET_TARGET, dispatcher.dispatcherInstance);
			ObjC.SendMessage(inner, SET_ACTION, selector);
		});
	}

	private static readonly IntPtr SET_TITLE = ObjSelector.Get("setTitle:");
	public string title {
		get { return null; }
		set { ObjC.SendMessage(inner, SET_TITLE, new NSString(value).inner); }
	}
}
