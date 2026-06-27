using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


public partial class NsProgressIndicator: NsView {
	private static readonly ObjClass Proto = ObjClass.Get("NSProgressIndicator");
	private static readonly IntPtr AllocSel = ObjSelector.Get("alloc");
	private static readonly IntPtr InitSel = ObjSelector.Get("init");
	private static readonly IntPtr SetMinSel = ObjSelector.Get("setMinValue:");
	private static readonly IntPtr SetMaxSel = ObjSelector.Get("setMaxValue:");
	private static readonly IntPtr SetValueSel = ObjSelector.Get("setDoubleValue:");
	private static readonly IntPtr GetValueSel = ObjSelector.Get("doubleValue");
	private static readonly IntPtr SetIndeterminateSel = ObjSelector.Get("setIndeterminate:");
	private static readonly IntPtr StartAnimSel = ObjSelector.Get("startAnimation:");
	private static readonly IntPtr StopAnimSel = ObjSelector.Get("stopAnimation:");

	public NsProgressIndicator () {
		var alloc = ObjC.SendMessage(Proto.inner, AllocSel);
		inner = ObjC.SendMessage(alloc, InitSel);
		TranslatesAutoresizingMaskIntoConstraints = false;
	}

	public double MinValue {
		set => SendDouble(inner, SetMinSel, value);
	}

	public double MaxValue {
		set => SendDouble(inner, SetMaxSel, value);
	}

	public double Value {
		get => GetDouble(inner, GetValueSel);
		set => SendDouble(inner, SetValueSel, value);
	}

	public bool Indeterminate {
		set {
			ObjC.SendMessage(inner, SetIndeterminateSel, value ? 1 : 0);
			ObjC.SendMessage(inner, value ? StartAnimSel : StopAnimSel, IntPtr.Zero);
		}
	}

	// arm64: objc_msgSend handles all types, so no _fpret variant needed
	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial void SendDouble (IntPtr obj, IntPtr sel, double value);

	[LibraryImport(ObjC.CocoaPath, EntryPoint = "objc_msgSend")]
	private static partial double GetDouble (IntPtr obj, IntPtr sel);
}
