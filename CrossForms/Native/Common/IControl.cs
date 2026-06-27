namespace CrossForms.Native.Common;


public interface IControl<TSelf> where TSelf: class, IControl<TSelf> {
	TSelf? Parent { get; protected set; }
	List<TSelf>? Children { get; protected set; }

	void Append (TSelf child);
	void Remove ();
	void Destroy ();
}
