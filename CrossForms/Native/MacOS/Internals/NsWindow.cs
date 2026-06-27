using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;

public class NsWindow: NsEventDispatcher {
	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, CGRect frame, long styleMask, long backing, int defer);

	[Flags]
	public enum StyleMask: long {
		Borderless = 0L,
		Titled = 1L,
		Closable = 2L,
		Miniaturizable = 4L,
		Resizable = 8L
	}

	private static readonly ObjClass proto = ObjClass.Get("NSWindow");

	internal NsWindow () {}

	private static readonly IntPtr INIT_CONTENT_FRAME = ObjSelector.Get("initWithContentRect:styleMask:backing:defer:");
	private static readonly IntPtr MAKE_KEY_AND_ORDER_FRONT = ObjSelector.Get("makeKeyAndOrderFront:");
	public NsWindow (CGRect frame, StyleMask style, long backingStore, bool defer = false) {
		proto.Construct(this);
		SendMessage(inner, INIT_CONTENT_FRAME, frame, (long) style, backingStore, defer ? 1 : 0);
		ObjC.SendMessage(inner, MAKE_KEY_AND_ORDER_FRONT, IntPtr.Zero);
	}

	private static readonly IntPtr GET_TITLE = ObjSelector.Get("title");
	private static readonly IntPtr SET_TITLE = ObjSelector.Get("setTitle:");
	public string Title {
		get { return new NsString(ObjC.SendMessage(inner, GET_TITLE)).Value; }
		set { ObjC.SendMessage(inner, SET_TITLE, new NsString(value).inner); }
	}

	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, CGPoint position);

	[DllImport(ObjC.COCOA, EntryPoint = "objc_msgSend")]
	private static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, CGSize size);

	private static readonly IntPtr FRAME = ObjSelector.Get("frame");
	public CGRect Frame => ObjC.SendMessage<CGRect>(inner, FRAME);

	private static readonly IntPtr SET_FRAME_ORIGIN = ObjSelector.Get("setFrameOrigin:");
	public void SetFrameOrigin (double x, double y) {
		SendMessage(inner, SET_FRAME_ORIGIN, new CGPoint { x = x, y = y });
	}
	public void SetFrameOrigin (CGPoint point) {
		SendMessage(inner, SET_FRAME_ORIGIN, point);
	}

	private static readonly IntPtr SET_INITIAL_FIRST_RESPONDER = ObjSelector.Get("setInitialFirstResponder:");
	public void SetInitialFirstResponder (NsView view) {
		ObjC.SendMessage(inner, SET_INITIAL_FIRST_RESPONDER, view.inner);
	}

	public NsView ContentView => new NsView { inner = ObjC.SendMessage(inner, "contentView") };
	public void Append (NsControl child) {
		child.parent = this;
		ContentView.AddSubview(child);
		child.OnAttach();
	}

	class Del: NativeManaged<IntPtr> {
		public static ObjClass proto;
		static Del () {
			proto = NsObject.proto.NewSubClass("NSWindowDelegate");
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
