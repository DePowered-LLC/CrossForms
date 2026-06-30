using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NsNested: NsObject {
	public NsNested? parent;
	
	protected NsNested (IntPtr ptr): base(ptr) {}
}
