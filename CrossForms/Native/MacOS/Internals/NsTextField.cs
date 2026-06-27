using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


public partial class NsTextField: NsView {
	private static readonly ObjClass Proto = ObjClass.Get("NSTextField");
	private static readonly IntPtr LabelWithStringSel = ObjSelector.Get("labelWithString:");
	private static readonly IntPtr GetStringValueSel = ObjSelector.Get("stringValue");
	private static readonly IntPtr SetStringValueSel = ObjSelector.Get("setStringValue:");

	public NsTextField (string text) {
		inner = SendMessage(Proto.inner, LabelWithStringSel, new NsString(text).inner);
		TranslatesAutoresizingMaskIntoConstraints = false;
	}

	public string StringValue {
		get => new NsString(ObjC.SendMessage(inner, GetStringValueSel)).Value;
		set => ObjC.SendMessage(inner, SetStringValueSel, new NsString(value).inner);
	}

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr str);
}
