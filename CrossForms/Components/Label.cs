using System.Diagnostics.CodeAnalysis;

namespace CrossForms.Components;


public class Label: NativeLabel {
	[SetsRequiredMembers]
	public Label (string text) {
		Text = text;
	}
}
