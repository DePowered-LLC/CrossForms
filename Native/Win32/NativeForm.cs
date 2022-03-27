using CrossForms.Native.Common;
using static CrossForms.Native.Win32.Internals;

namespace CrossForms.Native.Win32;
public class NativeForm: Control, IForm {
	private WindowClassEx windowClass;

	public string id { get; set; }
	public string title { get; set; }
	public ushort width { get; set; } = 420;
	public ushort height { get; set; } = 380;

	protected override ControlCreationOptions GetCreationOptions () {
		windowClass = WindowClassEx.CreateDefault(id, OnWindowMessage);
		if (!windowClass.Register()) {
			throw new Win32Exception($"Cannot register \"{id}\" window class");
		}

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
			case WM_COMMAND:
				return OnWindowCommand(High(wParam), lParam);
			// Window close requested
			case WM_CLOSE:
				DestroyWindow(window);
				return (IntPtr) 0;
			// Window starts to be destoyed
			case WM_DESTROY:
				UnLoad();
				return (IntPtr) 0;
		}

		return DefWindowProc(window, msg, wParam, lParam);
	}

	private IntPtr OnWindowCommand (ushort command, IntPtr controlHandle) {
		var control = GetChild(controlHandle);
		if (control == null) return (IntPtr) (-1);
		else return control.DispatchEvent(command);
	}

	public void Show () {
		if (!IsLoaded) Load();
		ShowWindow(handle, ShowWindowCommand.ShowNormal);
	}

	protected override void UnLoad () {
		windowClass.UnRegister();

		if (this == Application.mainWindow) PostQuitMessage(0);
	}
}
