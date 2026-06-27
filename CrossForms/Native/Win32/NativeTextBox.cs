using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeTextBox: Control, ITextBox {
	private string _text = "";

	public required string Text {
		get => IsLoaded ? GetWindowText(handle) : _text;
		set {
			_text = value;
			if (IsLoaded) SetWindowText(handle, value);
		}
	}

	public int X { get; set; } = 0;
	public int Y { get; set; } = 0;
	public ushort Width { get; set; } = 200;
	public ushort Height { get; set; } = 22;
	public EventHandler<ChangeEvent> OnChange { get; set; } = (_, _) => {};

	protected override ControlCreationOptions GetCreationOptions () {
		return new ControlCreationOptions {
			className = "Edit",
			style = WindowStyle.Visible | WindowStyle.Child | WindowStyle.Border,
			label = _text,
			width = Width,
			height = Height,
			x = X,
			y = Y
		};
	}

	internal override IntPtr DispatchEvent (ushort command) {
		if ((EditCommand) command == EditCommand.Changed) {
			OnChange(this, new ChangeEvent { Text = Text });
			return 0;
		}

		return -1;
	}
}
