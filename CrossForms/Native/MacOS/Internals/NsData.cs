using System.Runtime.InteropServices;

namespace CrossForms.Native.MacOS.Internals;


public class NsData: NsObject, IObjClass<NsData> {
	private new static readonly ObjClass<NsData> Proto = ObjClass<NsData>.Get("NSData");
	
	private static readonly IntPtr DataWithBytesSel = ObjSelector.Get("dataWithBytes:length:");

	public static NsData CreateAuto (byte[] data) {
		var dataPin = GCHandle.Alloc(data, GCHandleType.Pinned);
		var dataPtr = dataPin.AddrOfPinnedObject();

		try {
			return new NsData(ObjC.SendMessage(Proto.inner, DataWithBytesSel, dataPtr, (nint) data.Length));
		} finally {
			dataPin.Free();
		}
	}
	
	public new static NsData Borrow (IntPtr ptr) => new(ptr);
	protected NsData (IntPtr ptr): base(ptr) {}
}
