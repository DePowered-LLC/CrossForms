namespace CrossForms.Native.MacOS.Internals;


public class NsStatusItem: NsNested, IObjClass<NsStatusItem> {
	public new static readonly ObjClass<NsStatusItem> Proto = ObjClass<NsStatusItem>.Get("NSStatusItem");
	
	private static readonly IntPtr ButtonSel = ObjSelector.Get("button");
	private static readonly IntPtr SetMenuSel = ObjSelector.Get("setMenu:");
	private static readonly IntPtr RemoveStatusItemSel = ObjSelector.Get("removeStatusItem:");

	private static readonly IntPtr SetImageSel = ObjSelector.Get("setImage:");
	private static readonly IntPtr SetToolTipSel = ObjSelector.Get("setToolTip:");
	private static readonly IntPtr SetTargetSel = ObjSelector.Get("setTarget:");
	private static readonly IntPtr SetActionSel = ObjSelector.Get("setAction:");
	private static readonly IntPtr SendActionOnSel = ObjSelector.Get("sendActionOn:");

	// todo: enum https://github.com/phracker/MacOSX-SDKs/blob/master/MacOSX10.8.sdk/System/Library/Frameworks/AppKit.framework/Versions/C/Headers/NSEvent.h
	// NSEventMaskLeftMouseUp (1<<2=4) | NSEventMaskRightMouseUp (1<<4=16) = 20
	private const int ClickEventMask = 20;

	public static NsStatusItem CreateAuto () {
		return NsStatusBar.System.CreateItemAuto(NsStatusBar.SquareItemLength);
	}
	
	public new static NsStatusItem Borrow (IntPtr ptr) => new(ptr);
	protected NsStatusItem (IntPtr ptr): base(ptr) {}

	public NsStatusBarButton BorrowButton () {
		return NsStatusBarButton.Borrow(ObjC.SendMessage(inner, ButtonSel));
	}

	public void SetMenu (NsMenu? menu) {
		ObjC.SendMessage(inner, SetMenuSel, menu?.inner ?? IntPtr.Zero);
	}

	public void Remove () {
		ObjC.SendMessage(NsStatusBar.System.inner, RemoveStatusItemSel, inner);
	}

	public void SetupButton (string? iconPath, byte[]? iconData, string? tooltip, IntPtr target, IntPtr action) {
		var button = BorrowButton();
		
		if (iconData != null) {
			var nsIconImg = NsImage.CreateOwned(NsData.CreateAuto(iconData));
			nsIconImg.Template = true;
			ObjC.SendMessage(button.inner, SetImageSel, nsIconImg.inner);
			nsIconImg.Release();
		} else if (iconPath is { Length: > 0 }) {
			var nsIconPath = NsString.CloneOwned(iconPath);
			var nsIconImg = NsImage.CreateOwned(nsIconPath);
			nsIconPath.Release();
			
			nsIconImg.Template = true;
			ObjC.SendMessage(button.inner, SetImageSel, nsIconImg.inner);
			nsIconImg.Release();
		}
		
		if (tooltip is { Length: > 0 }) {
			var nsTooltip = NsString.CloneOwned(tooltip);
			ObjC.SendMessage(button.inner, SetToolTipSel, nsTooltip.inner);
			nsTooltip.Release();
		}

		ObjC.SendMessage(button.inner, SetTargetSel, target);
		ObjC.SendMessage(button.inner, SetActionSel, action);
		// capture both left and right mouse up events
		ObjC.SendMessage(button.inner, SendActionOnSel, ClickEventMask);
	}
}
