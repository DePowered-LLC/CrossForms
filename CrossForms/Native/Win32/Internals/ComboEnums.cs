namespace CrossForms.Native.Win32.Internals;


public enum ComboStyle: uint {
	DropDownList = 0x0003
}

public enum ComboMessage: uint {
	AddString    = 0x0143,
	ResetContent = 0x014B,
	GetCurSel    = 0x0147,
	SetCurSel    = 0x014E
}

public enum ComboCommand: ushort {
	SelChange = 1
}
