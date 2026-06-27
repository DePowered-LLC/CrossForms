using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;


public class NativeRadioGroup: IRadioGroup {
	private EventHandler<RadioChangeEvent>? _onChange;

	public IRadioButton[] Items { get; }
	public int SelectedIndex { get; set; } = 0;

	public NativeRadioGroup (params IRadioButton[] items) {
		Items = items;
		foreach (var item in items) {
			((NativeRadioButton) item).group = this;
		}
	}

	public EventHandler<RadioChangeEvent> OnChange {
		get => _onChange!;
		set => _onChange = value;
	}

	internal void NotifyChange (int index) {
		if (SelectedIndex == index) return;
		
		SelectedIndex = index;
		_onChange?.Invoke(this, new RadioChangeEvent { selectedIndex = index });
	}
}
