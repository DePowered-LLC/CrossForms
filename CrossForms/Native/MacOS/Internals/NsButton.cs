namespace CrossForms.Native.MacOS.Internals;


public class NsButton: NsControl, IObjClass<NsButton> {
	public new static readonly ObjClass<NsButton> Proto = ObjClass<NsButton>.Get("NSButton");

	private static readonly IntPtr TitledButtonSel = ObjSelector.Get("buttonWithTitle:target:action:");
	private static readonly IntPtr SetTargetSel = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SetActionSel = ObjSelector.Get("setAction:");
	private static readonly IntPtr GetTitleSel = ObjSelector.Get("title");
	private static readonly IntPtr SetTitleSel = ObjSelector.Get("setTitle:");

	public static NsButton CreateAuto (NsString title, MethodRef handler) {
		var inner = ObjC.SendMessage(
			Proto.inner,
			TitledButtonSel,
			title.inner,
			handler.DelegatePtr,
			handler.MethodPtr
		);

		return new NsButton(inner) {
			TranslatesAutoresizingMaskIntoConstraints = false
		};
	}

	public new static NsButton Borrow (IntPtr ptr) => new(ptr);
	protected NsButton (IntPtr ptr): base(ptr) {}

	public string Title {
		get => NsString.Borrow(ObjC.SendMessage(inner, GetTitleSel)).Value;
		set {
			var nsTitle = NsString.CloneOwned(value);
			ObjC.SendMessage(inner, SetTitleSel, nsTitle.inner);
			nsTitle.Release();
		}
	}

	public void OnClick (Action handler) {
		PreRegisterEvent("click", handler, (dispatcher, selector) => {
			ObjC.SendMessage(inner, SetTargetSel, dispatcher.dispatcherInstance);
			ObjC.SendMessage(inner, SetActionSel, selector);
		});
	}
}
