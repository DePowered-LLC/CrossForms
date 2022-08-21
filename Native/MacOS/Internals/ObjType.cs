using System.Text;

namespace CrossForms.Native.MacOS;

public static class ObjType {
	public static void Append (StringBuilder builder, Type type) {
		if (type == typeof(void)) builder.Append('v');
		else if (type == typeof(IntPtr)) builder.Append('@');
		else if (type == typeof(ObjSelector)) builder.Append(':');
	}
}
