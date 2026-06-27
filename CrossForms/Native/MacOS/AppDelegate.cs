using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


internal class AppDelegate: NativeManaged<IntPtr> {
	public delegate void _OnClickFn (IntPtr self, IntPtr sel, IntPtr sender);

	public delegate void OnClickFn (IntPtr self, ObjSelector sel, IntPtr sender);

	public static readonly ObjClass proto = NsObject.Proto.NewSubClass("AppDelegate",
		cls => { NO_OP = cls.AddMethod("noop", (NsEventDispatcher.DispatchEventFn) ((_, _, _) => {}), "v@:@"); });

	public AppDelegate () {
		proto.Construct(this);
		ObjC.SendMessage(inner, "init");
	}

	public static IntPtr NO_OP { get; private set; }
}
