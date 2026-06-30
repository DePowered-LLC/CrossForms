namespace CrossForms.Native.MacOS.Internals;


public class NsProgressIndicator: NsView, IObjClass<NsProgressIndicator> {
	public new static readonly ObjClass<NsProgressIndicator> Proto = ObjClass<NsProgressIndicator>.Get("NSProgressIndicator");
	
	private static readonly IntPtr InitSel = ObjSelector.Get("init");
	private static readonly IntPtr SetMinSel = ObjSelector.Get("setMinValue:");
	private static readonly IntPtr SetMaxSel = ObjSelector.Get("setMaxValue:");
	private static readonly IntPtr SetValueSel = ObjSelector.Get("setDoubleValue:");
	private static readonly IntPtr GetValueSel = ObjSelector.Get("doubleValue");
	private static readonly IntPtr SetIndeterminateSel = ObjSelector.Get("setIndeterminate:");
	private static readonly IntPtr StartAnimSel = ObjSelector.Get("startAnimation:");
	private static readonly IntPtr StopAnimSel = ObjSelector.Get("stopAnimation:");

	public static NsProgressIndicator CreateOwned () {
		var result = Proto.Allocate();
		result.inner = ObjC.SendMessage(result.inner, InitSel);
		
		result.TranslatesAutoresizingMaskIntoConstraints = false;
		return result;
	}
	
	public new static NsProgressIndicator Borrow (IntPtr ptr) => new(ptr);
	protected NsProgressIndicator (IntPtr ptr): base(ptr) {}

	public double MinValue {
		set => ObjC.SetDouble(inner, SetMinSel, value);
	}

	public double MaxValue {
		set => ObjC.SetDouble(inner, SetMaxSel, value);
	}

	public double Value {
		get => ObjC.SendMessage<double>(inner, GetValueSel);
		set => ObjC.SetDouble(inner, SetValueSel, value);
	}

	public bool Indeterminate {
		set {
			ObjC.SendMessage(inner, SetIndeterminateSel, value ? 1 : 0);
			ObjC.SendMessage(inner, value ? StartAnimSel : StopAnimSel, IntPtr.Zero);
		}
	}
}
