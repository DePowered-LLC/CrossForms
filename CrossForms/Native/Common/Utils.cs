using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;

namespace CrossForms.Native.Common;


public class NativeStatus {
	public readonly string error;

	private readonly IntPtr _ptr;
	public readonly bool success;
	public NativeStatus (IntPtr ptr): this(ref ptr) {}

	public NativeStatus (ref IntPtr cursor) {
		_ptr = cursor;
		success = Marshal.ReadByte(cursor) != 0;

		NativeUtils.Step<bool>(ref cursor);
		if (success)
			error = string.Empty;
		else
			error = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(cursor)) ?? string.Empty;

		NativeUtils.Step<IntPtr>(ref cursor);
	}

	public void Dispose () {
		Marshal.FreeHGlobal(_ptr);
	}
}

public class NativeResult<T>: NativeStatus where T: struct {
	public readonly T value;

	public NativeResult (ref IntPtr cursor): base(ref cursor) {
		value = Marshal.PtrToStructure<T>(cursor);
		NativeUtils.Step<T>(ref cursor);
	}
}

public abstract class NativeManaged<TN> where TN: struct {
	public TN inner;
}

public class Cell<TN> (IntPtr ptr) where TN: struct {
	protected IntPtr ptr = ptr;
	public TN value = Marshal.PtrToStructure<TN>(ptr);

	~Cell () {
		Marshal.DestroyStructure<TN>(ptr);
		Marshal.FreeHGlobal(ptr);
	}
}

public static class NativeUtils {
	public const DynamicallyAccessedMemberTypes DynConstructors =
		DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors;

	public static TItem[] ReadArray
		<[DynAccess(DynConstructors)] TN, [DynAccess(DynConstructors)] TItem>
		(int count, Action<IntPtr> consume)
		where TN: struct where TItem: NativeManaged<TN>, new() {
		//
		var step = Marshal.SizeOf<TN>();
		var bufferPtr = Marshal.AllocHGlobal(step * count);
		consume(bufferPtr);

		var result = new TItem[count];
		uint i = 0;

		var end = bufferPtr + step * count;
		for (var cursor = bufferPtr; cursor != end; cursor += step, i++) {
			result[i] = new TItem {
				inner = Marshal.PtrToStructure<TN>(cursor)
			};
		}

		Marshal.FreeHGlobal(bufferPtr);
		return result;
	}

	public static void Step<TS> (ref IntPtr cursor) {
		var step = Marshal.SizeOf<TS>();
		cursor += step + step % IntPtr.Size;
	}
}
