using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;


public class NsApplication: NsObject, IObjClass<NsApplication> {
	public enum ActivationPolicy {
		Regular,
		Accessory,
		Prohibited
	}

	private new static readonly ObjClass<NsApplication> Proto = ObjClass<NsApplication>.Get("NSApplication");

	public static NsApplication Current {
		get {
			field ??= BorrowShared();
			return field;
		}
		set;
	}
	
	private static readonly IntPtr SharedAppSel = ObjSelector.Get("sharedApplication");

	private static readonly IntPtr TerminateSel = ObjSelector.Get("terminate:");

	private static readonly IntPtr CurrentEventSel = ObjSelector.Get("currentEvent");

	private readonly IntPtr _activateIgnoringOtherAppsSel = ObjSelector.Get("activateIgnoringOtherApps:");

	private readonly IntPtr _setActivationPolicySel = ObjSelector.Get("setActivationPolicy:");
	private readonly IntPtr _setDelegateSel = ObjSelector.Get("setDelegate:");

	public static NsApplication BorrowShared () {
		return Borrow(ObjC.SendMessage(Proto.inner, SharedAppSel));
	}

	public new static NsApplication Borrow (IntPtr ptr) => new(ptr);
	protected NsApplication (IntPtr ptr): base(ptr) {}

	public bool IsRunning => ObjC.SendMessage(inner, ObjSelector.Get("isRunning")) != IntPtr.Zero;

	internal AppDelegate AppDelegate {
		get;
		set {
			ObjC.SendMessage(inner, _setDelegateSel, value.inner);
			field = value;
		}
	} = AppDelegate.CreateOwned();

	public NsEvent? BorrowCurrentEvent () {
		var ptr = ObjC.SendMessage(inner, CurrentEventSel);
		return ptr == IntPtr.Zero ? null : NsEvent.Borrow(ptr);
	}

	public void SetActivationPolicy (ActivationPolicy value) {
		ObjC.SendMessage(inner, _setActivationPolicySel, (int) value);
	}

	public void Activate () {
		ObjC.SendMessage(inner, _activateIgnoringOtherAppsSel, 1);
	}

	public void SetMainMenu (NsMenu menu) {
		ObjC.SendMessage(inner, ObjSelector.Get("setMainMenu:"), menu.inner);
	}

	public void Run () {
		CocoaSynchronizationContext.Install();
		ObjC.SendMessage(inner, _activateIgnoringOtherAppsSel, 1);
		ObjC.SendMessage(inner, "run");
	}

	public void Terminate () {
		ObjC.SendMessage(inner, TerminateSel, IntPtr.Zero);
	}
}
