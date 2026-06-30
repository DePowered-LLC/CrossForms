using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeCheckBox: ICheckBox, INativeAttachable, INativeFocusable {
	internal NsCheckBox? nsCheckBox;

	private bool _checked;
	private bool _enabled = true;
	private EventHandler<CheckEvent>? _onChange;

	public string Text { get; set; } = "";

	public bool Checked {
		get => nsCheckBox?.State ?? _checked;
		set {
			_checked = value;
			nsCheckBox?.State = value;
		}
	}

	public bool Enabled {
		get => nsCheckBox?.Enabled ?? _enabled;
		set {
			_enabled = value;
			nsCheckBox?.Enabled = value;
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

	public NsView? FocusView => nsCheckBox;

	public void AttachTo (NsWindow window) {
		var nsBoxText = NsString.CloneOwned(Text);
		var box = NsCheckBox.CreateAuto(nsBoxText, NsApplication.Current.AppDelegate.RefNoOp);
		nsBoxText.Release();
		nsCheckBox = box;
		
		box.State = _checked;
		if (!_enabled) {
			box.Enabled = false;
		}
		
		box.OnClick(() => {
			_checked = box.State;
			_onChange?.Invoke(this, new CheckEvent { Checked = _checked });
		});

		window.Append(box);
		box.ApplyConstraints(window, X, Y, Width, Height);
	}
}
