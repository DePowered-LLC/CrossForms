using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public class NativeTextBox: ITextBox {
	public string Text {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public int X => throw new NotImplementedException();
	public int Y => throw new NotImplementedException();
	public ushort Width => throw new NotImplementedException();
	public ushort Height => throw new NotImplementedException();

	public EventHandler<ChangeEvent> OnChange {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}
}
