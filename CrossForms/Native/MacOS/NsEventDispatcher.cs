using System.Runtime.InteropServices;

using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public struct EventHandlers {
	public Dictionary<IntPtr, Action> map;
	public IntPtr selector;
}

public class NsEventDispatcher: NsNested, IObjClass<NsEventDispatcher> {
	public delegate void DispatchEventFn (IntPtr self, IntPtr sel, IntPtr source);

	public IntPtr dispatcherInstance;

	protected Dictionary<string, EventHandlers> events = new();

	private static int _classSeq;

	public new static NsEventDispatcher Borrow (IntPtr ptr) => new(ptr);
	protected NsEventDispatcher (IntPtr ptr): base(ptr) {}

	public ObjClass<NsEventDispatcher> DispatcherClass {
		get {
			if (field != null) {
				return field;
			}

			var seq = Interlocked.Increment(ref _classSeq);
			field = NsObject.Proto.NewSubClass<NsEventDispatcher>(GetType().Name + "_ED_" + seq);
			dispatcherInstance = ObjC.SendMessage(ObjC.SendMessage(field.inner, "alloc"), "init");
			return field;
		}
	}

	public IntPtr AttachEvent (NativeManaged<IntPtr> source, string name, Action handler) {
		if (events.TryGetValue(name, out var handlers)) {
			handlers.map[source.inner] = handler;
			return handlers.selector;
		}

		// v void (@ self, : sel, @ sender)
		var selector = DispatcherClass.AddMethod(name, (DispatchEventFn) DispatchEvent, "v@:@");
		events.Add(name, new EventHandlers {
			selector = selector,
			map = new Dictionary<IntPtr, Action> {
				{ source.inner, handler }
			}
		});

		return selector;
	}

	public void DispatchEvent (IntPtr self, IntPtr selector, IntPtr source) {
		var name = Marshal.PtrToStringAnsi(ObjSelector.GetName(selector));
		if (name == null) return;
		
		if (NsNotification.Proto.TryCast(source, out var notification)) {
			source = notification.Object;
		}

		if (!events.TryGetValue(name, out var handlers)) return;
		
		if (handlers.map.TryGetValue(source, out var handle)) {
			handle();
		}
	}
}
