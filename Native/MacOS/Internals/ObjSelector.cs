using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS;

public class ObjSelector {
	public delegate IntPtr GetUidFn (string name);
	public static readonly GetUidFn Get;

	public delegate IntPtr RegisterNameFn (string name);
	public static readonly RegisterNameFn Register;

	private delegate IntPtr GetNameFn (IntPtr selector);
	private static readonly GetNameFn _GetName;
	public static string? GetName (IntPtr selector) {
		return Marshal.PtrToStringAnsi(_GetName(selector));
	}

	static ObjSelector () {
		Get = ObjC.GetFunction<GetUidFn>("sel_getUid");
		Register = ObjC.GetFunction<RegisterNameFn>("sel_registerName");
		_GetName = ObjC.GetFunction<GetNameFn>("sel_getName");
	}
}
