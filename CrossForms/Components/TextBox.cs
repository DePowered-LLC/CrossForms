using System.Diagnostics.CodeAnalysis;

namespace CrossForms.Components;


public class TextBox: NativeTextBox {
	[SetsRequiredMembers]
	public TextBox (string text = "") {
		Text = text;
	}
}
