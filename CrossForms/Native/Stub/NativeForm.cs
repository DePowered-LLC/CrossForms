using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public class NativeForm: IForm {
	public string Id => throw new NotImplementedException();
	public string Title => throw new NotImplementedException();
	public ushort Width => throw new NotImplementedException();
	public ushort Height => throw new NotImplementedException();

	public void Show () {
		throw new NotImplementedException();
	}

	public void SetInitialControl (IButton button) {
		throw new NotImplementedException();
	}
}
