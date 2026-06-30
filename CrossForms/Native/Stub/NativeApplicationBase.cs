using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public class NativeApplicationBase: ApplicationBase {
	public new static void Quit () => throw new NotImplementedException();
}
