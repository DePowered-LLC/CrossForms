using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeRadioButton: IRadioButton, INativeAttachable, INativeFocusable {
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

	public NsView? FocusView => nsRadioButton;

	public void AttachTo (NsWindow window) {
		var nsBtnText = NsString.CloneOwned(Text);
		var btn = NsRadioButton.CreateAuto(nsBtnText, NsApplication.Current.AppDelegate.RefNoOp);
		btn.State = _checked;
		nsBtnText.Release();
		nsRadioButton = btn;
		
		if (!_enabled) {
			btn.Enabled = false;
		}

		if (group != null) {
			var index = Array.IndexOf(group.Items, this);
			btn.OnClick(() => {
				foreach (var item in group.Items) {
					((NativeRadioButton) item).nsRadioButton!.State = false;
				}

				btn.State = true;
				group.NotifyChange(index);
			});
		}

		window.Append(btn);
		btn.ApplyConstraints(window, X, Y, Width, Height);
	}
}
