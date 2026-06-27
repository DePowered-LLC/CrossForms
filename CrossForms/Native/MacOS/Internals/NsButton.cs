using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


public partial class NsButton: NsControl {
	private static readonly ObjClass Proto = ObjClass.Get("NSButton");
	private static readonly IntPtr TitledButtonSel = ObjSelector.Get("buttonWithTitle:target:action:");

	private static readonly IntPtr SetTargetSel = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SetActionSel = ObjSelector.Get("setAction:");

	private static readonly IntPtr GetTitleSel = ObjSelector.Get("title");
	private static readonly IntPtr SetTitleSel = ObjSelector.Get("setTitle:");

	public NsButton (string title) {
		inner = SendMessage(Proto.inner, TitledButtonSel, new NsString(title).inner,
			NsApplication.Current.AppDelegate.inner, AppDelegate.NO_OP);
		TranslatesAutoresizingMaskIntoConstraints = false;
	}

	public string Title {
		get => new NsString(ObjC.SendMessage(inner, GetTitleSel)).Value;
		set => ObjC.SendMessage(inner, SetTitleSel, new NsString(value).inner);
	}

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial IntPtr SendMessage (
		IntPtr cls, IntPtr selector, IntPtr title, IntPtr appDelegate,
		IntPtr handler
	);

	public void OnClick (Action handler) {
		PreRegisterEvent("click", handler, (dispatcher, selector) => {
			ObjC.SendMessage(inner, SetTargetSel, dispatcher.dispatcherInstance);
			ObjC.SendMessage(inner, SetActionSel, selector);
		});
	}
}
