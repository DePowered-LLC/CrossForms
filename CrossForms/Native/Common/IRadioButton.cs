namespace CrossForms.Native.Common;


public interface IRadioButton: IEnabled {
	string Text { get; set; }
	bool Checked { get; set; }
	int X { get; set; }
	int Y { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
}
