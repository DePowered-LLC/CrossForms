using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


internal static unsafe class Dispatch {
	private static readonly IntPtr Lib = NativeLibrary.Load("/usr/lib/system/libdispatch.dylib");
	private static readonly IntPtr MainQueue = NativeLibrary.GetExport(Lib, "_dispatch_main_q");

	private static readonly delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void> AsyncF =
		(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>)
		NativeLibrary.GetExport(Lib, "dispatch_async_f");

	private static readonly delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void> SyncF =
		(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>)
		NativeLibrary.GetExport(Lib, "dispatch_sync_f");

	private static readonly IntPtr CallbackPtr = (IntPtr) (delegate* unmanaged[Cdecl]<IntPtr, void>) &Callback;

	[UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
	private static void Callback (IntPtr context) {
		var handle = GCHandle.FromIntPtr(context);
		var action = (Action) handle.Target!;
		handle.Free();
		action();
	}

	public static void Post (Action action) {
		var handle = GCHandle.Alloc(action);
		AsyncF(MainQueue, GCHandle.ToIntPtr(handle), CallbackPtr);
	}

	public static void Send (Action action) {
		var handle = GCHandle.Alloc(action);
		SyncF(MainQueue, GCHandle.ToIntPtr(handle), CallbackPtr);
	}
}
