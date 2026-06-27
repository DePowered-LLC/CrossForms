namespace CrossForms.Native.Common;


public interface ICheckBox: IEnabled {
	string Text { get; }
	bool Checked { get; set; }
	int X { get; }
	int Y { get; }
	ushort Width { get; }
	ushort Height { get; }

	EventHandler<CheckEvent> OnChange { get; set; }
}

public struct CheckEvent {
	public bool Checked;
}
