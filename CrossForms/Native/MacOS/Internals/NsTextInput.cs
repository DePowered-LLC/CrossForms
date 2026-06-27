using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


public partial class NsTextInput: NsControl {
	private static readonly ObjClass Proto = ObjClass.Get("NSTextField");
	private static readonly IntPtr TextFieldWithStringSel = ObjSelector.Get("textFieldWithString:");
	private static readonly IntPtr GetStringValueSel = ObjSelector.Get("stringValue");
	private static readonly IntPtr SetStringValueSel = ObjSelector.Get("setStringValue:");
	private static readonly IntPtr SetDelegateSel = ObjSelector.Get("setDelegate:");

	public NsTextInput (string text) {
		inner = SendMessage(Proto.inner, TextFieldWithStringSel, new NsString(text).inner);
		TranslatesAutoresizingMaskIntoConstraints = false;
	}

	public string StringValue {
		get => new NsString(ObjC.SendMessage(inner, GetStringValueSel)).Value;
		set => ObjC.SendMessage(inner, SetStringValueSel, new NsString(value).inner);
	}

	public void OnChange (Action handler) {
		PreRegisterEvent("controlTextDidChange:", handler, (dispatcher, _) => {
			ObjC.SendMessage(inner, SetDelegateSel, dispatcher.dispatcherInstance);
		});
	}

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr str);
}
