using System.Runtime.InteropServices;
using static CrossForms.Native.Win32.Internals;

namespace CrossForms.Native.Win32;

public class Win32Exception: NativeException {
	public Win32Exception (string message) : base(message) {}

	protected override string PrepareMessage (string message) {
		var errCode = (uint) Marshal.GetLastWin32Error();

		FormatMessage(
			FormatMessageFlag.AllocateBuffer | FormatMessageFlag.FromSystem | FormatMessageFlag.IgnoreInserts,
			IntPtr.Zero,
			errCode,
			0 | 1 << 10, // Current user language (neutral, default)
			out string formatted,
			0
		);

		return $"[0x{errCode:X4}] {message}\n{formatted}";
	}
}
