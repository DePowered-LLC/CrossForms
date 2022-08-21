using System.Runtime.InteropServices;
using System.Text;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

public class ObjClass: NativeManaged<IntPtr> {
	private delegate IntPtr GetClassNameFn (IntPtr handle);
	private static readonly GetClassNameFn GetName;
	public string Name => Marshal.PtrToStringAnsi(GetName(inner));

	private delegate int GetClassListFn (IntPtr buffer, int count);
	private static readonly GetClassListFn GetClassList;
	public static ObjClass[] GetRegistered () {
		var count = GetClassList(IntPtr.Zero, 0);
		return NativeUtils.ReadArray<IntPtr, ObjClass>(count, buffer => GetClassList(buffer, count));
	}

	private delegate IntPtr GetClassFn (string name);
	private static readonly GetClassFn GetClass;
	public static ObjClass? TryGet (string name) {
		var handle = GetClass(name);

		if (handle == IntPtr.Zero) return null;
		else return new ObjClass { inner = handle };
	}
	public static ObjClass Get (string name) {
		var proto = TryGet(name);
		if (proto == null) throw new Exception($"Class {name} not registered");
		else return proto;
	}


	public IntPtr CallRaw (string selector) => ObjC.SendMessage(inner, selector);
	public IntPtr CallRaw (string selector, params NativeArg[] args) => ObjC.SendMessage(inner, selector, args);
	public IntPtr CallRaw (IntPtr selector, params NativeArg[] args) => ObjC.SendMessage(inner, selector, args);


	private static readonly IntPtr ALLOC_SEL;
	public void Construct (NativeManaged<IntPtr> obj) {
		obj.inner = CallRaw(ALLOC_SEL);
	}

	public void Construct (NativeManaged<IntPtr> obj, string selector, params NativeArg[] args) {
		obj.inner = CallRaw(selector, args);
	}

	private delegate IntPtr AllocateClassPairFn (IntPtr superClass, string name, nuint extraBytes);
	private static readonly AllocateClassPairFn AllocateClassPair;
	private delegate void RegisterClassPairFn (IntPtr cls);
	private static readonly RegisterClassPairFn RegisterClassPair;

	public ObjClass NewSubClass (string name, Action<ObjClass> fillClass) => NewSubClass(inner, name, fillClass);
	public static ObjClass NewSubClass (IntPtr inner, string name, Action<ObjClass> fillClass) {
		var cls = new ObjClass { inner = AllocateClassPair(inner, name, 0) };
		fillClass(cls);
		RegisterClassPair(cls.inner);
		return cls;
	}

	public ObjClass NewSubClass (string name) => NewSubClass(inner, name);
	public static ObjClass NewSubClass (IntPtr inner, string name) {
		var cls = new ObjClass { inner = AllocateClassPair(inner, name, 0) };
		RegisterClassPair(cls.inner);
		return cls;
	}

	private delegate bool IsMetaClassFn (IntPtr cls);
	private static readonly IsMetaClassFn IsMetaClass;
	public bool isMeta => IsMetaClass(inner);

	private delegate IntPtr GetMethodNameFn (IntPtr method);
	private static readonly GetMethodNameFn GetMethodName;
	private unsafe delegate IntPtr* CopyMethodListFn (IntPtr cls, ref uint count);
	private static readonly CopyMethodListFn CopyMethodList;
	public string[] GetMethods () {
		unsafe {
			uint count = 0;
			var list = CopyMethodList(inner, ref count);

			var result = new string[count];
			for (uint i = 0; i < count; i++) {
				result[i] = Marshal.PtrToStringAnsi(GetMethodName(list[i]));
			}

			Marshal.FreeHGlobal((IntPtr) list);
			return result;
		}
	}


	private delegate bool AddClassMethodFn (IntPtr cls, IntPtr nameSel, IntPtr impl, string types);
	private static readonly AddClassMethodFn AddClassMethod;
	public IntPtr AddMethod<D> (string name, Delegate fn) where D: Delegate {
		var fnType = typeof(D).GetMethod("Invoke");
		var types = new StringBuilder();

		// todo: оптимизировать создание метода
		ObjType.Append(types, fnType.ReturnType);
		foreach (var argType in fnType.GetParameters()) {
			ObjType.Append(types, argType.ParameterType);
		}

		var sel = ObjSelector.Register(name);
		AddClassMethod(inner, sel, Marshal.GetFunctionPointerForDelegate(fn), types.ToString());
		return sel;
	}
	public IntPtr AddMethod (string name, Delegate fn, string types) {
		var sel = ObjSelector.Register(name);
		AddClassMethod(inner, sel, Marshal.GetFunctionPointerForDelegate(fn), types);
		return sel;
	}


	static ObjClass () {
		ALLOC_SEL = ObjSelector.Get("alloc");

		GetName = ObjC.GetFunction<GetClassNameFn>("class_getName");
		GetClassList = ObjC.GetFunction<GetClassListFn>("objc_getClassList");
		AllocateClassPair = ObjC.GetFunction<AllocateClassPairFn>("objc_allocateClassPair");
		RegisterClassPair = ObjC.GetFunction<RegisterClassPairFn>("objc_registerClassPair");
		AddClassMethod = ObjC.GetFunction<AddClassMethodFn>("class_addMethod");
		IsMetaClass = ObjC.GetFunction<IsMetaClassFn>("class_isMetaClass");
		CopyMethodList = ObjC.GetFunction<CopyMethodListFn>("class_copyMethodList");
		GetMethodName = ObjC.GetFunction<GetMethodNameFn>("method_getName");
		GetClass = ObjC.GetFunction<GetClassFn>("objc_getClass");
	}
}
