namespace CrossForms.Native.MacOS.Internals;


public class NsControl: NsView {
	private Dictionary<string, PreRegisteredEvent>? _preRegisteredEvents = new();

	public NsEventDispatcher GetEventDispatcher () {
		var nextParent = parent;
		while (nextParent != null) {
			if (nextParent is NsEventDispatcher dispatcher) return dispatcher;
			nextParent = nextParent.parent;
		}

		throw new Exception("Root NSEventDispatcher not provided!");
	}

	protected void PreRegisterEvent (
		string name, Action eventHandler,
		Action<NsEventDispatcher, IntPtr> registerHandler
	) {
		if (_preRegisteredEvents == null) {
			var dispatcher = GetEventDispatcher();
			var eventSel = dispatcher.AttachEvent(this, name, eventHandler);
			registerHandler(dispatcher, eventSel);
		} else {
			if (_preRegisteredEvents.TryGetValue(name, out var e)) {
				e.handler += eventHandler;
				e.register += registerHandler;
			} else {
				_preRegisteredEvents.Add(name, new PreRegisteredEvent {
					handler = eventHandler,
					register = registerHandler
				});
			}
		}
	}

	public void OnAttach () {
		if (_preRegisteredEvents == null) return;

		var dispatcher = GetEventDispatcher();
		foreach (var pair in _preRegisteredEvents) {
			var eventSel = dispatcher.AttachEvent(this, pair.Key, pair.Value.handler);
			pair.Value.register(dispatcher, eventSel);
		}

		_preRegisteredEvents = null;
	}

	private struct PreRegisteredEvent {
		public Action handler;
		public Action<NsEventDispatcher, IntPtr> register;
	}
}
