using System.Runtime.InteropServices;

using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeApplicationBase: ApplicationBase {
	private static IntPtr _actCtx;

	public new static void Start () {
		var ctx = new ActivationContext {
			cbSize = Marshal.SizeOf<ActivationContext>(),
			dwFlags = ActivationFlag.ResourceNameValid,
			lpSource = Environment.ProcessPath ?? string.Empty,
			lpResourceName = 1
		};

		var ctxHandle = CreateActCtx(ref ctx);
		if (ctxHandle == -1) throw new Win32Exception("Cannot create activation context");

		ActivateActCtx(ctxHandle, out _actCtx);

		// if (!InitCommonControlsEx(ControlClass.NativeFont)) {
		// 	throw new Win32Exception("Common control classes not loaded");
		// }

		// SetThemeAppProperties(ThemeAppProperty.AllowControls | ThemeAppProperty.AllowNonClient);
	}

	public new static bool EventLoop () {
		GetMessage(out var msg, IntPtr.Zero, 0, 0);
		if (msg.message == WmQuit) return false;

		TranslateMessage(in msg);
		DispatchMessage(in msg);
		return true;
	}

	public new static void Dispose () {
		DeactivateActCtx(0, _actCtx);
		_actCtx = IntPtr.Zero;
	}
}
