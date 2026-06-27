using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public abstract class NativeSelectBase: Control, ISelect {
	protected int selectedIndex;
	protected EventHandler<SelectChangeEvent>? onChange;

	public int X { get; set; } = 0;
	public int Y { get; set; } = 0;
	public ushort Width { get; set; } = 200;
	public ushort Height { get; set; } = 26;

	public int SelectedIndex {
		get => IsLoaded
			? (int) SendMessage(handle, (uint) ComboMessage.GetCurSel, 0, 0)
			: selectedIndex;
		set {
			selectedIndex = value;
			if (IsLoaded) SendMessage(handle, (uint) ComboMessage.SetCurSel, value, 0);
		}
	}

	public EventHandler<SelectChangeEvent> OnChange {
		get => onChange!;
		set => onChange = value;
	}

	protected abstract string[] GetDisplayItems ();

	protected override void Load () {
		base.Load();
		foreach (var item in GetDisplayItems())
			SendMessage(handle, (uint) ComboMessage.AddString, 0, item);
		SendMessage(handle, (uint) ComboMessage.SetCurSel, selectedIndex, 0);
	}

	protected void ReloadItems () {
		SendMessage(handle, (uint) ComboMessage.ResetContent, 0, 0);
		foreach (var item in GetDisplayItems())
			SendMessage(handle, (uint) ComboMessage.AddString, 0, item);
	}

	protected override ControlCreationOptions GetCreationOptions () => new() {
		className = "ComboBox",
		style = WindowStyle.Visible | WindowStyle.Child
		      | (WindowStyle) (uint) ComboStyle.DropDownList,
		label = "",
		width = Width,
		height = Height,
		x = X,
		y = Y
	};

	internal override IntPtr DispatchEvent (ushort command) {
		if ((ComboCommand) command == ComboCommand.SelChange) {
			selectedIndex = SelectedIndex;
			onChange?.Invoke(this, new SelectChangeEvent { selectedIndex = selectedIndex });
			return 0;
		}

		return -1;
	}
}


public class NativeSelect<T>: NativeSelectBase {
	private T[] _typedItems = [];
	private readonly Func<T, string> _label;

	public NativeSelect (Func<T, string>? label = null) {
		_label = label ?? (t => t?.ToString() ?? "");
	}

	public T[] TypedItems {
		get => _typedItems;
		set {
			_typedItems = value;
			if (IsLoaded) ReloadItems();
		}
	}

	public T? ActiveItem => selectedIndex >= 0 && selectedIndex < _typedItems.Length
		? _typedItems[selectedIndex]
		: default;

	protected override string[] GetDisplayItems () =>
		Array.ConvertAll(_typedItems, t => _label(t));
}
