using System.Runtime.InteropServices;
using CrossForms.Native.Common;

namespace CrossForms.Native.MacOS;

public struct EventHandlers {
	public Dictionary<IntPtr, Action> map;
	public IntPtr selector;
}

public class NSEventDispatcher: NSNested {
	private ObjClass _dispatcherClass;
	public IntPtr dispatcherInstance;
	internal ObjClass dispatcherClass {
		get {
			if (_dispatcherClass == null) {
				_dispatcherClass = NSObject.proto.NewSubClass(GetType().Name + "_ED");
				dispatcherInstance = ObjC.SendMessage(_dispatcherClass.inner, "alloc");
			}

			return _dispatcherClass;
		}
	}

	protected Dictionary<string, EventHandlers> events = new();
	public IntPtr AttachEvent (NativeManaged<IntPtr> source, string name, Action handler) {
		if (events.TryGetValue(name, out EventHandlers handlers)) {
			handlers.map.Add(source.inner, handler);
			return handlers.selector;
		} else {
			// v void (@ self, : sel, @ sender)
			var selector = dispatcherClass.AddMethod(name, (DispatchEventFn) DispatchEvent, "v@:@");
			events.Add(name, new EventHandlers {
				selector = selector,
				map = new Dictionary<IntPtr, Action> {
					{ source.inner, handler }
				}
			});

			return selector;
		}
	}

	public delegate void DispatchEventFn (IntPtr self, IntPtr sel, IntPtr source);
	public void DispatchEvent (IntPtr _self, IntPtr selector, IntPtr source) {
		var name = Marshal.PtrToStringAnsi(ObjSelector.GetName(selector));

		var isNotification = NSNotification.proto.IsInstance(source);
		if (isNotification) {
			var notification = new NSNotification { inner = source };
			source = notification.Object;
		}

		if (events.TryGetValue(name, out EventHandlers handlers)) {
			if (handlers.map.TryGetValue(source, out Action handle)) {
				handle();
			}
		}
	}
}
