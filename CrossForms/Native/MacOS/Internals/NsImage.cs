namespace CrossForms.Native.MacOS.Internals;


public class NsImage: NsObject, IObjClass<NsImage> {
	private new static readonly ObjClass<NsImage> Proto = ObjClass<NsImage>.Get("NSImage");
	
	private static readonly IntPtr InitWithFileSel = ObjSelector.Get("initWithContentsOfFile:");
	private static readonly IntPtr InitWithDataSel = ObjSelector.Get("initWithData:");
	private static readonly IntPtr GetTemplateSel = ObjSelector.Get("isTemplate");
	private static readonly IntPtr SetTemplateSel = ObjSelector.Get("setTemplate:");

	public static NsImage CreateOwned (NsString path) {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, InitWithFileSel, path.inner);
		return result;
	}

	public static NsImage CreateOwned (NsData data) {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, InitWithDataSel, data.inner);
		return result;
	}

	public new static NsImage Borrow (IntPtr ptr) => new(ptr);
	protected NsImage (IntPtr ptr): base(ptr) {}

	public bool Template {
		get => ObjC.SendMessage(inner, GetTemplateSel) != 0;
		set => ObjC.SendMessage(inner, SetTemplateSel, value ? 1 : 0);
	}
}
