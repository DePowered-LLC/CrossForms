using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CrossForms.Native.Common;
using DelegateCreator = Func<Type[], Type>;

public class NativeArg {
	public Type type;
	public object value;
	public static NativeArg From<T> (T value) where T: notnull {
		return new NativeArg {
			type = typeof(T),
			value = value
		};
	}

	public static implicit operator NativeArg (int value) => From(value);
	public static implicit operator NativeArg (bool value) => From(value ? 1 : 0);
	public static implicit operator NativeArg (long value) => From(value);
	public static implicit operator NativeArg (IntPtr value) => From(value);
	public static implicit operator NativeArg (NativeManaged<IntPtr> value) => value.inner;

	// public delegate void WriteFn (ref IntPtr cursor);
	// public WriteFn Write;
	// public int size;

	// public static NativeArg ForStruct<N> (N value) where N: struct {
	// 	var size = Marshal.SizeOf<N>();
	// 	return new NativeArg {
	// 		Write = (ref IntPtr cursor) => {
	// 			Marshal.StructureToPtr(value, cursor, false);
	// 			// Marshal.Copy(ptr, cursor, 0, size);
	// 			cursor += size;
	// 		},
	// 		size = size
	// 	};
	// }

	// public static implicit operator NativeArg (int value) {
	// 	return new NativeArg {
	// 		Write = (ref IntPtr cursor) => {
	// 			Marshal.WriteInt32(cursor, value);
	// 			cursor += 4;
	// 		},
	// 		size = 4
	// 	};
	// }

	// public static implicit operator NativeArg (long value) {
	// 	return new NativeArg {
	// 		Write = (ref IntPtr cursor) => {
	// 			Marshal.WriteInt64(cursor, value);
	// 			cursor += 8;
	// 		},
	// 		size = 8
	// 	};
	// }

	// public static implicit operator NativeArg (bool value) => value ? 1 : 0;

	// public static implicit operator NativeArg (NativeManaged<IntPtr> value) => value.inner;
	// public static implicit operator NativeArg (IntPtr value) {
	// 	var size = Marshal.SizeOf<IntPtr>();
	// 	return new NativeArg {
	// 		Write = (ref IntPtr cursor) => {
	// 			Marshal.WriteIntPtr(cursor, value);
	// 			cursor += size;
	// 		},
	// 		size = size
	// 	};
	// }

	// public static IntPtr WriteList (NativeArg[] args) {
	// 	var ptr = Marshal.AllocHGlobal(args.Sum(arg => arg.size));

	// 	var cursor = ptr;
	// 	foreach (var arg in args) arg.Write(ref cursor);

	// 	return ptr;
	// }
}

public class NativeFn {
	public static readonly DelegateCreator MakeDelegate;
    static NativeFn () {
#pragma warning disable
		var helperType = typeof(Expression).Assembly.GetType("System.Linq.Expressions.Compiler.DelegateHelpers");
		var creatorMethod = helperType.GetMethod("MakeNewCustomDelegate", BindingFlags.NonPublic | BindingFlags.Static);
		MakeDelegate = (DelegateCreator) Delegate.CreateDelegate(typeof(DelegateCreator), creatorMethod);
#pragma warning restore
	}

	public static R? Apply<R> (IntPtr fnPtr, List<Type> types, List<object> values, NativeArg[] args) {
		foreach (var arg in args) {
			types.Add(arg.type);
			values.Add(arg.value);
		}

		types.Add(typeof(R));
		var fnType = MakeDelegate(types.ToArray());
		var fn = Marshal.GetDelegateForFunctionPointer(fnPtr, fnType);
		return (R?) fn.DynamicInvoke(values.ToArray());
	}
}
