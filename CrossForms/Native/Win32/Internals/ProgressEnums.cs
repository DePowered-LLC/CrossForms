namespace CrossForms.Native.Win32.Internals;


public enum ProgressMessage: uint {
	SetPos     = 0x0402,
	SetRange32 = 0x0406,
	GetPos     = 0x0408,
	SetMarquee = 0x040A
}

[Flags]
public enum ProgressStyle: uint {
	Smooth   = 0x01,
	Vertical = 0x04,
	Marquee  = 0x08
}
