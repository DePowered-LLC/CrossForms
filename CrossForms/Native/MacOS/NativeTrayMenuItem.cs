using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeTrayMenuItem: ITrayMenuItem {
	internal NsMenuItem? nsItem;

	public string Text { get; set; } = "";

	private bool _enabled = true;

	public bool Enabled {
		get => _enabled;
		set {
			_enabled = value;
			nsItem?.Enabled = value;
		}
	}

	public Action? OnClick { get; set; }
}
