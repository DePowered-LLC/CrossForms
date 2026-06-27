namespace CrossForms.Components;


public class CheckBox: NativeCheckBox {
	public CheckBox (string text, bool @checked = false) {
		Text = text;
		Checked = @checked;
	}
}
