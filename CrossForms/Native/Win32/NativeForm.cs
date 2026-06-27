using CrossForms.Components;
using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeForm: Control, IForm {
	private NativeButton? _initialControl;
	private WindowClassEx windowClass;

	public required string id { get; set; }
	public required string title { get; set; }
	public ushort width { get; set; } = 420;
	public ushort height { get; set; } = 380;

	public void SetInitialControl (IButton button) {
		_initialControl = (NativeButton) button;
	}

	public void Show () {
		if (!IsLoaded) Load();
		ShowWindow(handle, ShowWindowCommand.ShowNormal);
	}

	protected override ControlCreationOptions GetCreationOptions () {
		windowClass = WindowClassEx.CreateDefault(id, OnWindowMessage);
		if (!windowClass.Register()) throw new Win32Exception($"Cannot register \"{id}\" window class");

		return new ControlCreationOptions {
			className = id,
			label = title,
			style = WindowStyle.OverlapperWindow,
			styleEx = WindowStyleEx.Overlapped,
			width = width,
			height = height
		};
	}

	private IntPtr OnWindowMessage (IntPtr window, uint msg, IntPtr wParam, IntPtr lParam) {
		switch (msg) {
			// Some control in window has emitted message
			// https://docs.microsoft.com/en-us/windows/win32/menurc/wm-command#remarks
			case WmCommand:
				return OnWindowCommand(High(wParam), lParam);
			// Window close requested
			case WmClose:
				DestroyWindow(window);
				return 0;
			// Window starts to be destoyed
			case WmDestroy:
				UnLoad();
				return 0;
		}

		return DefWindowProc(window, msg, wParam, lParam);
	}

	private IntPtr OnWindowCommand (ushort command, IntPtr controlHandle) {
		var control = GetChild(controlHandle);
		if (control == null) return -1;
		return control.DispatchEvent(command);
	}

	protected override void UnLoad () {
		windowClass.UnRegister();

		if (Application.mainWindow != null)
			if ((IForm) this == Application.mainWindow)
				PostQuitMessage(0);
	}
}
