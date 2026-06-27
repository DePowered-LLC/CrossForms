namespace CrossForms.Native.Common;


public interface IButton: IEnabled {
	string Text { get; }
	int X { get; }
	int Y { get; }
	ushort Width { get; }
	ushort Height { get; }

	EventHandler<ClickEvent> OnClick { get; set; }

	void SetNextControl (IButton next);
}

public struct ClickEvent {
	public int x;
	public int y;
}
