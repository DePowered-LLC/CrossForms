using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

namespace CrossForms.Native.Win32;


public class NativeButton: Control, IButton {
	internal NativeButton? nextControl;
	public required string Text { get; set; }
	public int X { get; set; } = 0;
	public int Y { get; set; } = 0;
	public ushort Width { get; set; } = 120;
	public ushort Height { get; set; } = 22;

	public EventHandler<ClickEvent> OnClick { get; set; } = (sender, e) => {};

	public void SetNextControl (IButton next) {
		nextControl = (NativeButton) next;
	}

	protected override ControlCreationOptions GetCreationOptions () {
		return new ControlCreationOptions {
			className = "Button",
			style = WindowStyle.TabStop | WindowStyle.Visible | WindowStyle.Child |
			        (WindowStyle) (uint) ButtonStyle.Flat,
			label = Text,
			width = Width,
			height = Height,
			x = X,
			y = Y
		};
	}

	internal override IntPtr DispatchEvent (ushort command) {
		switch ((ButtonCommand) command) {
			case ButtonCommand.Clicked:
				OnClick(this, new ClickEvent { x = 0, y = 0 });
				return 0;
		}

		return -1;
	}
}
