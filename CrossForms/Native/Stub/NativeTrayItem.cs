using CrossForms.Native.Common;

namespace CrossForms.Native.Stub;


public class NativeTrayItem: ITrayItem {
	public string IconPath {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public byte[]? IconData {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public string? Tooltip {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public Func<ITrayPopup?>? OnClick {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public ITrayMenu? Menu {
		get => throw new NotImplementedException();
		set => throw new NotImplementedException();
	}

	public void Show () => throw new NotImplementedException();
	public void Hide () => throw new NotImplementedException();
}
