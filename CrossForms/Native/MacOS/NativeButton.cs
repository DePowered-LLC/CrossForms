using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeButton: IButton {
	internal NativeButton? nextControl;

	internal NsButton? nsButton;

	private bool _enabled = true;
	private EventHandler<ClickEvent>? _onClick;

	public string Text { get; set; } = "";

	public bool Enabled {
		get => nsButton?.Enabled ?? _enabled;
		set {
			_enabled = value;
			if (nsButton != null) nsButton.Enabled = value;
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

	public void SetNextControl (IButton next) {
		nextControl = (NativeButton) next;
		if (nsButton != null && nextControl.nsButton != null)
			nsButton.SetNextKeyView(nextControl.nsButton);
	}

	internal NsButton CreateNsButton () {
		var btn = new NsButton(Text);
		if (!_enabled) btn.Enabled = false;
		btn.OnClick(() => {
			var clickPos = new ClickEvent();
			var ev = NsApplication.Current.CurrentEvent;
			if (ev != null) {
				var loc = ev.LocationInWindow;
				var frame = btn.Window?.ContentView.Frame;
				if (frame.HasValue) {
					clickPos.x = (int) loc.x;
					clickPos.y = (int) (frame.Value.size.height - loc.y);
				}
			}

			_onClick?.Invoke(this, clickPos);
		});
		
		return btn;
	}
}
