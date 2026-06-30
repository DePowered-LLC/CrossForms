namespace CrossForms.Native.MacOS.Internals;


public class NsTextInput: NsControl, IObjClass<NsTextInput> {
	public new static readonly ObjClass<NsTextInput> Proto = ObjClass<NsTextInput>.Get("NSTextField");
	
	private static readonly IntPtr TextFieldWithStringSel = ObjSelector.Get("textFieldWithString:");
	private static readonly IntPtr GetStringValueSel = ObjSelector.Get("stringValue");
	private static readonly IntPtr SetStringValueSel = ObjSelector.Get("setStringValue:");
	private static readonly IntPtr SetDelegateSel = ObjSelector.Get("setDelegate:");

	public static NsTextInput CreateAuto (NsString text) {
		var inner = ObjC.SendMessage(Proto.inner, TextFieldWithStringSel, text.inner);
		return new NsTextInput(inner) {
			TranslatesAutoresizingMaskIntoConstraints = false
		};
	}

	public new static NsTextInput Borrow (IntPtr ptr) => new(ptr);
	protected NsTextInput (IntPtr ptr): base(ptr) {}

	public string StringValue {
		get => NsString.Borrow(ObjC.SendMessage(inner, GetStringValueSel)).Value;
		set {
			var nsValue = NsString.CloneOwned(value);
			ObjC.SendMessage(inner, SetStringValueSel, nsValue.inner);
			nsValue.Release();
		}
	}

	public void OnChange (Action handler) {
		PreRegisterEvent("controlTextDidChange:", handler, (dispatcher, _) => {
			ObjC.SendMessage(inner, SetDelegateSel, dispatcher.dispatcherInstance);
		});
	}
}
