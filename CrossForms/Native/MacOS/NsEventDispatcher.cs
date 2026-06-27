using System.Runtime.InteropServices;

using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public struct EventHandlers {
	public Dictionary<IntPtr, Action> map;
	public IntPtr selector;
}

public class NsEventDispatcher: NsNested {
	public delegate void DispatchEventFn (IntPtr self, IntPtr sel, IntPtr source);

	public IntPtr dispatcherInstance;

	protected Dictionary<string, EventHandlers> events = new();

	internal ObjClass DispatcherClass {
		get {
			if (field != null) {
				return field;
			}

			field = NsObject.Proto.NewSubClass(GetType().Name + "_ED");
			dispatcherInstance = ObjC.SendMessage(field.inner, "alloc");
			return field;
		}
	}

	public IntPtr AttachEvent (NativeManaged<IntPtr> source, string name, Action handler) {
		if (events.TryGetValue(name, out var handlers)) {
			handlers.map.Add(source.inner, handler);
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

		var isNotification = NsNotification.Proto.IsInstance(source);
		if (isNotification) {
			var notification = new NsNotification { inner = source };
			source = notification.Object;
		}

		if (!events.TryGetValue(name, out var handlers)) return;
		
		if (handlers.map.TryGetValue(source, out var handle)) {
			handle();
		}
	}
}
