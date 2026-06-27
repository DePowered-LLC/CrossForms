using CrossForms.Native.Common;

namespace CrossForms.Native.Win32;


public class NativeRadioGroup: IRadioGroup {
	private EventHandler<RadioChangeEvent>? _onChange;

	public IRadioButton[] Items { get; }
	public int SelectedIndex { get; set; }

	public NativeRadioGroup (params IRadioButton[] items) {
		Items = items;
		for (var i = 0; i < items.Length; i++) {
			var rb = (NativeRadioButton) items[i];
			rb.group = this;
			rb.isFirst = i == 0;
		}
	}

	public EventHandler<RadioChangeEvent> OnChange {
		get => _onChange!;
		set => _onChange = value;
	}

	internal void NotifyChange (NativeRadioButton clicked) {
		SelectedIndex = Array.IndexOf(Items, clicked);
		_onChange?.Invoke(this, new RadioChangeEvent { selectedIndex = SelectedIndex });
	}
}
