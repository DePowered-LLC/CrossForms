using System.Runtime.InteropServices;

namespace CrossForms.Native.Win32;
internal partial class Internals {
	[StructLayout(LayoutKind.Sequential)]
	public struct NativeMessage {
		public IntPtr handle;
		public uint message;
		public UIntPtr wParam;
		public IntPtr lParam;
		public ulong time;
		public NativePoint pt;
	}

	public struct NativePoint {
		public int x;
		public int y;
	}

	[DllImport("user32.dll")]
	public static extern bool GetMessage (out NativeMessage lpMsg, IntPtr windowHandle, uint wMsgFilterMin, uint wMsgFilterMax);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	public static extern long FormatMessage (FormatMessageFlag dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, out string lpBuffer, uint nSize);
	[Flags]
	public enum FormatMessageFlag: uint {
		AllocateBuffer = 0x00000100,
		FromSystem = 0x00001000,
		IgnoreInserts = 0x00000200
	}

	[DllImport("user32.dll")]
	public static extern bool TranslateMessage ([In] ref NativeMessage msg);

	[DllImport("user32.dll")]
	public static extern bool DispatchMessage ([In] ref NativeMessage msg);
}
