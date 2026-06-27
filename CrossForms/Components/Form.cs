using System.Diagnostics.CodeAnalysis;

namespace CrossForms.Components;


public class Form: NativeForm {
	[SetsRequiredMembers]
	public Form (string id, string title) {
		Id = id;
		Title = title;
	}
}
