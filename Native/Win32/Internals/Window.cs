using System.Runtime.InteropServices;

namespace CrossForms.Native.Win32;
internal partial class Internals {
	public const uint WM_DESTROY = 0x0002;
	public const uint WM_CLOSE = 0x0010;
	public const uint WM_COMMAND = 0x0111;
	public const uint WM_QUIT = 0x0012;
	public const uint WM_LBUTTONUP = 0x0202;
	public const uint WM_USER = 0x0400;
	public const uint WM_TRAY = WM_USER + 1;
	public delegate IntPtr WndProc (IntPtr window, uint msg, IntPtr wParam, IntPtr lParam);

	[StructLayout(LayoutKind.Sequential)]
	public struct WindowClassEx {
		public uint cbSize;
		/* Win 3.x */
		public uint style;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public WndProc lpfnWndProc;
		public int cbClsExtra;
		public int cbWndExtra;
		public IntPtr hInstance;
		public IntPtr hIcon;
		public IntPtr hCursor;
		public IntPtr hbrBackground;
		public string lpszMenuName;
		public string lpszClassName;
		/* Win 4.0 */
		public IntPtr hIconSm;

		public static WindowClassEx CreateDefault (string className, WndProc onMessage) {
			return new WindowClassEx {
				cbSize = (uint) Marshal.SizeOf<WindowClassEx>(),
				lpfnWndProc = onMessage,
				hInstance = GetModuleHandle(),
				lpszClassName = className
			};
		}

		public bool Register () => RegisterClassEx(ref this) != 0;
		public void UnRegister () => UnregisterClass(lpszClassName, GetModuleHandle());
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern ushort RegisterClassEx ([In] ref WindowClassEx lpwcx);

	[DllImport("user32.dll")]
	private static extern bool UnregisterClass (string className, IntPtr hInstance);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	public static extern IntPtr GetModuleHandle (string moduleName = null);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr CreateWindowEx (WindowStyleEx exStyle, string className, string windowName, WindowStyle style, int x, int y, int width, int height, IntPtr parent, IntPtr menu, IntPtr instance, IntPtr lpParam);

	[DllImport("user32.dll")]
	public static extern IntPtr DefWindowProc (IntPtr handle, uint msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern bool UpdateWindow (IntPtr handle);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool ShowWindow (IntPtr handle, ShowWindowCommand command);

	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowLongPtr (IntPtr handle, GWL index);

	[DllImport("user32.dll")]
	public static extern IntPtr SetParent (IntPtr child, IntPtr parent);

	[DllImport("user32.dll")]
	public static extern bool DestroyWindow (IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern void PostQuitMessage (int exitCode);

	[DllImport("user32.dll")]
	public static extern bool GetCursorPos (out NativePoint pos);
}
