using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public class NsMenuItem: NsObject, IObjClass<NsMenuItem> {
	private new static readonly ObjClass<NsMenuItem> Proto = ObjClass<NsMenuItem>.Get("NSMenuItem");
	
	private static readonly IntPtr InitSel = ObjSelector.Get("initWithTitle:action:keyEquivalent:");
	private static readonly IntPtr SetTargetSel = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SetActionSel = ObjSelector.Get("setAction:");
	private static readonly IntPtr SetEnabledSel = ObjSelector.Get("setEnabled:");
	private static readonly IntPtr SetSubmenuSel = ObjSelector.Get("setSubmenu:");
	
	public static NsMenuItem CreateOwned (NsString title, IntPtr action, NsString keyEquivalent) {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, InitSel, title.inner, action, keyEquivalent.inner);
		return result;
	}

	public new static NsMenuItem Borrow (IntPtr ptr) => new(ptr);
	protected NsMenuItem (IntPtr ptr): base(ptr) {}

	public bool Enabled {
		set => ObjC.SendMessage(inner, SetEnabledSel, value ? 1 : 0);
	}

	public void SetTarget (IntPtr target) => ObjC.SendMessage(inner, SetTargetSel, target);
	public void SetAction (IntPtr action) => ObjC.SendMessage(inner, SetActionSel, action);
	public void SetSubmenu (NsMenu menu) => ObjC.SendMessage(inner, SetSubmenuSel, menu.inner);
}
