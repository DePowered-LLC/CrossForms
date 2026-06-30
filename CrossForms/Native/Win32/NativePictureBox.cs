using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativePictureBox: Control, IPictureBox {
	private string _imagePath = "";
	private byte[]? _imageData;
	private IntPtr _hBitmap = IntPtr.Zero;
	
	public void LoadImage (string path) {
		_imagePath = path;
		_imageData = null;
		if (IsLoaded) ApplyImage();
	}
	
	public void LoadImage (byte[]? data) {
		_imageData = data;
		if (data != null) {
			_imagePath = "";
			if (IsLoaded) ApplyImageData(data);
		}
	}

	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 100;
	public ushort Height { get; set; } = 100;

	protected override void Load () {
		base.Load();
		if (_imageData != null) ApplyImageData(_imageData);
		else if (_imagePath != "") ApplyImage();
	}

	private void ApplyImage () {
		var hNew = Internals.Internals.LoadImage(
			IntPtr.Zero,
			_imagePath,
			(uint) ImageType.Bitmap,
			Width,
			Height,
			0x0010 /* LR_LOADFROMFILE */
		);
		SetBitmap(hNew);
	}

	private void ApplyImageData (byte[] data) {
		var physW = (int) (Width * NativeApplicationBase.DpiScale);
		var physH = (int) (Height * NativeApplicationBase.DpiScale);
		var hNew = HBitmapFromImageData(data, physW, physH);
		SetBitmap(hNew);
	}

	private void SetBitmap (IntPtr hNew) {
		SendMessage(handle, (uint) StaticMessage.SetImage, (int) ImageType.Bitmap, hNew);
		if (_hBitmap != IntPtr.Zero) DeleteObject(_hBitmap);
		_hBitmap = hNew;
	}

	protected override ControlCreationOptions GetCreationOptions () => new() {
		className = "Static",
		style = WindowStyle.Visible | WindowStyle.Child | (WindowStyle) (uint) StaticStyle.Bitmap,
		label = "",
		width = Width,
		height = Height,
		x = X,
		y = Y
	};
}
