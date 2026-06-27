using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeCheckBox: ICheckBox {
	internal NsCheckBox? nsCheckBox;

	private bool _checked;
	private bool _enabled = true;
	private EventHandler<CheckEvent>? _onChange;

	public string Text { get; set; } = "";

	public bool Checked {
		get => nsCheckBox?.State ?? _checked;
		set {
			_checked = value;
			if (nsCheckBox != null) nsCheckBox.State = value;
		}
	}

	public bool Enabled {
		get => nsCheckBox?.Enabled ?? _enabled;
		set {
			_enabled = value;
			if (nsCheckBox != null) nsCheckBox.Enabled = value;
		}
	}

	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 120;
	public ushort Height { get; set; } = 22;

	public EventHandler<CheckEvent> OnChange {
		get => _onChange!;
		set => _onChange = value;
	}

	internal NsCheckBox CreateNsCheckBox () {
		var cb = new NsCheckBox(Text);
		cb.State = _checked;
		if (!_enabled) cb.Enabled = false;
		cb.OnClick(() => {
			_checked = cb.State;
			_onChange?.Invoke(this, new CheckEvent { Checked = _checked });
		});
		return cb;
	}
}
