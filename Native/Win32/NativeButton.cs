using CrossForms.Native.Common;

namespace CrossForms.Native.Win32;
public class NativeButton: Control, IButton {
	public string text { get; set; }
	public int x { get; set; } = 0;
	public int y { get; set; } = 0;
	public ushort width { get; set; } = 120;
	public ushort height { get; set; } = 22;

	public EventHandler<ClickEvent> OnClick { get; set; } = (sender, e) => {};

	protected override ControlCreationOptions GetCreationOptions () {
		return new ControlCreationOptions {
			className = "Button",
			style = WindowStyle.TabStop | WindowStyle.Visible | WindowStyle.Child | (WindowStyle) (uint) ButtonStyle.Flat,
			label = text,
			width = width,
			height = height,
			x = x,
			y = y
		};
	}

	internal override IntPtr DispatchEvent (ushort command) {
		switch ((ButtonCommand) command) {
			case ButtonCommand.Clicked:
				OnClick(this, new ClickEvent { x = 0, y = 0 });
				return (IntPtr) 0;
		}

		return (IntPtr) (-1);
	}
}
