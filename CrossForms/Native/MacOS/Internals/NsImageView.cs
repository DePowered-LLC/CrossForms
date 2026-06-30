namespace CrossForms.Native.MacOS.Internals;


public class NsImageView: NsView, IObjClass<NsImageView> {
	private new static readonly ObjClass<NsImageView> Proto = ObjClass<NsImageView>.Get("NSImageView");
	
	private static readonly IntPtr InitSel = ObjSelector.Get("init");
	private static readonly IntPtr GetImageSel = ObjSelector.Get("image");
	private static readonly IntPtr SetImageSel = ObjSelector.Get("setImage:");
	private static readonly IntPtr SetScalingSel = ObjSelector.Get("setImageScaling:");

	public static NsImageView CreateOwned () {
		var view = Proto.Allocate();
		view.inner = ObjC.SendMessage(view.inner, InitSel);
		
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		// todo: enum, property
		ObjC.SendMessage(view.inner, SetScalingSel, 3); // NSImageScaleProportionallyUpOrDown
		return view;
	}

	public new static NsImageView Borrow (IntPtr ptr) => new(ptr);
	protected NsImageView (IntPtr ptr): base(ptr) {}

	public NsImage? Image {
		get {
			var ptr = ObjC.SendMessage(inner, GetImageSel);
			return ptr == IntPtr.Zero ? null : NsImage.Borrow(ptr);
		}
		set => ObjC.SendMessage(inner, SetImageSel, value?.inner ?? IntPtr.Zero);
	}
}
