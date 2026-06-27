using System.Runtime.InteropServices;

using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public partial class NsWindow: NsEventDispatcher {
	public delegate void WindowWillCloseFn (IntPtr self, IntPtr sel, IntPtr notification);

	[Flags]
	public enum StyleMask: long {
		Borderless = 0L,
		Titled = 1L,
		Closable = 2L,
		Miniaturizable = 4L,
		Resizable = 8L
	}

	private static readonly ObjClass Proto = ObjClass.Get("NSWindow");

	private static readonly IntPtr InitContentFrameSel = ObjSelector.Get("initWithContentRect:styleMask:backing:defer:");
	private static readonly IntPtr MakeKeyAndOrderFrontSel = ObjSelector.Get("makeKeyAndOrderFront:");

	private static readonly IntPtr GetTitleSel = ObjSelector.Get("title");
	private static readonly IntPtr SetTitleSel = ObjSelector.Get("setTitle:");

	private static readonly IntPtr FrameSel = ObjSelector.Get("frame");

	private static readonly IntPtr SetFrameOriginSel = ObjSelector.Get("setFrameOrigin:");

	private static readonly IntPtr SetInitialFirstResponderSel = ObjSelector.Get("setInitialFirstResponder:");

	internal NsWindow () {}

	public NsWindow (CgRect frame, StyleMask style, long backingStore, bool defer = false) {
		Proto.Construct(this);
		SendMessage(inner, InitContentFrameSel, frame, (long) style, backingStore, defer ? 1 : 0);
		ObjC.SendMessage(inner, MakeKeyAndOrderFrontSel, IntPtr.Zero);
	}

	public string Title {
		get => new NsString(ObjC.SendMessage(inner, GetTitleSel)).Value;
		set => ObjC.SendMessage(inner, SetTitleSel, new NsString(value).inner);
	}

	public CgRect Frame => ObjC.SendMessage<CgRect>(inner, FrameSel);

	public NsView ContentView => new() { inner = ObjC.SendMessage(inner, "contentView") };

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial IntPtr SendMessage (
		IntPtr cls, IntPtr selector, CgRect frame, long styleMask, long backing,
		int defer
	);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, CgPoint position);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, CgSize size);

	public void SetFrameOrigin (double x, double y) {
		SendMessage(inner, SetFrameOriginSel, new CgPoint { x = x, y = y });
	}

	public void SetFrameOrigin (CgPoint point) {
		SendMessage(inner, SetFrameOriginSel, point);
	}

	public void SetInitialFirstResponder (NsView view) {
		ObjC.SendMessage(inner, SetInitialFirstResponderSel, view.inner);
	}

	public void Append (NsControl child) {
		child.parent = this;
		ContentView.AddSubview(child);
		child.OnAttach();
	}

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

	private class Del: NativeManaged<IntPtr> {
		public static readonly ObjClass Proto;

		static Del () {
			Proto = NsObject.Proto.NewSubClass("NSWindowDelegate");
			// class_addProtocol(myWindowDelegateClass, @protocol(NSWindowDelegate));
			Proto.AddMethod("windowWillClose:", (WindowWillCloseFn) ((_, _, _) => Console.WriteLine("Smth!")), "v@:@");
		}

		public Del () {
			Proto.Construct(this);
		}
	}
}
