namespace CrossForms.Native.MacOS.Internals;


public class NsNotificationCenter: NsObject, IObjClass<NsNotificationCenter> {
	public new static readonly ObjClass<NsNotificationCenter> Proto = ObjClass<NsNotificationCenter>.Get("NSNotificationCenter");
	
	private static readonly IntPtr DefaultCenterSel = ObjSelector.Get("defaultCenter");
	private static readonly IntPtr RemoveObserverSel = ObjSelector.Get("removeObserver:");
	private static readonly IntPtr AddObserverSel = ObjSelector.Get("addObserver:selector:name:object:");
	
	public new static NsNotificationCenter Borrow (IntPtr ptr) => new(ptr);
	protected NsNotificationCenter (IntPtr ptr): base(ptr) {}

	public static NsNotificationCenter BorrowDefault () {
		return Borrow(ObjC.SendMessage(Proto.inner, DefaultCenterSel));
	}

	public void On (IntPtr observerPtr, IntPtr msgSel, NsString name, NsObject sender) {
		ObjC.SendMessage(inner, AddObserverSel, observerPtr, msgSel, name.inner, sender.inner);
	}

	public void Off (IntPtr observerPtr) {
		ObjC.SendMessage(inner, RemoveObserverSel, observerPtr);
	}
}
