using System.Runtime.InteropServices;

namespace CrossForms.Native.Win32.Internals;


internal partial class Internals {
	[LibraryImport("uxtheme.dll")]
	public static partial void SetThemeAppProperties (ThemeAppProperty flags);

	[LibraryImport("uxtheme.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
	public static partial int SetWindowTheme (IntPtr handle, string appName, string subIds);

	[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
	public static extern IntPtr CreateActCtx (ref ActivationContext contextStruct);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool ActivateActCtx (IntPtr context, out IntPtr cookie);

	[LibraryImport("kernel32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static partial bool DeactivateActCtx (int flags, IntPtr cookie);
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct ActivationContext {
	public int cbSize;
	public ActivationFlag dwFlags;
	public string lpSource;
	public ushort wProcessorArchitecture;
	public ushort wLangId;
	public string lpAssemblyDirectory;
	public IntPtr lpResourceName;
	public string lpApplicationName;
	public IntPtr hModule;
}

[Flags]
public enum ActivationFlag: uint {
	ProcessorArchValid = 0x001,
	LangIdValid = 0x002,
	AssemblyDirValid = 0x004,
	ResourceNameValid = 0x008,
	SetProcessDefault = 0x010,
	AppNameValid = 0x020,
	HModuleValid = 0x080
}

// private const uint RT_MANIFEST = 24;
// private const uint CREATEPROCESS_MANIFEST_RESOURCE_ID = 1;
// private const uint ISOLATIONAWARE_MANIFEST_RESOURCE_ID = 2;
// private const uint ISOLATIONAWARE_NOSTATICIMPORT_MANIFEST_RESOURCE_ID = 3;

public enum ThemeAppProperty: uint {
	AllowNonClient = 0x00000001,
	AllowControls = 0x00000002,
	AllowWebContent = 0x00000004
}
