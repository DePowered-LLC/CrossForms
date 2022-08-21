using CrossForms.Native.Common;
using static CrossForms.Native.MacOS.NSEventDispatcher;

namespace CrossForms.Native.MacOS;

public class AppDelegate: NativeManaged<IntPtr> {
	public delegate void _OnClickFn (IntPtr self, IntPtr sel, IntPtr sender);
	public delegate void OnClickFn (IntPtr self, ObjSelector sel, IntPtr sender);

	public static IntPtr noOpSel;
	public static readonly ObjClass proto = NSObject.proto.NewSubClass("AppDelegate", cls => {
		noOpSel = cls.AddMethod("noop", (DispatchEventFn) ((_, _, _) => {}), "v@:@");
	});

	public AppDelegate () {
		proto.Construct(this);
		ObjC.SendMessage(inner, "init");
	}
}
