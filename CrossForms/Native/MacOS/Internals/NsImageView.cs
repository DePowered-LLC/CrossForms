using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


public class NsImageView: NsView {
	private static readonly ObjClass Proto = ObjClass.Get("NSImageView");
	private static readonly ObjClass ImageProto = ObjClass.Get("NSImage");
	private static readonly ObjClass DataProto = ObjClass.Get("NSData");
	private static readonly IntPtr AllocSel = ObjSelector.Get("alloc");
	private static readonly IntPtr InitSel = ObjSelector.Get("init");
	private static readonly IntPtr InitWithFileSel = ObjSelector.Get("initWithContentsOfFile:");
	private static readonly IntPtr InitWithDataSel = ObjSelector.Get("initWithData:");
	private static readonly IntPtr DataWithBytesSel = ObjSelector.Get("dataWithBytes:length:");
	private static readonly IntPtr SetImageSel = ObjSelector.Get("setImage:");
	private static readonly IntPtr SetScalingSel = ObjSelector.Get("setImageScaling:");

	public NsImageView () {
		var alloc = ObjC.SendMessage(Proto.inner, AllocSel);
		inner = ObjC.SendMessage(alloc, InitSel);
		TranslatesAutoresizingMaskIntoConstraints = false;
		ObjC.SendMessage(inner, SetScalingSel, 3); // NSImageScaleProportionallyUpOrDown
	}

	public string ImagePath {
		set {
			var alloc = ObjC.SendMessage(ImageProto.inner, AllocSel);
			var nsImg = ObjC.SendMessage(alloc, InitWithFileSel, new NsString(value).inner);
			ObjC.SendMessage(inner, SetImageSel, nsImg);
		}
	}

	public byte[] ImageData {
		set {
			var pin = GCHandle.Alloc(value, GCHandleType.Pinned);
			try {
				var nsData = ObjC.SendMessage(DataProto.inner, DataWithBytesSel, pin.AddrOfPinnedObject(), value.Length);
				var alloc = ObjC.SendMessage(ImageProto.inner, AllocSel);
				var nsImg = ObjC.SendMessage(alloc, InitWithDataSel, nsData);
				ObjC.SendMessage(inner, SetImageSel, nsImg);
			} finally {
				pin.Free();
			}
		}
	}
}
