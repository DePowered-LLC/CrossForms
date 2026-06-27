using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;

internal static unsafe class Dispatch {
    private static readonly IntPtr _lib = NativeLibrary.Load("/usr/lib/system/libdispatch.dylib");
    private static readonly IntPtr MainQueue = NativeLibrary.GetExport(_lib, "_dispatch_main_q");

    private static readonly delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void> _asyncF =
        (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>)
        NativeLibrary.GetExport(_lib, "dispatch_async_f");

    private static readonly delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void> _syncF =
        (delegate* unmanaged[Cdecl]<IntPtr, IntPtr, IntPtr, void>)
        NativeLibrary.GetExport(_lib, "dispatch_sync_f");

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void Callback(IntPtr context) {
        var handle = GCHandle.FromIntPtr(context);
        var action = (Action)handle.Target!;
        handle.Free();
        action();
    }

    private static readonly IntPtr CallbackPtr = (IntPtr)(delegate* unmanaged[Cdecl]<IntPtr, void>)&Callback;

    public static void Post(Action action) {
        var handle = GCHandle.Alloc(action);
        _asyncF(MainQueue, GCHandle.ToIntPtr(handle), CallbackPtr);
    }

    public static void Send(Action action) {
        var handle = GCHandle.Alloc(action);
        _syncF(MainQueue, GCHandle.ToIntPtr(handle), CallbackPtr);
    }
}
