namespace CrossForms.Native.MacOS.Internals;


public class NsControl: NsView, IObjClass<NsControl> {
	private static readonly IntPtr GetEnabledSel = ObjSelector.Get("isEnabled");
	private static readonly IntPtr SetEnabledSel = ObjSelector.Get("setEnabled:");
	
	public new static NsControl Borrow (IntPtr ptr) => new(ptr);
	protected NsControl (IntPtr ptr): base(ptr) {}

	public bool Enabled {
		get => ObjC.SendMessage(inner, GetEnabledSel) != IntPtr.Zero;
		set => ObjC.SendMessage(inner, SetEnabledSel, value ? 1 : 0);
	}

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
