namespace CrossForms;
public class Form: NativeForm {
	public Form (string id, string title) {
		this.id = id;
		this.title = title;
		children = new();
	}
}
