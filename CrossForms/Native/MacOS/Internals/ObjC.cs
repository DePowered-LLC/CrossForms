using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;

internal partial class ObjC {
	public const string COCOA = "/System/Library/Frameworks/Cocoa.framework/Cocoa";

	private static readonly IntPtr _cocoaHandle = NativeLibrary.Load(COCOA);
	private static readonly IntPtr _msgSendPtr = NativeLibrary.GetExport(_cocoaHandle, "objc_msgSend");
	private static readonly IntPtr _msgSendStretPtr =
		RuntimeInformation.ProcessArchitecture != Architecture.Arm64
			? NativeLibrary.GetExport(_cocoaHandle, "objc_msgSend_stret")
			: IntPtr.Zero;

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector);

	[DllImport(COCOA, EntryPoint = "objc_msgSend", CharSet = CharSet.Ansi)]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, string arg1);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, int arg1);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, bool arg1);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, float arg1);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1, float arg2);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, double arg1);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1, double arg2);

	[DllImport(COCOA, EntryPoint = "objc_msgSend")]
	public static extern IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1, int arg2, int arg3, IntPtr arg4, int arg5, float arg6, float arg7);

	public static IntPtr SendMessage (IntPtr receiver, string selector) {
		return SendMessage(receiver, ObjSelector.Get(selector));
	}

	public static unsafe T SendMessage<T> (IntPtr receiver, IntPtr selector) where T : unmanaged {
		if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64) {
			return ((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, T>)_msgSendPtr)(receiver, selector);
		} else {
			T result = default;
			((delegate* unmanaged[Cdecl]<T*, IntPtr, IntPtr, void>)_msgSendStretPtr)(&result, receiver, selector);
			return result;
		}
	}
}
