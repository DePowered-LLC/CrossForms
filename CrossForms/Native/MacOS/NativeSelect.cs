using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public abstract class NativeSelectBase: ISelect, INativeAttachable, INativeFocusable {
	internal NsPopUpButton? nsPopUpButton;

	protected int selectedIndex;
	protected bool enabled = true;
	protected EventHandler<SelectChangeEvent>? onChange;

	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 200;
	public ushort Height { get; set; } = 26;

	public int SelectedIndex {
		get => nsPopUpButton?.SelectedIndex ?? selectedIndex;
		set {
			selectedIndex = value;
			nsPopUpButton?.SelectedIndex = value;
		}
	}

	public bool Enabled {
		get => nsPopUpButton?.Enabled ?? enabled;
		set {
			enabled = value;
			nsPopUpButton?.Enabled = value;
		}
	}

	public EventHandler<SelectChangeEvent> OnChange {
		get => onChange!;
		set => onChange = value;
	}

	public NsView? FocusView => nsPopUpButton;

	public void AttachTo (NsWindow window) {
		var pb = CreateNsPopUpButton();
		nsPopUpButton = pb;
		window.Append(pb);
		pb.ApplyConstraints(window, X, Y, Width, Height);
	}

	internal abstract NsPopUpButton CreateNsPopUpButton ();
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
			if (nsPopUpButton != null) ReloadItems();
		}
	}

	public T? ActiveItem => selectedIndex >= 0 && selectedIndex < _typedItems.Length
		? _typedItems[selectedIndex]
		: default;

	private void ReloadItems () {
		nsPopUpButton!.RemoveAllItems();
		foreach (var item in _typedItems) {
			var nsItemStr = NsString.CloneOwned(_label(item));
			nsPopUpButton.AddItem(nsItemStr);
			nsItemStr.Release();
		}
	}

	internal override NsPopUpButton CreateNsPopUpButton () {
		var pb = NsPopUpButton.CreateOwned();
		foreach (var item in _typedItems) {
			var nsItemStr = NsString.CloneOwned(_label(item));
			pb.AddItem(nsItemStr);
			nsItemStr.Release();
		}
		
		pb.SelectedIndex = selectedIndex;
		if (!enabled) pb.Enabled = false;
		
		pb.OnChange(() => {
			if (pb.SelectedIndex == selectedIndex) return;
			
			selectedIndex = pb.SelectedIndex;
			onChange?.Invoke(this, new SelectChangeEvent { selectedIndex = selectedIndex });
		});
		
		return pb;
	}
}

// todo: check that all *Auto retained if necessary and all *Owned is released
