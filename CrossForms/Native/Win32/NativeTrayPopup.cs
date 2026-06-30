using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeTrayPopup: Control, ITrayPopup {
	private WindowClassEx _windowClass;

	public NativeTrayPopup () {
		Children = [];
	}

	public ushort Width { get; set; } = 300;
	public ushort Height { get; set; } = 200;

	protected override ControlCreationOptions GetCreationOptions () {
		var id = "TrayPopup_" + GetHashCode();
		_windowClass = WindowClassEx.CreateDefault(id, OnMessage);
		if (!_windowClass.Register()) {
			throw new Win32Exception("Cannot register TrayPopup window class");
		}

		return new ControlCreationOptions {
			className = id,
			label = "",
			style = WindowStyle.Popup,
			styleEx = WindowStyleEx.Toolbar | WindowStyleEx.Topmost,
			width = Width,
			height = Height,
			x = -Width,
			y = -Height
		};
	}

	private static readonly IntPtr WhiteBrush = GetStockObject(0);

	private IntPtr OnMessage (IntPtr window, uint msg, IntPtr wParam, IntPtr lParam) {
		switch (msg) {
			case WmCommand:
				return OnWindowCommand(High(wParam), lParam);
			case WmActivateApp when wParam == IntPtr.Zero:
				ShowWindow(window, ShowWindowCommand.Hide);
				return 0;
			case WmCtlColorStatic:
			case WmCtlColorBtn:
				if (!IsWindowEnabled(lParam)) break;
				SetBkColor(wParam, 0x00FFFFFF);
				return WhiteBrush;
			case WmDestroy:
				_windowClass.UnRegister();
				return 0;
		}

		return DefWindowProc(window, msg, wParam, lParam);
	}

	private IntPtr OnWindowCommand (ushort command, IntPtr controlHandle) {
		var control = GetChild(controlHandle);
		return control?.DispatchEvent(command) ?? -1;
	}

	public void ShowNear (double x, double y) {
		if (!IsLoaded) Load();
		var dpi = NativeApplicationBase.DpiScale;
		SetWindowPos(handle, HwndTopmost, (int) (x * dpi), (int) (y * dpi), (int) (Width * dpi), (int) (Height * dpi), SwpShowWindow);
		SetForegroundWindow(handle);
	}

	public void Hide () {
		if (IsLoaded) ShowWindow(handle, ShowWindowCommand.Hide);
	}

	// todo: commonize append
	public void Append (NativeButton button) => Append((Control) button);
	public void Append (NativeLabel label) => Append((Control) label);
	public void Append (NativeCheckBox checkBox) => Append((Control) checkBox);
	public void Append (NativeTextBox textBox) => Append((Control) textBox);
	public void Append (NativePictureBox pictureBox) => Append((Control) pictureBox);
	public void Append (NativeProgressBar progressBar) => Append((Control) progressBar);
	public void Append (NativeSelectBase select) => Append((Control) select);

	public void Append (NativeRadioGroup group) {
		foreach (var item in group.Items) {
			Append((NativeRadioButton) item);
		}
		
		if (group.SelectedIndex >= 0 && group.SelectedIndex < group.Items.Length) {
			((NativeRadioButton) group.Items[group.SelectedIndex]).Checked = true;
		}
	}
}
