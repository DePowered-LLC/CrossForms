using System.Runtime.InteropServices;

using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeApplicationBase: ApplicationBase {
	private static IntPtr _actCtx;
	internal static double DpiScale { get; private set; } = 1.0;

	public new static void Start () {
		// DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = -2
		SetProcessDpiAwarenessContext((IntPtr) (-2));

		var ctx = new ActivationContext {
			cbSize = Marshal.SizeOf<ActivationContext>(),
			dwFlags = ActivationFlag.ResourceNameValid,
			lpSource = Environment.ProcessPath ?? string.Empty,
			lpResourceName = 1
		};

		var ctxHandle = CreateActCtx(ref ctx);
		if (ctxHandle == -1) throw new Win32Exception("Cannot create activation context");

		ActivateActCtx(ctxHandle, out _actCtx);

		InitCommonControlsEx(ControlClass.Progress | ControlClass.Standard);
		InitGdiplus();

		// SetThemeAppProperties(ThemeAppProperty.AllowControls | ThemeAppProperty.AllowNonClient);

		var dpi = GetDpiForSystem();
		DpiScale = dpi / 96.0;

		// 10pt Segoe UI with ClearType, scaled to system DPI
		Control.AppFont = CreateFont(-(int)(10 * dpi / 72.0), 0, 0, 0, 400, 0, 0, 0, 1, 0, 0, 5, 0, "Segoe UI");
	}

	public new static bool EventLoop () {
		GetMessage(out var msg, IntPtr.Zero, 0, 0);
		if (msg.message == WmQuit) return false;

		TranslateMessage(in msg);
		DispatchMessage(in msg);
		return true;
	}

	public new static void Quit () {
		PostQuitMessage(0);
	}

	public new static void Dispose () {
		ShutdownGdiplus();
		DeactivateActCtx(0, _actCtx);
		_actCtx = IntPtr.Zero;
	}
}
