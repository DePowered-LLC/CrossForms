namespace CrossForms.Native.MacOS.Internals;


public class NsTextField: NsView, IObjClass<NsTextField> {
	public new static readonly ObjClass<NsTextField> Proto = ObjClass<NsTextField>.Get("NSTextField");
	
	private static readonly IntPtr LabelWithStringSel = ObjSelector.Get("labelWithString:");
	private static readonly IntPtr GetStringValueSel = ObjSelector.Get("stringValue");
	private static readonly IntPtr SetStringValueSel = ObjSelector.Get("setStringValue:");

	public static NsTextField CreateAuto (NsString text) {
		var inner = ObjC.SendMessage(Proto.inner, LabelWithStringSel, text.inner);
		return new NsTextField(inner) {
			TranslatesAutoresizingMaskIntoConstraints = false
		};
	}

	public new static NsTextField Borrow (IntPtr ptr) => new(ptr);
	protected NsTextField (IntPtr ptr): base(ptr) {}

	public string StringValue {
		get => NsString.Borrow(ObjC.SendMessage(inner, GetStringValueSel)).Value;
		set {
			var nsValue = NsString.CloneOwned(value);
			ObjC.SendMessage(inner, SetStringValueSel, nsValue.inner);
			nsValue.Release();
		}
	}
}
