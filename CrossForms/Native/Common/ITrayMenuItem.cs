namespace CrossForms.Native.Common;


public interface ITrayMenuItem {
	string Text { get; set; }
	bool Enabled { get; set; }
	Action? OnClick { get; set; }
}
