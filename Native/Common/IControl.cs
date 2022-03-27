namespace CrossForms.Native.Common;
public interface IControl<Self> where Self: class, IControl<Self> {
	Self? parent { get; protected set; }
	List<Self>? children { get; protected set; }

	void Append (Self child);
	void Remove ();
	void Destroy ();
}