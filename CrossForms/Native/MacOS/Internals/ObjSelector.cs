using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


internal partial class ObjSelector {
	[LibraryImport(ObjC.CocoaPath, EntryPoint = "sel_registerName", StringMarshalling = StringMarshalling.Utf8)]
	public static partial IntPtr Register (string selector);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "sel_getUid", StringMarshalling = StringMarshalling.Utf8)]
	public static partial IntPtr Get (string selector);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "sel_getName", StringMarshalling = StringMarshalling.Utf8)]
	public static partial IntPtr GetName (IntPtr selector);
}
