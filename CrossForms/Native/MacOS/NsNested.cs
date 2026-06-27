using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;


public class NsNested: NativeManaged<IntPtr> {
	public NsNested? parent;
}
