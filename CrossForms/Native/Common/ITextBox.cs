namespace CrossForms.Native.Common;


public interface ITextBox {
	string Text { get; set; }
	int X { get; }
	int Y { get; }
	ushort Width { get; }
	ushort Height { get; }

	EventHandler<ChangeEvent> OnChange { get; set; }
}

public struct ChangeEvent {
	public string Text;
}
