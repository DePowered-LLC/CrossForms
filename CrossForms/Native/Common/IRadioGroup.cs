namespace CrossForms.Native.Common;


public interface IRadioGroup {
	IRadioButton[] Items { get; }
	int SelectedIndex { get; set; }
	EventHandler<RadioChangeEvent> OnChange { get; set; }
}

public struct RadioChangeEvent {
	public int selectedIndex;
}
