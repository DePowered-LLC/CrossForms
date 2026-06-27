using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeLabel: ILabel {
	internal NsTextField? nsTextField;

	public string Text { get; set; } = "";
	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 120;
	public ushort Height { get; set; } = 17;

	internal NsTextField CreateNsTextField () {
		return new NsTextField(Text);
	}
}
