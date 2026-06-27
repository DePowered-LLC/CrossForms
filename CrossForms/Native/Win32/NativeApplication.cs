using System.Runtime.InteropServices;
using CrossForms.Native.Common;
using static CrossForms.Native.Win32.Internals;

namespace CrossForms.Native.Win32;

public class NativeApplication: IApplication {
	private static IntPtr _actCtx;
	public new static void Start () {
		var ctx = new ActivationContext {
			cbSize = Marshal.SizeOf<ActivationContext>(),
			dwFlags = ActivationFlag.ResourceNameValid,
			lpSource = Environment.ProcessPath ?? string.Empty,
			lpResourceName = (IntPtr) 1
		};

		var ctxHandle = CreateActCtx(ref ctx);
		if (ctxHandle == (IntPtr) (-1)) {
			throw new Win32Exception("Cannot create activation context");
		}

		ActivateActCtx(ctxHandle, out _actCtx);

		// if (!InitCommonControlsEx(ControlClass.NativeFont)) {
		// 	throw new Win32Exception("Common control classes not loaded");
		// }

		// SetThemeAppProperties(ThemeAppProperty.AllowControls | ThemeAppProperty.AllowNonClient);
	}

	public new static bool EventLoop () {
		GetMessage(out var msg, IntPtr.Zero, 0, 0);
		if (msg.message == WM_QUIT) return false;

		TranslateMessage(ref msg);
		DispatchMessage(ref msg);
		return true;
	}

	public new static void Dispose () {
		DeactivateActCtx(0, _actCtx);
		_actCtx = IntPtr.Zero;
	}
}
