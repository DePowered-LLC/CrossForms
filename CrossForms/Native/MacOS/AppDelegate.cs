using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;

internal class AppDelegate: NativeManaged<IntPtr> {
	public delegate void _OnClickFn (IntPtr self, IntPtr sel, IntPtr sender);
	public delegate void OnClickFn (IntPtr self, ObjSelector sel, IntPtr sender);

	public static IntPtr NO_OP { get; private set; }
	public static readonly ObjClass proto = NsObject.proto.NewSubClass("AppDelegate", cls => {
		NO_OP = cls.AddMethod("noop", (NsEventDispatcher.DispatchEventFn) ((_, _, _) => {}), "v@:@");
	});

	public AppDelegate () {
		proto.Construct(this);
		ObjC.SendMessage(inner, "init");
	}
}
