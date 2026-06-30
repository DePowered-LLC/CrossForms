namespace CrossForms.Native.Common;


public interface ITrayItem {
	string IconPath { get; set; }
	byte[]? IconData { get; set; }
	string? Tooltip { get; set; }
	Func<ITrayPopup?>? OnClick { get; set; }
	ITrayMenu? Menu { get; set; }
	void Show ();
	void Hide ();
}
