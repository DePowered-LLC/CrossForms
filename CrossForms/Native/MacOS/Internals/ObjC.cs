using System.Runtime.InteropServices;

using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


internal static partial class ObjC {
	public const string CocoaPath = "/System/Library/Frameworks/Cocoa.framework/Cocoa";

	private static readonly IntPtr CocoaHandle = NativeLibrary.Load(CocoaPath);
	private static readonly IntPtr MsgSendPtr = NativeLibrary.GetExport(CocoaHandle, "objc_msgSend");

	private static readonly IntPtr MsgSendStretPtr =
		RuntimeInformation.ProcessArchitecture != Architecture.Arm64
			? NativeLibrary.GetExport(CocoaHandle, "objc_msgSend_stret")
			: IntPtr.Zero;

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend", StringMarshalling = StringMarshalling.Utf8)]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, string arg1);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, int arg1);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, byte arg1);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, float arg1);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1, float arg2);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, double arg1);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1, double arg2);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1, nint arg2);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (
		IntPtr cls, IntPtr selector, IntPtr arg1, int arg2, int arg3, IntPtr arg4,
		int arg5, float arg6, float arg7
	);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (
		IntPtr cls, IntPtr selector, IntPtr arg1, IntPtr arg2, IntPtr arg3, IntPtr arg4
	);
	
	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (
		IntPtr cls, IntPtr selector, CgRect arg1, long arg2, long arg3, IntPtr arg4
	);
	
	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, IntPtr arg1, IntPtr arg2, IntPtr arg3);
	
	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, CgPoint point);
	
	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (IntPtr cls, IntPtr selector, CgSize size);
	
	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial IntPtr SendMessage (
		IntPtr cls, IntPtr selector, CgRect frame, long styleMask, long backing,
		int defer
	);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial void SetDouble (IntPtr obj, IntPtr sel, double value);

	[LibraryImport(CocoaPath, EntryPoint = "objc_msgSend")]
	public static partial double GetDouble (IntPtr obj, IntPtr sel);

	
	[LibraryImport(CocoaPath, EntryPoint = "CFRelease")]
	public static partial void ReleaseNsObject (IntPtr ptr);

	public static IntPtr SendMessage (IntPtr receiver, string selector) {
		return SendMessage(receiver, ObjSelector.Get(selector));
	}

	public static unsafe T SendMessage<T> (IntPtr receiver, IntPtr selector) where T: unmanaged {
		if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
			return ((delegate* unmanaged[Cdecl]<IntPtr, IntPtr, T>) MsgSendPtr)(receiver, selector);

		T result = default;
		((delegate* unmanaged[Cdecl]<T*, IntPtr, IntPtr, void>) MsgSendStretPtr)(&result, receiver, selector);
		return result;
	}
}