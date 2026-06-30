using CrossForms.Native.Common;

namespace CrossForms.Native.Win32;


public class NativeTrayMenuItem: ITrayMenuItem {
	public string Text { get; set; } = "";
	public bool Enabled { get; set; } = true;
	public Action? OnClick { get; set; }
}
