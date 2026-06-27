using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeRadioButton: IRadioButton {
	internal NsRadioButton? nsRadioButton;
	internal NativeRadioGroup? group;

	private bool _checked;
	private bool _enabled = true;

	public string Text { get; set; } = "";

	public bool Checked {
		get => nsRadioButton?.State ?? _checked;
		set {
			_checked = value;
			nsRadioButton?.State = value;
		}
	}

	public bool Enabled {
		get => nsRadioButton?.Enabled ?? _enabled;
		set {
			_enabled = value;
			nsRadioButton?.Enabled = value;
		}
	}

	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 120;
	public ushort Height { get; set; } = 22;

	internal NsRadioButton CreateNsRadioButton () {
		var rb = new NsRadioButton(Text) {
			State = _checked
		};
		
		if (!_enabled) rb.Enabled = false;
		return rb;
	}
}
