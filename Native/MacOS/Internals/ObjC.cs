using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

internal partial class ObjC {
	// TODO: подумать о надобности этого хука, т.к. static { ... } выполняется в правильно порядке, если вызывать InitRuntime сразу
	private static Action? onRuntimeReady = () => {};
	public static void RuntimeReady (Action callback) {
		if (onRuntimeReady == null) callback();
		else onRuntimeReady += callback;
	}

	[DllImport("objc_helper.dylib", EntryPoint = "initRuntime")]
	private static extern IntPtr _InitRuntime ();
	public static void InitRuntime () {
		if (onRuntimeReady == null) return;

		var result = new NativeStatus(_InitRuntime());
		if (!result.success) throw new Exception(result.error);

		onRuntimeReady();
		onRuntimeReady = null;

		msgSendPtr = GetFunction("objc_msgSend");
		SendMessageStatic = GetFunction<MsgSendFn>("objc_msgSend");
	}

	[DllImport("objc_helper.dylib", EntryPoint = "getFunction", CharSet = CharSet.Ansi)]
	public static extern IntPtr GetFunction (string symbol);
	public static T GetFunction<T> (string symbol) => Marshal.GetDelegateForFunctionPointer<T>(GetFunction(symbol));


	private static IntPtr msgSendPtr;
	private static Type[] msgSendTypes = new Type[] { typeof(IntPtr), typeof(IntPtr) };

	public static IntPtr SendMessage (IntPtr receiver, IntPtr selector, params NativeArg[] args) {
		var types = new List<Type>(msgSendTypes);
		return NativeFn.Apply<IntPtr>(msgSendPtr, types, new List<object> { receiver, selector }, args);
	}

	public static IntPtr SendMessage (IntPtr receiver, string selector, params NativeArg[] args) {
		return SendMessage(receiver, ObjSelector.Get(selector), args);
	}

	private delegate IntPtr MsgSendFn (IntPtr cls, IntPtr selector);
	private static MsgSendFn SendMessageStatic;

	public static IntPtr SendMessage (IntPtr receiver, string selector) {
		return SendMessageStatic(receiver, ObjSelector.Get(selector));
	}

	public static IntPtr SendMessage (IntPtr receiver, IntPtr selector) {
		return SendMessageStatic(receiver, selector);
	}
}
