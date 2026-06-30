using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public class NativePictureBox: IPictureBox {
	public void LoadImage (string path) {
		throw new NotImplementedException();
	}
	public void LoadImage (byte[] data) {
		throw new NotImplementedException();
	}

	public int X {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public int Y {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public ushort Width {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public ushort Height {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}
}
