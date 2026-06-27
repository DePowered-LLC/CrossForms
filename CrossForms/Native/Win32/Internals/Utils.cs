using System.Runtime.CompilerServices;

namespace CrossForms.Native.Win32;
internal partial class Internals {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ushort Low (IntPtr param) => (ushort) (((uint) param) & 0xffff);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ushort High (IntPtr param) => (ushort) ((((uint) param) >> 16) & 0xffff);
}
