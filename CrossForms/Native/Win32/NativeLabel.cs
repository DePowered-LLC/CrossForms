using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

namespace CrossForms.Native.Win32;


public class NativeLabel: Control, ILabel {
	public required string Text { get; set; }
	public int X { get; set; } = 0;
	public int Y { get; set; } = 0;
	public ushort Width { get; set; } = 120;
	public ushort Height { get; set; } = 17;

	protected override ControlCreationOptions GetCreationOptions () {
		return new ControlCreationOptions {
			className = "Static",
			style = WindowStyle.Visible | WindowStyle.Child,
			label = Text,
			width = Width,
			height = Height,
			x = X,
			y = Y
		};
	}
}
