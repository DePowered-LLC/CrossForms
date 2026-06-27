using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public class NativeRadioGroup: IRadioGroup {
	public IRadioButton[] Items => throw new NotImplementedException();

	public int SelectedIndex {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public EventHandler<RadioChangeEvent> OnChange {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}
}
