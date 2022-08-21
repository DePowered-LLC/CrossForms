using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;
public class NativeButton : IButton {
	public string text { get; set; }

	public int x { get; set; }

	public int y { get; set; }

	public ushort width { get; set; }

	public ushort height { get; set; }

	public EventHandler<ClickEvent> OnClick { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
