using System.Runtime.InteropServices;

namespace CrossForms.Native.Win32.Internals;


internal partial class Internals {
	[Flags]
	public enum FormatMessageFlag: uint {
		AllocateBuffer = 0x00000100,
		FromSystem = 0x00001000,
		IgnoreInserts = 0x00000200
	}

	[LibraryImport("user32.dll", EntryPoint = "GetMessageW")]
	public static partial byte GetMessage (
		out NativeMessage lpMsg, IntPtr windowHandle, uint wMsgFilterMin,
		uint wMsgFilterMax
	);

	[LibraryImport("kernel32.dll", EntryPoint = "FormatMessageA", StringMarshalling = StringMarshalling.Utf8)]
	public static partial long FormatMessage (
		FormatMessageFlag dwFlags, IntPtr lpSource, uint dwMessageId,
		uint dwLanguageId, out string lpBuffer, uint nSize
	);

	[LibraryImport("user32.dll")]
	public static partial byte TranslateMessage (in NativeMessage msg);

	[LibraryImport("user32.dll", EntryPoint = "DispatchMessageW")]
	public static partial byte DispatchMessage (in NativeMessage msg);

	[StructLayout(LayoutKind.Sequential)]
	public struct NativeMessage {
		public IntPtr handle;
		public uint message;
		public UIntPtr wParam;
		public IntPtr lParam;
		public ulong time;
		public NativePoint pt;
	}

	public struct NativePoint (int x, int y) {
		public int x = x;
		public int y = y;
	}
}
