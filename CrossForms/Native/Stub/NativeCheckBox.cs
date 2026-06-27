using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public class NativeCheckBox: ICheckBox {
	public string Text => throw new NotImplementedException();

	public bool Enabled {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public bool Checked {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public int X => throw new NotImplementedException();
	public int Y => throw new NotImplementedException();
	public ushort Width => throw new NotImplementedException();
	public ushort Height => throw new NotImplementedException();

	public EventHandler<CheckEvent> OnChange {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}
}
