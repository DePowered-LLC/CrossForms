namespace CrossForms.Native.MacOS.Internals;


public class NsPopUpButton: NsControl, IObjClass<NsPopUpButton> {
	public new static readonly ObjClass<NsPopUpButton> Proto = ObjClass<NsPopUpButton>.Get("NSPopUpButton");
	
	private static readonly IntPtr InitSel = ObjSelector.Get("init");
	private static readonly IntPtr AddItemSel = ObjSelector.Get("addItemWithTitle:");
	private static readonly IntPtr RemoveAllItemsSel = ObjSelector.Get("removeAllItems");
	private static readonly IntPtr SelectAtIndexSel = ObjSelector.Get("selectItemAtIndex:");
	private static readonly IntPtr IndexOfSelectedSel = ObjSelector.Get("indexOfSelectedItem");
	private static readonly IntPtr SetTargetSel = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SetActionSel = ObjSelector.Get("setAction:");

	public static NsPopUpButton CreateOwned () {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, InitSel);
		
		result.TranslatesAutoresizingMaskIntoConstraints = false;
		return result;
	}

	public new static NsPopUpButton Borrow (IntPtr ptr) => new(ptr);
	protected NsPopUpButton (IntPtr ptr): base(ptr) {}

	public void AddItem (NsString title) {
		ObjC.SendMessage(inner, AddItemSel, title.inner);
	}

	public void RemoveAllItems () {
		ObjC.SendMessage(inner, RemoveAllItemsSel);
	}

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
