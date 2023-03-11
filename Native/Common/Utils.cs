using System.Runtime.InteropServices;

namespace CrossForms.Native.Common;

public class NativeStatus {
	public readonly bool success;
	public readonly string error;

	private IntPtr ptr;
	public NativeStatus (IntPtr ptr): this(ref ptr) {}
	public NativeStatus (ref IntPtr cursor) {
		ptr = cursor;
		success = Marshal.ReadByte(cursor) != 0;

		NativeUtils.Step<bool>(ref cursor);
		if (success) {
			error = string.Empty;
		} else {
			error = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(cursor)) ?? string.Empty;
		}

		NativeUtils.Step<IntPtr>(ref cursor);
	}

	public void Dispose () {
		Marshal.FreeHGlobal(ptr);
	}
}

public class NativeResult<T>: NativeStatus where T: struct {
	public readonly T value;

	public NativeResult (ref IntPtr cursor): base(ref cursor) {
		value = Marshal.PtrToStructure<T>(cursor);
		NativeUtils.Step<T>(ref cursor);
	}
}

public abstract class NativeManaged<N> where N: struct {
	public N inner;
}

public class Cell<N> where N: struct {
	protected IntPtr ptr;
	public N value;
	public Cell (IntPtr ptr) {
		this.ptr = ptr;
		value = Marshal.PtrToStructure<N>(ptr);
	}

	~Cell () {
		Marshal.DestroyStructure<N>(ptr);
		Marshal.FreeHGlobal(ptr);
	}
}

public class NativeUtils {
	public static T[] ReadArray<N, T> (int count, Action<IntPtr> consume)
	where N: struct where T: NativeManaged<N>, new() {
		var step = Marshal.SizeOf<N>();
		var bufferPtr = Marshal.AllocHGlobal(step * count);
		consume(bufferPtr);

		var result = new T[count];
		uint i = 0;

		var end = bufferPtr + step * count;
		for (var cursor = bufferPtr; cursor != end; cursor += step, i++) {
			result[i] = new T {
				inner = Marshal.PtrToStructure<N>(cursor)
			};
		}

		Marshal.FreeHGlobal(bufferPtr);
		return result;
	}

	public static void Step<S> (ref IntPtr cursor) {
		var step = Marshal.SizeOf<S>();
		cursor += step + step % IntPtr.Size;
	}
}
