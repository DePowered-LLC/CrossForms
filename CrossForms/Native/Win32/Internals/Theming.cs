using System.Runtime.InteropServices;

namespace CrossForms.Native.Win32;
internal partial class Internals {
	[DllImport("uxtheme.dll")]
	public static extern void SetThemeAppProperties (ThemeAppProperty flags);

    [DllImport("uxtheme.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int SetWindowTheme (IntPtr handle, string appName, string subIds);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr CreateActCtx (ref ActivationContext contextStruct);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ActivateActCtx (IntPtr context, out IntPtr cookie);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool DeactivateActCtx (int flags, IntPtr cookie);
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
    LangIDValid = 0x002,
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
