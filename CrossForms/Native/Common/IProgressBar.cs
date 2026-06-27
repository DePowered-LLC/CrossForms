namespace CrossForms.Native.Common;


public interface IProgressBar {
	double Min { get; set; }
	double Max { get; set; }
	double Value { get; set; }
	bool Indeterminate { get; set; }
	int X { get; set; }
	int Y { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
}
