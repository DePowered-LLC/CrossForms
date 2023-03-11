using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS;

internal class ObjSelector {
	[DllImport(ObjC.COCOA, EntryPoint = "sel_registerName", CharSet = CharSet.Ansi)]
    public static extern IntPtr Register (string selector);

	[DllImport(ObjC.COCOA, EntryPoint = "sel_getUid", CharSet = CharSet.Ansi)]
    public static extern IntPtr Get (string selector);

	[DllImport(ObjC.COCOA, EntryPoint = "sel_getName", CharSet = CharSet.Ansi)]
    public static extern IntPtr GetName (IntPtr selector);
}
