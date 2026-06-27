using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativePictureBox: IPictureBox {
	internal NsImageView? nsImageView;
	private string _imagePath = "";
	private byte[]? _imageData;

	public string ImagePath {
		get => _imagePath;
		set {
			_imagePath = value;
			_imageData = null;
			nsImageView?.ImagePath = value;
		}
	}

	public byte[]? ImageData {
		get => _imageData;
		set {
			_imageData = value;
			if (value != null) {
				_imagePath = "";
				nsImageView?.ImageData = value;
			}
		}
	}

	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 100;
	public ushort Height { get; set; } = 100;

	internal NsImageView CreateNsImageView () {
		var iv = new NsImageView();
		if (_imageData != null) {
			iv.ImageData = _imageData;
		} else if (_imagePath != "") {
			iv.ImagePath = _imagePath;
		}
		
		return iv;
	}
}
