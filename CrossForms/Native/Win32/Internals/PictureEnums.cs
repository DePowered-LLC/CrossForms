namespace CrossForms.Native.Win32.Internals;


public enum StaticStyle: uint {
	Bitmap = 0x000E, // SS_BITMAP
}

public enum StaticMessage: uint {
	SetImage = 0x0172, // STM_SETIMAGE
}

public enum ImageType: uint {
	Bitmap = 0, // IMAGE_BITMAP
}
