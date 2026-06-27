namespace CrossForms.Native.Common;
public interface IForm {
	string id { get; }
	string title { get; }
	ushort width { get; }
	ushort height { get; }

	void Show ();
}
