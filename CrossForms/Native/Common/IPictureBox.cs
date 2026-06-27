namespace CrossForms.Native.Common;


public interface IPictureBox {
	string ImagePath { get; set; }
	byte[]? ImageData { get; set; }
	int X { get; set; }
	int Y { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
}
