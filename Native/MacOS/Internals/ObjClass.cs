using System.Runtime.InteropServices;
using System.Text;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

internal class ObjClass: NativeManaged<IntPtr> {
	[DllImport(ObjC.COCOA, EntryPoint = "class_getName", CharSet = CharSet.Ansi)]
	private static extern IntPtr GetName (IntPtr handle);
	public string Name => Marshal.PtrToStringAnsi(GetName(inner));

	[DllImport(ObjC.COCOA, EntryPoint = "objc_getClassList")]
	private static extern int GetClassList (IntPtr buffer, int count);
	public static ObjClass[] GetRegistered () {
		var count = GetClassList(IntPtr.Zero, 0);
		return NativeUtils.ReadArray<IntPtr, ObjClass>(count, buffer => GetClassList(buffer, count));
	}

	[DllImport(ObjC.COCOA, EntryPoint = "objc_getClass", CharSet = CharSet.Ansi)]
	private static extern IntPtr GetClass (string name);
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

	private static readonly IntPtr CLASS = ObjSelector.Get("class");
	public static ObjClass Of (IntPtr instance) {
		return new ObjClass { inner = ObjC.SendMessage(instance, CLASS) };
	}


	private static readonly IntPtr ALLOC_SEL = ObjSelector.Get("alloc");
	public void Construct (NativeManaged<IntPtr> obj) {
		obj.inner = ObjC.SendMessage(inner, ALLOC_SEL);
	}
	public void Construct (NativeManaged<IntPtr> obj, IntPtr constructorSel) {
		obj.inner = ObjC.SendMessage(inner, constructorSel);
	}

	[DllImport(ObjC.COCOA, EntryPoint = "objc_allocateClassPair", CharSet = CharSet.Ansi)]
	private static extern IntPtr AllocateClassPair (IntPtr superClass, string name, nuint extraBytes);

	[DllImport(ObjC.COCOA, EntryPoint = "objc_registerClassPair")]
	private static extern void RegisterClassPair (IntPtr cls);

	public ObjClass NewSubClass (string name) {
		var cls = new ObjClass { inner = AllocateClassPair(inner, name, 0) };
		RegisterClassPair(cls.inner);
		return cls;
	}

	public ObjClass NewSubClass (string name, Action<ObjClass> fillClass) => NewSubClass(inner, name, fillClass);
	public static ObjClass NewSubClass (IntPtr inner, string name, Action<ObjClass> fillClass) {
		var cls = new ObjClass { inner = AllocateClassPair(inner, name, 0) };
		fillClass(cls);
		RegisterClassPair(cls.inner);
		return cls;
	}

	[DllImport(ObjC.COCOA, EntryPoint = "class_isMetaClass")]
	private static extern bool IsMetaClass (IntPtr cls);
	public bool isMeta => IsMetaClass(inner);

	[DllImport(ObjC.COCOA, EntryPoint = "method_getName")]
	private static extern IntPtr GetMethodName (IntPtr method);

	[DllImport(ObjC.COCOA, EntryPoint = "class_copyMethodList")]
	private static extern unsafe IntPtr* CopyMethodList (IntPtr cls, ref uint count);
	public string[] GetMethods () {
		unsafe {
			uint count = 0;
			var list = CopyMethodList(inner, ref count);

			var result = new string[count];
			for (uint i = 0; i < count; i++) {
				result[i] = Marshal.PtrToStringAnsi(GetMethodName(list[i]))!;
			}

			Marshal.FreeHGlobal((IntPtr) list);
			return result;
		}
	}


	[DllImport(ObjC.COCOA, EntryPoint = "class_addMethod", CharSet = CharSet.Ansi)]
	private static extern bool AddClassMethod (IntPtr cls, IntPtr nameSel, IntPtr impl, string types);
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

	[DllImport(ObjC.COCOA, EntryPoint = "class_replaceMethod", CharSet = CharSet.Ansi)]
	private static extern bool ReplaceClassMethod (IntPtr cls, IntPtr nameSel, IntPtr impl, string types);
	public IntPtr ReplaceMethod (string name, Delegate fn, string types) {
		var sel = ObjSelector.Register(name);
		ReplaceClassMethod(inner, sel, Marshal.GetFunctionPointerForDelegate(fn), types);
		return sel;
	}

	private static readonly IntPtr IS_KIND_OF = ObjSelector.Get("isKindOfClass:");
	public bool IsInstance (IntPtr mayBeInstance) {
		return ObjC.SendMessage(mayBeInstance, IS_KIND_OF, inner) != IntPtr.Zero;
	}
}
