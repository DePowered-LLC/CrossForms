namespace CrossForms.Native.MacOS.Internals;

public class CocoaSynchronizationContext : SynchronizationContext {
    private readonly int _mainThreadId = Environment.CurrentManagedThreadId;

    public static void Install() {
        SetSynchronizationContext(new CocoaSynchronizationContext());
    }

    public override void Post(SendOrPostCallback d, object? state) {
        Dispatch.Post(() => d(state));
    }

    public override void Send(SendOrPostCallback d, object? state) {
        if (Environment.CurrentManagedThreadId == _mainThreadId) {
            d(state);
        } else {
            Dispatch.Send(() => d(state));
        }
    }

    public override SynchronizationContext CreateCopy() => new CocoaSynchronizationContext();
}
