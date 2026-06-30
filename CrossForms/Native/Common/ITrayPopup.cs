namespace CrossForms.Native.Common;


public interface ITrayPopup {
	ushort Width { get; set; }
	ushort Height { get; set; }
	void ShowNear (double x, double y);
	void Hide ();
}
