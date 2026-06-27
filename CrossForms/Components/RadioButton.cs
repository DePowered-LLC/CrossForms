using System.Diagnostics.CodeAnalysis;

namespace CrossForms.Components;


public class RadioButton: NativeRadioButton {
	[SetsRequiredMembers]
	public RadioButton (string text) {
		Text = text;
	}
}
