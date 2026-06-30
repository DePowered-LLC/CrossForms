using System.Runtime.InteropServices;

namespace CrossForms.Native.Win32.Internals;


internal partial class Internals {
	// Shell_NotifyIcon commands
	public const uint NimAdd = 0x00000000;
	public const uint NimModify = 0x00000001;
	public const uint NimDelete = 0x00000002;

	// NOTIFYICONDATA flags
	public const uint NifMessage = 0x00000001;
	public const uint NifIcon = 0x00000002;
	public const uint NifTip = 0x00000004;

	// WM_TRAY lParam notifications
	public const uint WmRButtonUp = 0x0205;
	public const uint WmNull2 = 0x0000;

	// WM_ACTIVATEAPP
	public const uint WmActivateApp = 0x001C;

	// TrackPopupMenu flags
	public const uint TpmBottomAlign = 0x0020;
	public const uint TpmRightAlign = 0x0008;
	public const uint TpmReturnCmd = 0x0100;

	// AppendMenu flags
	public const uint MfString = 0x00000000;
	public const uint MfGrayed = 0x00000001;

	// SetWindowPos flags
	public const uint SwpShowWindow = 0x0040;
	public const uint SwpNoActivate = 0x0010;

	// LoadImage type
	public const uint ImageIcon = 1;
	public const uint LrLoadFromFile = 0x00000010;

	// Special HWND values for SetWindowPos
	public static readonly IntPtr HwndTopmost = new(-1);

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public unsafe struct NotifyIconData {
		public uint cbSize;
		public IntPtr hWnd;
		public uint uID;
		public uint uFlags;
		public uint uCallbackMessage;
		public IntPtr hIcon;
		public fixed char szTip[128];
		public uint dwState;
		public uint dwStateMask;
		public fixed char szInfo[256];
		public uint uTimeoutOrVersion;
		public fixed char szInfoTitle[64];
		public uint dwInfoFlags;
	}

	[LibraryImport("shell32.dll", EntryPoint = "Shell_NotifyIconW")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool ShellNotifyIcon (uint dwMessage, ref NotifyIconData pnid);

	[StructLayout(LayoutKind.Sequential)]
	public struct NotifyIconIdentifier {
		public uint cbSize;
		public IntPtr hWnd;
		public uint uID;
		public Guid guidItem;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct NativeRect {
		public int left, top, right, bottom;
	}

	[LibraryImport("shell32.dll")]
	public static partial int Shell_NotifyIconGetRect (ref NotifyIconIdentifier identifier, out NativeRect iconLocation);

	[LibraryImport("user32.dll")]
	public static partial IntPtr CreatePopupMenu ();

	[LibraryImport("user32.dll", EntryPoint = "AppendMenuW", StringMarshalling = StringMarshalling.Utf16)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool AppendMenu (IntPtr hMenu, uint uFlags, uint uIdNewItem, string lpNewItem);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool DestroyMenu (IntPtr hMenu);

	[LibraryImport("user32.dll")]
	public static partial int TrackPopupMenu (IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

	[LibraryImport("user32.dll", EntryPoint = "PostMessageW")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool PostMessage (IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool SetForegroundWindow (IntPtr hWnd);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool SetWindowPos (IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
}
