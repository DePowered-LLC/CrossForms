namespace CrossForms.Native.Common;


public interface ISelect: IEnabled {
	int SelectedIndex { get; set; }
	EventHandler<SelectChangeEvent> OnChange { get; set; }
}

public struct SelectChangeEvent {
	public int selectedIndex;
}
