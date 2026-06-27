namespace CrossForms.Components;


public class Select<T>: NativeSelect<T> {
	public Select (params T[] items) {
		TypedItems = items;
	}

	public Select (Func<T, string> label, params T[] items) : base(label) {
		TypedItems = items;
	}
}
