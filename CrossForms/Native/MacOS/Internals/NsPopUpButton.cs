namespace CrossForms.Native.MacOS.Internals;


public class NsPopUpButton: NsControl {
	private static readonly ObjClass Proto = ObjClass.Get("NSPopUpButton");
	private static readonly IntPtr AllocSel = ObjSelector.Get("alloc");
	private static readonly IntPtr InitSel = ObjSelector.Get("init");
	private static readonly IntPtr AddItemSel = ObjSelector.Get("addItemWithTitle:");
	private static readonly IntPtr RemoveAllItemsSel = ObjSelector.Get("removeAllItems");
	private static readonly IntPtr SelectAtIndexSel = ObjSelector.Get("selectItemAtIndex:");
	private static readonly IntPtr IndexOfSelectedSel = ObjSelector.Get("indexOfSelectedItem");
	private static readonly IntPtr SetTargetSel = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SetActionSel = ObjSelector.Get("setAction:");

	public NsPopUpButton () {
		var alloc = ObjC.SendMessage(Proto.inner, AllocSel);
		inner = ObjC.SendMessage(alloc, InitSel);
		TranslatesAutoresizingMaskIntoConstraints = false;
	}

	public void AddItem (string title) =>
		ObjC.SendMessage(inner, AddItemSel, new NsString(title).inner);

	public void RemoveAllItems () =>
		ObjC.SendMessage(inner, RemoveAllItemsSel);

	public int SelectedIndex {
		get => (int) ObjC.SendMessage(inner, IndexOfSelectedSel);
		set => ObjC.SendMessage(inner, SelectAtIndexSel, value);
	}

	public void OnChange (Action handler) {
		PreRegisterEvent("click", handler, (dispatcher, selector) => {
			ObjC.SendMessage(inner, SetTargetSel, dispatcher.dispatcherInstance);
			ObjC.SendMessage(inner, SetActionSel, selector);
		});
	}
}
