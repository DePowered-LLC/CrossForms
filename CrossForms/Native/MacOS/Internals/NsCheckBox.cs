namespace CrossForms.Native.MacOS.Internals;


public class NsCheckBox: NsControl, IObjClass<NsCheckBox> {
	private new static readonly ObjClass<NsCheckBox> Proto = ObjClass<NsCheckBox>.Get("NSButton");
	
	private static readonly IntPtr CheckboxWithTitleSel = ObjSelector.Get("checkboxWithTitle:target:action:");
	private static readonly IntPtr SetTargetSel = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SetActionSel = ObjSelector.Get("setAction:");
	private static readonly IntPtr GetStateSel = ObjSelector.Get("state");
	private static readonly IntPtr SetStateSel = ObjSelector.Get("setState:");

	public static NsCheckBox CreateAuto (NsString title, MethodRef handler) {
		var inner = ObjC.SendMessage(
			Proto.inner,
			CheckboxWithTitleSel,
			title.inner,
			handler.DelegatePtr,
			handler.MethodPtr
		);
		
		return new NsCheckBox(inner) {
			TranslatesAutoresizingMaskIntoConstraints = false
		};
	}
	
	public new static NsCheckBox Borrow (IntPtr ptr) => new(ptr);
	protected NsCheckBox (IntPtr ptr): base(ptr) {}

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
}
