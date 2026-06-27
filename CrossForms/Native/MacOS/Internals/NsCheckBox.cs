using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


public partial class NsCheckBox: NsControl {
	private static readonly ObjClass Proto = ObjClass.Get("NSButton");
	private static readonly IntPtr CheckboxWithTitleSel = ObjSelector.Get("checkboxWithTitle:target:action:");
	private static readonly IntPtr SetTargetSel = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SetActionSel = ObjSelector.Get("setAction:");
	private static readonly IntPtr GetStateSel = ObjSelector.Get("state");
	private static readonly IntPtr SetStateSel = ObjSelector.Get("setState:");

	public NsCheckBox (string title) {
		inner = SendMessage(Proto.inner, CheckboxWithTitleSel, new NsString(title).inner,
			NsApplication.Current.AppDelegate.inner, AppDelegate.NO_OP);
		TranslatesAutoresizingMaskIntoConstraints = false;
	}

	public bool State {
		get => ObjC.SendMessage(inner, GetStateSel) != IntPtr.Zero;
		set => ObjC.SendMessage(inner, SetStateSel, value ? 1 : 0);
	}

	public void OnClick (Action handler) {
		PreRegisterEvent("click", handler, (dispatcher, selector) => {
			ObjC.SendMessage(inner, SetTargetSel, dispatcher.dispatcherInstance);
			ObjC.SendMessage(inner, SetActionSel, selector);
		});
	}

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial IntPtr SendMessage (
		IntPtr cls, IntPtr selector, IntPtr title, IntPtr appDelegate, IntPtr handler
	);
}
