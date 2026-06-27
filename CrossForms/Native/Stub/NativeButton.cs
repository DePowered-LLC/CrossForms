using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public class NativeButton: IButton {
	public string Text => throw new NotImplementedException();
	public bool Enabled {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}
	public int X => throw new NotImplementedException();
	public int Y => throw new NotImplementedException();
	public ushort Width => throw new NotImplementedException();
	public ushort Height => throw new NotImplementedException();

	public EventHandler<ClickEvent> OnClick {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public void SetNextControl (IButton next) => throw new NotImplementedException();
}
