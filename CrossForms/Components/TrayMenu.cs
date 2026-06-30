namespace CrossForms.Components;


public class TrayMenu: NativeTrayMenu {
	public TrayMenu (params NativeTrayMenuItem[] items) {
		Items = items;
	}
}
