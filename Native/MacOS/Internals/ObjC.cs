using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

internal partial class ObjC {
	public const string COCOA = "/System/Library/Frameworks/Cocoa.framework/Cocoa";

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector);

	[DllImport(COCOA, EntryPoint = "objc_msgSend", CharSet = CharSet.Ansi)]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, string arg1);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, int arg1);

	public static IntPtr SendMessage (IntPtr receiver, string selector) {
		return SendMessage(receiver, ObjSelector.Get(selector));
	}

	[DllImport(COCOA, EntryPoint = "objc_msgSend_stret")]
	private static extern void SendMessageStruct (IntPtr returnPtr, IntPtr cls, IntPtr selector);
	public static Cell<T> SendMessage<T> (IntPtr receiver, IntPtr selector) where T: struct {
		var returnPtr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
		SendMessageStruct(returnPtr, receiver, selector);
		return new Cell<T>(returnPtr);
	}
}
