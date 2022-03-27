using CrossForms.Native.Common;
using static CrossForms.Native.Win32.Internals;

namespace CrossForms.Native.Win32;
public abstract class Control: IControl<Control> {
	public Control? parent { get; set; }
	public List<Control>? children { get; set; }

	protected IntPtr handle;
	protected bool IsLoaded => handle != IntPtr.Zero;

	protected abstract ControlCreationOptions GetCreationOptions ();
	protected virtual void Load () {
		var options = GetCreationOptions();
		var parentInstPtr = parent != null
			? GetWindowLongPtr(parent.handle, GWL.HINSTANCE)
			: GetModuleHandle();

		handle = CreateWindowEx(
			options.styleEx,
			options.className,
			options.label,
			options.style,
			options.x,
			options.y,
			(int) options.width,
			(int) options.height,
			parent?.handle ?? IntPtr.Zero,
			IntPtr.Zero,
			parentInstPtr,
			IntPtr.Zero
		);

		if (handle == IntPtr.Zero) {
			throw new Win32Exception("Cannot create control");
		}
	}

	protected virtual void UnLoad () {
		DestroyWindow(handle);
	}

	public void Append (Control child) {
		if (children == null) return;

		if (!IsLoaded) Load();
		child.parent = this;
		if (!child.IsLoaded) child.Load();
		children.Add(child);
	}

	public void Remove () {
		if (children == null) return;

		if (IsLoaded) UnLoad();
		if (parent != null) {
			parent.children?.Remove(this);
			parent = null;
		}
	}

	public void Destroy () {
		if (children == null) return;

		if (IsLoaded) UnLoad();
		parent = null;
		foreach (var child in children) child.Destroy();
	}

	internal Control GetChild (IntPtr handle) {
		foreach (var control in children) {
			if (control.handle == handle) return control;
		}

		return null;
	}

	internal virtual IntPtr DispatchEvent (ushort command) => (IntPtr) (-1);
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
