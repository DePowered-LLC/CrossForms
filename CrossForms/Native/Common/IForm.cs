namespace CrossForms.Native.Common;


public interface IForm {
	string Id { get; }
	string Title { get; }
	ushort Width { get; }
	ushort Height { get; }

	void Show ();
	void SetInitialControl (IButton button);
}
