using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

using CrossForms.Native.Common;

using DynAccess = System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute;

namespace CrossForms.Native.MacOS.Internals;


public partial class ObjClassAny: NativeManaged<IntPtr> {
	protected static readonly IntPtr ClassSel = ObjSelector.Get("class");
	protected static readonly IntPtr AllocSel = ObjSelector.Get("alloc");
	protected static readonly IntPtr IsKindOfSel = ObjSelector.Get("isKindOfClass:");
	
	// Delegates passed to the ObjC runtime must stay alive for the process lifetime
	protected static readonly List<Delegate> _pinnedDelegates = [];

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_getName", StringMarshalling = StringMarshalling.Utf8)]
	protected static partial IntPtr GetName (IntPtr handle);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_getClassList")]
	protected static partial int GetClassList (IntPtr buffer, int count);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_getClass", StringMarshalling = StringMarshalling.Utf8)]
	protected static partial IntPtr GetClass (string name);
	
	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_allocateClassPair", StringMarshalling = StringMarshalling.Utf8)]
	protected static partial IntPtr AllocateClassPair (IntPtr superClass, string name, nuint extraBytes);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_registerClassPair")]
	protected static partial void RegisterClassPair (IntPtr cls);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_isMetaClass")]
	protected static partial byte IsMetaClass (IntPtr cls);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "method_getName")]
	protected static partial IntPtr GetMethodName (IntPtr method);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_copyMethodList")]
	protected static unsafe partial IntPtr* CopyMethodList (IntPtr cls, ref uint count);
	
	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_addMethod", StringMarshalling = StringMarshalling.Utf8)]
	protected static partial byte AddClassMethod (IntPtr cls, IntPtr nameSel, IntPtr impl, string types);
	
	[LibraryImport(ObjC.CocoaPath, EntryPoint = "class_replaceMethod", StringMarshalling = StringMarshalling.Utf8)]
	protected static partial byte ReplaceClassMethod (IntPtr cls, IntPtr nameSel, IntPtr impl, string types);
	
	public static ObjClassAny[] GetRegistered () {
		var count = GetClassList(IntPtr.Zero, 0);
		return NativeUtils.ReadArray<IntPtr, ObjClassAny>(count, buffer => GetClassList(buffer, count));
	}

	public static ObjClassAny Of (IntPtr instance) {
		return new ObjClassAny { inner = ObjC.SendMessage(instance, ClassSel) };
	}
}

public interface IObjClass<out T> {
	public static abstract T Borrow (IntPtr ptr);
}

public class ObjClass<T>: ObjClassAny where T: NativeManaged<IntPtr>, IObjClass<T> {
	private const DynamicallyAccessedMemberTypes DynPubMethods = DynamicallyAccessedMemberTypes.PublicMethods;

	public string Name => Marshal.PtrToStringAnsi(GetName(inner))!;
	public bool IsMeta => IsMetaClass(inner) == 1;
	
	public static ObjClass<T>? TryGet (string name) {
		var handle = GetClass(name);
		return handle == IntPtr.Zero ? null : new ObjClass<T> { inner = handle };
	}

	public static ObjClass<T> Get (string name) {
		return TryGet(name) ?? throw new Exception($"Class {name} not registered");
	}


	public void Allocate (NativeManaged<IntPtr> obj) {
		obj.inner = ObjC.SendMessage(inner, AllocSel);
	}
	
	public T Allocate () {
		return T.Borrow(ObjC.SendMessage(inner, AllocSel));
	}

	public void Construct (NativeManaged<IntPtr> obj, IntPtr constructorSel) {
		obj.inner = ObjC.SendMessage(inner, constructorSel);
	}


	public ObjClass<TS> NewSubClass<TS> (string name) where TS: NativeManaged<IntPtr>, IObjClass<TS> {
		var ptr = AllocateClassPair(inner, name, 0);
		if (ptr == IntPtr.Zero) {
			throw new InvalidOperationException($"'{name}' already registered or superclass is nil");
		}
		
		var cls = new ObjClass<TS> { inner = ptr };
		RegisterClassPair(cls.inner);
		return cls;
	}

	public ObjClass<TS> NewSubClass<TS> (string name, Action<ObjClass<TS>> fillClass) where TS: NativeManaged<IntPtr>, IObjClass<TS> {
		return NewSubClass(inner, name, fillClass);
	}

	private static ObjClass<TS> NewSubClass<TS> (IntPtr inner, string name, Action<ObjClass<TS>> fillClass) where TS: NativeManaged<IntPtr>, IObjClass<TS> {
		var cls = new ObjClass<TS> { inner = AllocateClassPair(inner, name, 0) };
		fillClass(cls);
		RegisterClassPair(cls.inner);
		return cls;
	}

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
	

	public IntPtr AddMethod<[DynAccess(DynPubMethods)] TD> (string name, TD fn) where TD: Delegate {
		var fnType = typeof(TD).GetMethod("Invoke");
		var types = new StringBuilder();

		// todo: оптимизировать создание метода
		ObjType.Append(types, fnType!.ReturnType);
		foreach (var argType in fnType.GetParameters()) {
			ObjType.Append(types, argType.ParameterType);
		}

		_pinnedDelegates.Add(fn);
		var sel = ObjSelector.Register(name);
		AddClassMethod(inner, sel, Marshal.GetFunctionPointerForDelegate(fn), types.ToString());
		return sel;
	}

	public IntPtr AddMethod<TD> (string name, TD fn, string types) where TD: Delegate {
		_pinnedDelegates.Add(fn);
		
		var sel = ObjSelector.Register(name);
		AddClassMethod(inner, sel, Marshal.GetFunctionPointerForDelegate(fn), types);
		return sel;
	}

	public IntPtr ReplaceMethod<TD> (string name, TD fn, string types) where TD: Delegate {
		var sel = ObjSelector.Register(name);
		ReplaceClassMethod(inner, sel, Marshal.GetFunctionPointerForDelegate(fn), types);
		return sel;
	}

	public bool IsInstance (IntPtr mayBeInstance) {
		return ObjC.SendMessage(mayBeInstance, IsKindOfSel, inner) != IntPtr.Zero;
	}

	public bool TryCast (IntPtr ptr, out T result) {
		result = T.Borrow(IntPtr.Zero);
		if (!IsInstance(ptr)) return false;
		
		result.inner = ptr;
		return true;
	}
}
