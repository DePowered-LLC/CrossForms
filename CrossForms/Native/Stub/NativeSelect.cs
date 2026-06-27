using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public abstract class NativeSelectBase: ISelect {
	public int SelectedIndex {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public bool Enabled {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public EventHandler<SelectChangeEvent> OnChange {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}
}


public class NativeSelect<T>: NativeSelectBase {
	public NativeSelect (Func<T, string>? label = null) { }

	public T[] TypedItems {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public T? ActiveItem => throw new NotImplementedException();
}
