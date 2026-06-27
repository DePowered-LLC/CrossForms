using System.Runtime.InteropServices;

using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


[StructLayout(LayoutKind.Sequential)]
public struct CgRect (double x, double y, double width, double height) {
	public CgPoint origin = new() { x = x, y = y };
	public CgSize size = new() { width = width, height = height };

	public static implicit operator NativeArg (CgRect value) {
		return NativeArg.From(value);
	}
}

[StructLayout(LayoutKind.Sequential)]
public struct CgPoint {
	public double x, y;

	public static implicit operator NativeArg (CgPoint value) {
		return NativeArg.From(value);
	}
}

[StructLayout(LayoutKind.Sequential)]
public struct CgSize {
	public double width, height;
}
