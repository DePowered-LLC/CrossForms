namespace CrossForms.Native.Common;


public interface IPictureBox {
	void LoadImage (string path);
	void LoadImage (byte[] data);
	
	int X { get; set; }
	int Y { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
}
