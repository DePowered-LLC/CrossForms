using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public interface INativeAttachable {
	void AttachTo (NsWindow window);
}
