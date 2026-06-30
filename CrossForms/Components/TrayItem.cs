using CrossForms.Platform;

namespace CrossForms.Components;


public class TrayItem: NativeTrayItem {
	public TrayItem (string iconPath = "") {
		IconPath = iconPath;
	}
	
	public TrayItem (Resource? icon) {
		IconData = icon?.ToBuffer();
	}
}
