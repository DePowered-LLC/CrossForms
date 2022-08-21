namespace CrossForms.Native.MacOS;

public class NSControl: NSView {
	public NSEventDispatcher GetEventDispatcher () {
		NSNested nextParent = parent;
		while (nextParent != null)  {
			if (nextParent is NSEventDispatcher dispatcher) return dispatcher;
			else nextParent = nextParent.parent;
		}

		throw new Exception("Root NSEventDispatcher not provided!");
	}

	private struct PreRegisteredEvent {
		public Action handler;
		public Action<NSEventDispatcher, IntPtr> register;
	}

	private Dictionary<string, PreRegisteredEvent>? preRegisteredEvents = new();
	protected void PreRegisterEvent (string name, Action eventHandler, Action<NSEventDispatcher, IntPtr> registerHandler) {
		if (preRegisteredEvents == null) {
			var dispatcher = GetEventDispatcher();
			var eventSel = dispatcher.AttachEvent(this, name, eventHandler);
			registerHandler(dispatcher, eventSel);
		} else {
			if (preRegisteredEvents.TryGetValue(name, out PreRegisteredEvent e)) {
				e.handler += eventHandler;
				e.register += registerHandler;
			} else {
				preRegisteredEvents.Add(name, new PreRegisteredEvent {
					handler = eventHandler,
					register = registerHandler
				});
			}
		}
	}

	public void OnAttach () {
		if (preRegisteredEvents == null) return;

		var dispatcher = GetEventDispatcher();
		foreach (var pair in preRegisteredEvents) {
			var eventSel = dispatcher.AttachEvent(this, pair.Key, pair.Value.handler);
			pair.Value.register(dispatcher, eventSel);
		}

		preRegisteredEvents = null;
	}
}
