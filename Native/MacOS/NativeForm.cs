using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;
public class NativeForm : IForm {
	public string id { get; set; }

	public string title { get; set; }

	public ushort width { get; set; }

	public ushort height { get; set; }

	public List<object> children;

	public void Show()
	{
		throw new NotImplementedException();
	}
}
