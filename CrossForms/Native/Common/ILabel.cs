namespace CrossForms.Native.Common;


public interface ILabel {
	string Text { get; }
	int X { get; }
	int Y { get; }
	ushort Width { get; }
	ushort Height { get; }
}
