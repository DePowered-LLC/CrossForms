using System.Runtime.InteropServices;

namespace CrossForms.Native.Win32.Internals;


internal partial class Internals {
	[StructLayout(LayoutKind.Sequential)]
	private struct GdiplusStartupInput {
		public uint GdiplusVersion;
		public IntPtr DebugEventCallback;
		public int SuppressBackgroundThread;
		public int SuppressExternalCodecs;
	}

	private static IntPtr _gdipToken;

	public static void InitGdiplus () {
		var input = new GdiplusStartupInput { GdiplusVersion = 1 };
		var status = GdiplusStartup(out _gdipToken, in input, IntPtr.Zero);
		if (status != 0) throw new Win32Exception($"GdiplusStartup failed: GDI+ status {status}");
	}

	public static void ShutdownGdiplus () {
		if (_gdipToken == IntPtr.Zero) return;
		GdiplusShutdown(_gdipToken);
		_gdipToken = IntPtr.Zero;
	}

	[LibraryImport("gdiplus.dll")]
	private static partial int GdiplusStartup (out IntPtr token, in GdiplusStartupInput input, IntPtr output);

	[LibraryImport("gdiplus.dll")]
	private static partial void GdiplusShutdown (IntPtr token);

	[LibraryImport("gdiplus.dll")]
	private static partial int GdipCreateBitmapFromStream (IntPtr stream, out IntPtr bitmap);

	[LibraryImport("gdiplus.dll")]
	private static partial int GdipCreateBitmapFromScan0 (int width, int height, int stride, int format, IntPtr scan0, out IntPtr bitmap);

	[LibraryImport("gdiplus.dll")]
	private static partial int GdipGetImageGraphicsContext (IntPtr image, out IntPtr graphics);

	[LibraryImport("gdiplus.dll")]
	private static partial int GdipDrawImageRectI (IntPtr graphics, IntPtr image, int x, int y, int width, int height);

	[LibraryImport("gdiplus.dll")]
	private static partial int GdipDeleteGraphics (IntPtr graphics);

	[LibraryImport("gdiplus.dll")]
	private static partial int GdipCreateHBITMAPFromBitmap (IntPtr bitmap, out IntPtr hbmReturn, uint background);

	[LibraryImport("gdiplus.dll")]
	private static partial int GdipDisposeImage (IntPtr image);

	[LibraryImport("shlwapi.dll")]
	private static partial IntPtr SHCreateMemStream (byte[] pInit, uint cbInit);

	public static IntPtr HBitmapFromImageData (byte[] data, int width, int height) {
		var stream = SHCreateMemStream(data, (uint) data.Length);
		if (stream == IntPtr.Zero) throw new Win32Exception("SHCreateMemStream failed");
		try {
			var status = GdipCreateBitmapFromStream(stream, out var original);
			if (status != 0)
				throw new Win32Exception($"GdipCreateBitmapFromStream failed: GDI+ status {status}");
			try {
				if (width > 0 && height > 0) {
					// PixelFormat32bppARGB = 0x0026200A
					GdipCreateBitmapFromScan0(width, height, 0, 0x0026200A, IntPtr.Zero, out var scaled);
					try {
						GdipGetImageGraphicsContext(scaled, out var g);
						GdipDrawImageRectI(g, original, 0, 0, width, height);
						GdipDeleteGraphics(g);
						GdipCreateHBITMAPFromBitmap(scaled, out var hBitmap, 0xFFFFFFFF);
						return hBitmap;
					} finally {
						GdipDisposeImage(scaled);
					}
				} else {
					GdipCreateHBITMAPFromBitmap(original, out var hBitmap, 0xFFFFFFFF);
					return hBitmap;
				}
			} finally {
				GdipDisposeImage(original);
			}
		} finally {
			Marshal.Release(stream);
		}
	}
}
