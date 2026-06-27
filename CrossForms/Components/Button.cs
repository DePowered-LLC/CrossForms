using System.Diagnostics.CodeAnalysis;

namespace CrossForms.Components;


public class Button: NativeButton {
	[SetsRequiredMembers]
	public Button (string text) {
		Text = text;
	}
}
