using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

[StructLayout(LayoutKind.Sequential)]
public struct CGRect {
	public CGPoint origin;
	public CGSize size;

	public CGRect (double x, double y, double width, double height) {
		origin = new CGPoint { x = x, y = y };
		size = new CGSize { width = width, height = height };
	}

	public static implicit operator NativeArg (CGRect value) => NativeArg.From(value);
}

[StructLayout(LayoutKind.Sequential)]
public struct CGPoint {
	public double x, y;

	public static implicit operator NativeArg (CGPoint value) => NativeArg.From(value);
}

[StructLayout(LayoutKind.Sequential)]
public struct CGSize {
	public double height, width;
}
