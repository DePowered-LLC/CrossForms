using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativePictureBox: IPictureBox, INativeAttachable {
	private readonly NsImageView _nsImageView = NsImageView.CreateOwned();

	public void LoadImage (string path) {
		var nsPath = NsString.CloneOwned(path);
		var nsImg = NsImage.CreateOwned(nsPath);
		nsPath.Release();
		EmplaceImage(nsImg);
		nsImg.Release();
	}

	public void LoadImage (byte[] data) {
		var nsImg = NsImage.CreateOwned(NsData.CreateAuto(data));
		EmplaceImage(nsImg);
		nsImg.Release();
	}

	private void EmplaceImage (NsImage image) {
		_nsImageView.Image = image;
	}

	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 100;
	public ushort Height { get; set; } = 100;

	public void AttachTo (NsWindow window) {
		window.BorrowContentView().AddSubview(_nsImageView);
		_nsImageView.ApplyConstraints(window, X, Y, Width, Height);
	}
}
