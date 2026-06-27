using CrossForms.Components;
using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeForm: Control, IForm {
	private NativeButton? _initialControl;
	private WindowClassEx _windowClass;

	public NativeForm () {
		Children = [];
	}

	public required string Id { get; set; }
	public required string Title { get; set; }
	public ushort Width { get; set; } = 420;
	public ushort Height { get; set; } = 380;

	public void SetInitialControl (IButton button) {
		_initialControl = (NativeButton) button;
	}

	public void Show () {
		if (!IsLoaded) Load();
		ShowWindow(handle, ShowWindowCommand.ShowNormal);
	}

	public void Append (NativePictureBox pictureBox) {
		Append((Control) pictureBox);
	}

	public void Append (NativeProgressBar progressBar) {
		Append((Control) progressBar);
	}

	public void Append (NativeSelectBase select) {
		Append((Control) select);
	}

	public void Append (NativeRadioGroup group) {
		foreach (var item in group.Items) {
			Append((NativeRadioButton) item);
		}
		
		if (group.SelectedIndex >= 0 && group.SelectedIndex < group.Items.Length) {
			((NativeRadioButton) group.Items[group.SelectedIndex]).Checked = true;
		}
	}

	public void Append (NativeRadioButton radioButton) {
		Append((Control) radioButton);
	}

	protected override ControlCreationOptions GetCreationOptions () {
		_windowClass = WindowClassEx.CreateDefault(Id, OnWindowMessage);
		if (!_windowClass.Register()) throw new Win32Exception($"Cannot register \"{Id}\" window class");

		return new ControlCreationOptions {
			className = Id,
			label = Title,
			style = WindowStyle.OverlapperWindow,
			styleEx = WindowStyleEx.Overlapped,
			width = Width,
			height = Height
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
		_windowClass.UnRegister();

		if (Application.MainWindow == null) return;
		if ((IForm) this == Application.MainWindow)
			PostQuitMessage(0);
	}
}
