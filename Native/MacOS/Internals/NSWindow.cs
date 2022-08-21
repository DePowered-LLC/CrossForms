namespace CrossForms.Native.MacOS;

public class NSWindow: NSEventDispatcher {
	public enum StyleMask: long {
		Borderless = 0L,
		Titled = 1L,
		Closable = 2L,
		Miniaturizable = 4L,
		Resizable = 8L
	}

	private static ObjClass proto;
	static NSWindow () {
		var proto = ObjClass.TryGet("NSWindow");
		if (proto == null) throw new Exception("Class NSWindow not registered");
		else NSWindow.proto = proto;
	}

	public NSWindow (CGRect frame, StyleMask style, long backingStore, bool defer = false) {
		proto.Construct(this);
		ObjC.SendMessage(inner, "initWithContentRect:styleMask:backing:defer:", frame, (long) style, (long) backingStore, defer);
		ObjC.SendMessage(inner, "makeKeyAndOrderFront:", IntPtr.Zero);
	}

	public string title {
		get { return null; }
		set { ObjC.SendMessage(inner, "setTitle:", new NSString(value)); }
	}

	public NSView contentView => new NSView { inner = ObjC.SendMessage(inner, "contentView") };
	public void Append (NSControl child) {
		child.parent = this;
		contentView.AddSubview(child);
		child.OnAttach();
	}
}
