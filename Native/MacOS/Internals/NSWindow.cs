using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

public class NSWindow: NSEventDispatcher {
	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, CGRect frame, long styleMask, long backing, int defer);

	public enum StyleMask: long {
		Borderless = 0L,
		Titled = 1L,
		Closable = 2L,
		Miniaturizable = 4L,
		Resizable = 8L
	}

	private static readonly ObjClass proto = ObjClass.Get("NSWindow");

	private static readonly IntPtr INIT_CONTENT_FRAME = ObjSelector.Get("initWithContentRect:styleMask:backing:defer:");
	private static readonly IntPtr MAKE_KEY_AND_ORDER_FRONT = ObjSelector.Get("makeKeyAndOrderFront:");
	public NSWindow (CGRect frame, StyleMask style, long backingStore, bool defer = false) {
		proto.Construct(this);
		SendMessage(inner, INIT_CONTENT_FRAME, frame, (long) style, backingStore, defer ? 1 : 0);
		ObjC.SendMessage(inner, MAKE_KEY_AND_ORDER_FRONT, IntPtr.Zero);
	}

	private static readonly IntPtr SET_TITLE = ObjSelector.Get("setTitle:");
	public string title {
		get { return null; }
		set { ObjC.SendMessage(inner, SET_TITLE, new NSString(value).inner); }
	}

	public NSView contentView => new NSView { inner = ObjC.SendMessage(inner, "contentView") };
	public void Append (NSControl child) {
		child.parent = this;
		contentView.AddSubview(child);
		child.OnAttach();
	}

	class Del: NativeManaged<IntPtr> {
		public static ObjClass proto;
		static Del () {
			proto = NSObject.proto.NewSubClass("NSWindowDelegate");
			// class_addProtocol(myWindowDelegateClass, @protocol(NSWindowDelegate));
			proto.AddMethod("windowWillClose:", (WindowWillCloseFn) ((_, _, _) => Console.WriteLine("Smth!")), "v@:@");
		}

		public Del () {
			proto.Construct(this);
		}
	}

	public delegate void WindowWillCloseFn (IntPtr self, IntPtr sel, IntPtr notification);
	public void OnClose (Action handle) {
		// var cls = NSObject.proto.NewSubClass("NSWindowDelegate", cls => {
		// 	cls.AddMethod("windowWillClose:", (WindowWillCloseFn) ((_, _, _) => Console.WriteLine("Smth!")), "v@:@");
		// });

		// var del = new Del();

		// https://github.com/ritalin/osx_app_in_plain_ziglang/blob/dc03a6884b193828a0545b6c4c502e49ddcd313f/examples/hand_made_binding/src/appKit/widget/NSWindow.zig#L150
		// https://github.com/ritalin/osx_app_in_plain_ziglang/blob/dc03a6884b193828a0545b6c4c502e49ddcd313f/examples/hand_made_binding/src/foundation/runtime/backend_helpers.c#L36
		// ObjClass.Get("NSWindowDelegate").ReplaceMethod("windowWillClose:", (WindowWillCloseFn) ((_, _, _) => Console.WriteLine("Smth!")), "v@:@");
		AttachEvent(this, "windowWillClose:", handle);
		// dispatcherClass.AddMethod("windowWillClose:", handle, "v@:@");
		ObjC.SendMessage(inner, ObjSelector.Get("setDelegate:"), dispatcherInstance);
		// ObjC.SendMessage(inner, ObjSelector.Get("setDelegate:"), del.inner);
	}
}
