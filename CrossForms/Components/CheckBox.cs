using System.Diagnostics.CodeAnalysis;

namespace CrossForms.Components;


public class CheckBox: NativeCheckBox {
	[SetsRequiredMembers]
	public CheckBox (string text, bool @checked = false) {
		Text = text;
		Checked = @checked;
	}
}
