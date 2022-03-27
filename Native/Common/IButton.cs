namespace CrossForms.Native.Common;
public interface IButton {
	string text { get; }
	int x { get; }
	int y { get; }
	ushort width { get; }
	ushort height { get; }

	EventHandler<ClickEvent> OnClick { get; set; }
}

public struct ClickEvent {
	public int x;
	public int y;
}
