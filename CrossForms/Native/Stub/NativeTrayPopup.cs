using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public class NativeTrayPopup: ITrayPopup {
	public ushort Width {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public ushort Height {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public void ShowNear (double x, double y) => throw new NotImplementedException();
	public void Hide () => throw new NotImplementedException();
}
