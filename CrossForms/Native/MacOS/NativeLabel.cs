using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeLabel: ILabel, INativeAttachable {
	internal NsTextField? nsTextField;

	public string Text { get; set; } = "";
	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 120;
	public ushort Height { get; set; } = 17;

	public void AttachTo (NsWindow window) {
		var nsLabelText = NsString.CloneOwned(Text);
		var tf = NsTextField.CreateAuto(nsLabelText);
		nsLabelText.Release();
		
		nsTextField = tf;
		window.BorrowContentView().AddSubview(tf);
		tf.ApplyConstraints(window, X, Y, Width, Height);
	}
}
