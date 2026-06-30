namespace CrossForms.Native.MacOS.Internals;


public class NsMenu: NsObject, IObjClass<NsMenu> {
	public new static readonly ObjClass<NsMenu> Proto = ObjClass<NsMenu>.Get("NSMenu");
	
	private static readonly IntPtr InitSel = ObjSelector.Get("init");
	private static readonly IntPtr AddItemSel = ObjSelector.Get("addItem:");
	private static readonly IntPtr PopUpContextMenuSel = ObjSelector.Get("popUpContextMenu:withEvent:forView:");
	
	public static NsMenu CreateOwned () {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, InitSel);
		return result;
	}

	public new static NsMenu Borrow (IntPtr ptr) => new(ptr);
	protected NsMenu (IntPtr ptr): base(ptr) {}
	
	public void AddItem (NsMenuItem item) {
		ObjC.SendMessage(inner, AddItemSel, item.inner);
	}

	public static void Open (NsMenu menu, NsEvent? ev, IntPtr view) {
		if (ev == null) return;
		ObjC.SendMessage(Proto.inner, PopUpContextMenuSel, menu.inner, ev.inner, view);
	}
}