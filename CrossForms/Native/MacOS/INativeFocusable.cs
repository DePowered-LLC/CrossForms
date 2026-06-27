using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


internal interface INativeFocusable {
	NsView? FocusView { get; }
}
