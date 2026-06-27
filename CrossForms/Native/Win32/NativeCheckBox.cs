using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeCheckBox: Control, ICheckBox {
	private bool _checked;

	public required string Text { get; set; }

	public bool Checked {
		get => IsLoaded
			? SendMessage(handle, (uint) ButtonMessage.GetCheck, IntPtr.Zero, IntPtr.Zero) != IntPtr.Zero
			: _checked;
		set {
			_checked = value;
			if (IsLoaded) SendMessage(handle, (uint) ButtonMessage.SetCheck, value ? 1 : 0, 0);
		}
	}

	public int X { get; set; } = 0;
	public int Y { get; set; } = 0;
	public ushort Width { get; set; } = 120;
	public ushort Height { get; set; } = 22;
	public EventHandler<CheckEvent> OnChange { get; set; } = (_, _) => {};

	protected override ControlCreationOptions GetCreationOptions () {
		return new ControlCreationOptions {
			className = "Button",
			style = WindowStyle.Visible | WindowStyle.Child |
			        (WindowStyle) (uint) ButtonStyle.AutoCheckBox,
			label = Text,
			width = Width,
			height = Height,
			x = X,
			y = Y
		};
	}

	internal override IntPtr DispatchEvent (ushort command) {
		if ((ButtonCommand) command == ButtonCommand.Clicked) {
			OnChange(this, new CheckEvent { Checked = Checked });
			return 0;
		}

		return -1;
	}
}
