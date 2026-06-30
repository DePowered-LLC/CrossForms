using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeTrayMenu: ITrayMenu {
	public ITrayMenuItem[] Items { get; init; } = [];

	internal NsMenu? BuiltMenu { get; private set; }

	internal NsMenu Build (NsEventDispatcher dispatcher) {
		if (BuiltMenu != null) return BuiltMenu;

		var menu = NsMenu.CreateOwned();
		BuiltMenu = menu;

		for (var i = 0; i < Items.Length; i++) {
			var item = (NativeTrayMenuItem) Items[i];
			
			var nsItemStr = NsString.CloneOwned(item.Text);
			var nsItem = NsMenuItem.CreateOwned(nsItemStr, IntPtr.Zero, NsString.Empty);
			nsItemStr.Release();
			
			nsItem.Enabled = item.Enabled;
			item.nsItem = nsItem;

			if (item.OnClick != null) {
				var handler = item.OnClick;
				var selectorName = $"trayMenuAction_{i}:";
				var sel = dispatcher.AttachEvent(nsItem, selectorName, handler);
				nsItem.SetTarget(dispatcher.dispatcherInstance);
				nsItem.SetAction(sel);
			}

			menu.AddItem(nsItem);
		}

		return menu;
	}
}
