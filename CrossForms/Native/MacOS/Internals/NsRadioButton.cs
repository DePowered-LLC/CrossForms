using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


public class NsRadioButton: NsControl, IObjClass<NsRadioButton> {
	public new static readonly ObjClass<NsRadioButton> Proto = ObjClass<NsRadioButton>.Get("NSButton");
	
	private static readonly IntPtr RadioWithTitleSel = ObjSelector.Get("radioButtonWithTitle:target:action:");
	private static readonly IntPtr SetTargetSel = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SetActionSel = ObjSelector.Get("setAction:");
	private static readonly IntPtr GetStateSel = ObjSelector.Get("state");
	private static readonly IntPtr SetStateSel = ObjSelector.Get("setState:");
	
	public static NsRadioButton CreateAuto (NsString title, MethodRef handler) {
		var inner = ObjC.SendMessage(
			Proto.inner,
			RadioWithTitleSel,
			title.inner,
			handler.DelegatePtr,
			handler.MethodPtr
		);
		
		return new NsRadioButton(inner) {
			TranslatesAutoresizingMaskIntoConstraints = false
		};
	}

	public new static NsRadioButton Borrow (IntPtr ptr) => new(ptr);
	protected NsRadioButton (IntPtr ptr): base(ptr) {}

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
