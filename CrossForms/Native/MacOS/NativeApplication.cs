using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;

public class NativeApplication : IApplication {
    private static NsApplication? _app;

    public new static void Start () {
        _app = new NsApplication();
        _app.SetActivationPolicy(NsApplication.ActivationPolicy.Regular);
    }

    public new static bool EventLoop () {
        _app!.Run();
        return false;
    }

    public new static void Dispose () { }
}
