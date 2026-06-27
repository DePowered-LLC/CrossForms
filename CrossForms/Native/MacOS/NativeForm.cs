using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeForm: IForm {
	private readonly List<NativeButton> _children = [];
	private NativeButton? _initialControl;

	private NsWindow? _window;
	public string Id { get; set; } = "";
	public string Title { get; set; } = "";
	public ushort Width { get; set; }
	public ushort Height { get; set; }

	public void SetInitialControl (IButton button) {
		_initialControl = (NativeButton) button;
	}

	public void Show () {
		var w = Width > 0 ? (double) Width : 800;
		var h = Height > 0 ? (double) Height : 600;

		_window = new NsWindow(
			new CgRect(0, 0, w, h),
			NsWindow.StyleMask.Titled | NsWindow.StyleMask.Closable | NsWindow.StyleMask.Resizable,
			2
		);
		_window.Title = Title;

		_window.OnClose(() => {
			if (!NsApplication.Current.IsRunning) return;
			NsApplication.Current.Terminate();
		});

		foreach (var child in _children) {
			AttachButton(child);
		}

		foreach (var btn in _children) {
			if (btn.nextControl?.nsButton != null) {
				btn.nsButton!.SetNextKeyView(btn.nextControl.nsButton);
			}
		}

		var first = _initialControl ?? (_children.Count > 0 ? _children[0] : null);
		if (first?.nsButton != null) {
			_window.SetInitialFirstResponder(first.nsButton);
		}
	}

	public void Append (NativeButton button) {
		if (_children.Count > 0 && _children[^1].nextControl == null) {
			_children[^1].nextControl = button;
		}

		_children.Add(button);
		if (_window != null) {
			AttachButton(button);
		}
	}

	private void AttachButton (NativeButton button) {
		var nsBtn = button.CreateNsButton();
		button.nsButton = nsBtn;
		_window!.Append(nsBtn);
		if (button.nextControl?.nsButton != null) {
			nsBtn.SetNextKeyView(button.nextControl.nsButton);
		}

		var contentView = _window.ContentView;
		var bw = button.Width > 0 ? (double) button.Width : 120;
		var bh = button.Height > 0 ? (double) button.Height : 22;

		nsBtn.LeadingAnchor.ConstraintToAnchor(contentView.LeadingAnchor, button.X).Active = true;
		nsBtn.TopAnchor.ConstraintToAnchor(contentView.TopAnchor, button.Y).Active = true;
		nsBtn.WidthAnchor.ConstraintToConstant(bw).Active = true;
		nsBtn.HeightAnchor.ConstraintToConstant(bh).Active = true;
	}
}
