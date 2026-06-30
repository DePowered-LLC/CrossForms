using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public abstract class Control: IControl<Control>, IEnabled {
	internal static IntPtr AppFont = IntPtr.Zero;

	protected IntPtr handle;
	protected bool IsLoaded => handle != IntPtr.Zero;
	public Control? Parent { get; set; }
	public List<Control>? Children { get; set; }

	private bool _enabled = true;
	public bool Enabled {
		get => _enabled;
		set {
			_enabled = value;
			if (IsLoaded) EnableWindow(handle, value);
		}
	}

	public void Append (Control child) {
		if (Children == null) return;

		if (!IsLoaded) Load();
		child.Parent = this;
		if (!child.IsLoaded) child.Load();
		Children.Add(child);
	}

	public void Remove () {
		if (Children == null) return;
		if (IsLoaded) UnLoad();
		if (Parent == null) return;
		
		Parent.Children?.Remove(this);
		Parent = null;
	}

	public void Destroy () {
		if (Children == null) return;

		if (IsLoaded) UnLoad();
		Parent = null;
		foreach (var child in Children) {
			child.Destroy();
		}
	}

	protected abstract ControlCreationOptions GetCreationOptions ();

	protected virtual void Load () {
		var options = GetCreationOptions();
		var parentInstPtr = Parent != null
			? GetWindowLongPtr(Parent.handle, Gwl.HInstance)
			: GetModuleHandle();

		var dpi = NativeApplicationBase.DpiScale;
		handle = CreateWindowEx(
			options.styleEx,
			options.className,
			options.label,
			options.style,
			(int) (options.x * dpi),
			(int) (options.y * dpi),
			(int) (options.width * dpi),
			(int) (options.height * dpi),
			Parent?.handle ?? IntPtr.Zero,
			IntPtr.Zero,
			parentInstPtr,
			IntPtr.Zero
		);

		if (handle == IntPtr.Zero) throw new Win32Exception("Cannot create control");
		if (!_enabled) EnableWindow(handle, false);
		if (AppFont != IntPtr.Zero) SendMessage(handle, WmSetFont, AppFont, (IntPtr) 1);
	}

	protected virtual void UnLoad () {
		DestroyWindow(handle);
	}

	internal void ResetHandle () => handle = IntPtr.Zero;

	internal void EnsureLoaded () {
		if (!IsLoaded) Load();
	}

	internal Control? GetChild (IntPtr needle) {
		return Children?.FirstOrDefault(control => control.handle == needle);
	}

	internal virtual IntPtr DispatchEvent (ushort command) {
		return -1;
	}
}

public struct ControlCreationOptions {
	public WindowStyle style;
	public WindowStyleEx styleEx;
	public string className;
	public string label;
	public uint width;
	public uint height;
	public int x;
	public int y;
}
