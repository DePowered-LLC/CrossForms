using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeButton: IButton, INativeAttachable, INativeFocusable {
	internal NsButton? nsButton;

	private bool _enabled = true;
	private EventHandler<ClickEvent>? _onClick;

	public string Text { get; set; } = "";

	public bool Enabled {
		get => nsButton?.Enabled ?? _enabled;
		set {
			_enabled = value;
			nsButton?.Enabled = value;
		}
	}

	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 120;
	public ushort Height { get; set; } = 22;

	public EventHandler<ClickEvent> OnClick {
		get => _onClick!;
		set => _onClick = value;
	}

	public NsView? FocusView => nsButton;

	public void SetNextControl (IButton next) {
		var nextBtn = (NativeButton) next;
		if (nsButton != null && nextBtn.nsButton != null) {
			nsButton.SetNextKeyView(nextBtn.nsButton);
		}
	}

	public void AttachTo (NsWindow window) {
		var nsBtnText = NsString.CloneOwned(Text);
		var btn = NsButton.CreateAuto(nsBtnText, NsApplication.Current.AppDelegate.RefNoOp);
		nsBtnText.Release();
		nsButton = btn;
		
		if (!_enabled) {
			btn.Enabled = false;
		}
		
		btn.OnClick(() => {
			var clickPos = new ClickEvent();
			var ev = NsApplication.Current.BorrowCurrentEvent();
			if (ev != null) {
				var loc = ev.LocationInWindow;
				var frame = btn.BorrowWindow()?.BorrowContentView().Frame;
				if (frame.HasValue) {
					clickPos.x = (int) loc.x;
					clickPos.y = (int) (frame.Value.size.height - loc.y);
				}
			}

			_onClick?.Invoke(this, clickPos);
		});

		window.Append(btn);
		btn.ApplyConstraints(window, X, Y, Width, Height);
	}
}
