using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS.Internals;

public class NsEvent : NativeManaged<IntPtr> {
    private static readonly IntPtr LOCATION_IN_WINDOW = ObjSelector.Get("locationInWindow");
    public CGPoint LocationInWindow => ObjC.SendMessage<CGPoint>(inner, LOCATION_IN_WINDOW);
}
