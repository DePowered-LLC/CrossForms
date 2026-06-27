using System.Runtime.InteropServices;

namespace CrossForms.Native.Win32.Internals;


internal partial class Internals {
	public delegate IntPtr WndProc (IntPtr window, uint msg, IntPtr wParam, IntPtr lParam);

	public const uint WmDestroy = 0x0002;
	public const uint WmClose = 0x0010;
	public const uint WmCommand = 0x0111;
	public const uint WmQuit = 0x0012;
	public const uint WmLButtonUp = 0x0202;
	public const uint WmUser = 0x0400;
	public const uint WmTray = WmUser + 1;
	public const uint WmSize = 0x0005;
	public const uint WmSetFont = 0x0030;
	public const uint WmCtlColorBtn = 0x0135;
	public const uint WmCtlColorStatic = 0x0138;

	[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
	private static extern ushort RegisterClassEx ([In] ref WindowClassEx lpwcx);

	[LibraryImport("user32.dll", EntryPoint = "UnregisterClassW", StringMarshalling = StringMarshalling.Utf16)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static partial bool UnregisterClass (string className, IntPtr hInstance);

	[LibraryImport("kernel32.dll", EntryPoint = "GetModuleHandleW", StringMarshalling = StringMarshalling.Utf16)]
	private static partial IntPtr GetModuleHandleRaw (string? moduleName);
	public static IntPtr GetModuleHandle (string? moduleName = null) => GetModuleHandleRaw(moduleName);

	[LibraryImport("user32.dll", EntryPoint = "CreateWindowExW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
	public static partial IntPtr CreateWindowEx (
		WindowStyleEx exStyle, string className, string windowName,
		WindowStyle style, int x, int y, int width, int height,
		IntPtr parent, IntPtr menu, IntPtr instance, IntPtr lpParam
	);

	[LibraryImport("user32.dll", EntryPoint = "DefWindowProcW")]
	public static partial IntPtr DefWindowProc (IntPtr handle, uint msg, IntPtr wParam, IntPtr lParam);

	[LibraryImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool UpdateWindow (IntPtr handle);

	[LibraryImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool ShowWindow (IntPtr handle, ShowWindowCommand command);

	[LibraryImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
	public static partial IntPtr GetWindowLongPtr (IntPtr handle, Gwl index);

	[LibraryImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
	public static partial IntPtr SetWindowLongPtr (IntPtr handle, Gwl index, IntPtr value);

	[LibraryImport("user32.dll")]
	public static partial IntPtr SetParent (IntPtr child, IntPtr parent);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool DestroyWindow (IntPtr hWnd);

	[LibraryImport("user32.dll")]
	public static partial void PostQuitMessage (int exitCode);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool GetCursorPos (out NativePoint pos);

	[LibraryImport("user32.dll", EntryPoint = "GetWindowTextW")]
	private static unsafe partial int GetWindowTextRaw (IntPtr hWnd, char* lpString, int nMaxCount);

	[LibraryImport("user32.dll", EntryPoint = "SetWindowTextW", StringMarshalling = StringMarshalling.Utf16)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool SetWindowText (IntPtr hWnd, string lpString);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool EnableWindow (IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool bEnable);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool SetProcessDpiAwarenessContext (IntPtr value);

	[LibraryImport("user32.dll")]
	public static partial uint GetDpiForSystem ();

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool IsWindowEnabled (IntPtr hWnd);

	[LibraryImport("user32.dll", EntryPoint = "SendMessageW")]
	public static partial IntPtr SendMessage (IntPtr hWnd, uint msg, int wParam, int lParam);

	[LibraryImport("user32.dll", EntryPoint = "SendMessageW")]
	public static partial IntPtr SendMessage (IntPtr hWnd, uint msg, int wParam, IntPtr lParam);

	[LibraryImport("user32.dll", EntryPoint = "SendMessageW")]
	public static partial IntPtr SendMessage (IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

	[LibraryImport("user32.dll", EntryPoint = "SendMessageW", StringMarshalling = StringMarshalling.Utf16)]
	public static partial IntPtr SendMessage (IntPtr hWnd, uint msg, int wParam, string lParam);

	[LibraryImport("user32.dll", EntryPoint = "LoadImageW", StringMarshalling = StringMarshalling.Utf16)]
	public static partial IntPtr LoadImage (IntPtr hinst, string lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

	[LibraryImport("gdi32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool DeleteObject (IntPtr hObject);

	[LibraryImport("gdi32.dll")]
	public static partial IntPtr GetStockObject (int fnObject);

	[LibraryImport("gdi32.dll", EntryPoint = "CreateFontW", StringMarshalling = StringMarshalling.Utf16)]
	public static partial IntPtr CreateFont (
		int nHeight, int nWidth, int nEscapement, int nOrientation, int fnWeight,
		uint fdwItalic, uint fdwUnderline, uint fdwStrikeOut,
		uint fdwCharSet, uint fdwOutputPrecision, uint fdwClipPrecision,
		uint fdwQuality, uint fdwPitchAndFamily, string lpszFace
	);

	[LibraryImport("gdi32.dll")]
	public static partial uint SetBkColor (IntPtr hdc, uint color);

	[LibraryImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool InvalidateRect (IntPtr hWnd, IntPtr lpRect, [MarshalAs(UnmanagedType.Bool)] bool bErase);

	public static unsafe string GetWindowText (IntPtr hWnd) {
		var buf = stackalloc char[256];
		GetWindowTextRaw(hWnd, buf, 256);
		return new string(buf);
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WindowClassEx {
		public uint cbSize;
		public uint style;
		[MarshalAs(UnmanagedType.FunctionPtr)] public WndProc lpfnWndProc;
		public int cbClsExtra;
		public int cbWndExtra;
		public IntPtr hInstance;
		public IntPtr hIcon;
		public IntPtr hCursor;
		public IntPtr hbrBackground;
		public string lpszMenuName;
		public string lpszClassName;
		public IntPtr hIconSm;

		public static WindowClassEx CreateDefault (string className, WndProc onMessage) {
			return new WindowClassEx {
				cbSize = (uint) Marshal.SizeOf<WindowClassEx>(),
				lpfnWndProc = onMessage,
				hInstance = GetModuleHandle(),
				lpszClassName = className,
				hbrBackground = (IntPtr) (5 + 1) // COLOR_WINDOW + 1
			};
		}

		public bool Register () {
			return RegisterClassEx(ref this) != 0;
		}

		public void UnRegister () {
			UnregisterClass(lpszClassName, GetModuleHandle());
		}
	}

}
