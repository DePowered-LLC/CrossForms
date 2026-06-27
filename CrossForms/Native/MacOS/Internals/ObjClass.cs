using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

using CrossForms.Native.Common;

using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;

namespace CrossForms.Native.MacOS.Internals;


internal partial class ObjClass: NativeManaged<IntPtr> {
	private const DynamicallyAccessedMemberTypes DynPubMethods = DynamicallyAccessedMemberTypes.PublicMethods;

	private static readonly IntPtr ClassSel = ObjSelector.Get("class");

	private static readonly IntPtr AllocSel = ObjSelector.Get("alloc");

	private static readonly IntPtr IsKindOfSel = ObjSelector.Get("isKindOfClass:");
	public string Name => Marshal.PtrToStringAnsi(GetName(inner))!;
	public bool IsMeta => IsMetaClass(inner) == 1;

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_getName", StringMarshalling = StringMarshalling.Utf8)]
	private static partial IntPtr GetName (IntPtr handle);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_getClassList")]
	private static partial int GetClassList (IntPtr buffer, int count);

	public static ObjClass[] GetRegistered () {
		var count = GetClassList(IntPtr.Zero, 0);
		return NativeUtils.ReadArray<IntPtr, ObjClass>(count, buffer => GetClassList(buffer, count));
	}

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_getClass", StringMarshalling = StringMarshalling.Utf8)]
	private static partial IntPtr GetClass (string name);

	public static ObjClass? TryGet (string name) {
		var handle = GetClass(name);
		return handle == IntPtr.Zero ? null : new ObjClass { inner = handle };
	}

	public static ObjClass Get (string name) {
		var proto = TryGet(name);
		if (proto == null) throw new Exception($"Class {name} not registered");
		return proto;
	}

	public static ObjClass Of (IntPtr instance) {
		return new ObjClass { inner = ObjC.SendMessage(instance, ClassSel) };
	}

	public void Construct (NativeManaged<IntPtr> obj) {
		obj.inner = ObjC.SendMessage(inner, AllocSel);
	}

	public void Construct (NativeManaged<IntPtr> obj, IntPtr constructorSel) {
		obj.inner = ObjC.SendMessage(inner, constructorSel);
	}

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_allocateClassPair", StringMarshalling = StringMarshalling.Utf8)]
	private static partial IntPtr AllocateClassPair (IntPtr superClass, string name, nuint extraBytes);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_registerClassPair")]
	private static partial void RegisterClassPair (IntPtr cls);

	public ObjClass NewSubClass (string name) {
		var cls = new ObjClass { inner = AllocateClassPair(inner, name, 0) };
		RegisterClassPair(cls.inner);
		return cls;
	}

	public ObjClass NewSubClass (string name, Action<ObjClass> fillClass) {
		return NewSubClass(inner, name, fillClass);
	}

	public static ObjClass NewSubClass (IntPtr inner, string name, Action<ObjClass> fillClass) {
		var cls = new ObjClass { inner = AllocateClassPair(inner, name, 0) };
		fillClass(cls);
		RegisterClassPair(cls.inner);
		return cls;
	}

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_isMetaClass")]
	private static partial byte IsMetaClass (IntPtr cls);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "method_getName")]
	private static partial IntPtr GetMethodName (IntPtr method);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_copyMethodList")]
	private static unsafe partial IntPtr* CopyMethodList (IntPtr cls, ref uint count);

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


	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_addMethod", StringMarshalling = StringMarshalling.Utf8)]
	private static partial byte AddClassMethod (IntPtr cls, IntPtr nameSel, IntPtr impl, string types);

	public IntPtr AddMethod<[DynAccess(DynPubMethods)] D> (string name, D fn) where D: Delegate {
		var fnType = typeof(D).GetMethod("Invoke");
		var types = new StringBuilder();

		// todo: оптимизировать создание метода
		ObjType.Append(types, fnType!.ReturnType);
		foreach (var argType in fnType.GetParameters()) {
			ObjType.Append(types, argType.ParameterType);
		}

		var sel = ObjSelector.Register(name);
		AddClassMethod(inner, sel, Marshal.GetFunctionPointerForDelegate(fn), types.ToString());
		return sel;
	}

	public IntPtr AddMethod<TD> (string name, TD fn, string types) where TD: Delegate {
		var sel = ObjSelector.Register(name);
		AddClassMethod(inner, sel, Marshal.GetFunctionPointerForDelegate(fn), types);
		return sel;
	}

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_replaceMethod", StringMarshalling = StringMarshalling.Utf8)]
	private static partial byte ReplaceClassMethod (IntPtr cls, IntPtr nameSel, IntPtr impl, string types);

	public IntPtr ReplaceMethod<TD> (string name, TD fn, string types) where TD: Delegate {
		var sel = ObjSelector.Register(name);
		ReplaceClassMethod(inner, sel, Marshal.GetFunctionPointerForDelegate(fn), types);
		return sel;
	}

	public bool IsInstance (IntPtr mayBeInstance) {
		return ObjC.SendMessage(mayBeInstance, IsKindOfSel, inner) != IntPtr.Zero;
	}
}
