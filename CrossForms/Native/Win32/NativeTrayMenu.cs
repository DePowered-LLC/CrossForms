using CrossForms.Native.Common;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeTrayMenu: ITrayMenu {
	public ITrayMenuItem[] Items { get; init; } = [];

	internal IntPtr BuildHMenu () {
		var hMenu = CreatePopupMenu();
		for (var i = 0; i < Items.Length; i++) {
			var item = Items[i];
			
			var flags = MfString;
			if (!item.Enabled) flags |= MfGrayed;
			
			AppendMenu(hMenu, flags, (uint) (i + 1), item.Text);
		}

		return hMenu;
	}

	internal void InvokeItem (int index) {
		if (index >= 0 && index < Items.Length) {
			Items[index].OnClick?.Invoke();
		}
	}
}
