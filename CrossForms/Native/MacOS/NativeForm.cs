using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeForm: IForm {
	private readonly List<INativeAttachable> _children = [];
	private readonly List<INativeFocusable> _focusOrder = [];
	private readonly HashSet<NativeRadioGroup> _radioGroups = [];
	private INativeFocusable? _initialFocusable;

	private NsWindow? _window;
	public string Id { get; set; } = "";
	public string Title { get; set; } = "";
	public ushort Width { get; set; }
	public ushort Height { get; set; }

	public void SetInitialControl (IButton button) {
		_initialFocusable = (NativeButton) button;
	}

	public void Show () {
		var w = Width > 0 ? (double) Width : 800;
		var h = Height > 0 ? (double) Height : 600;

		_window = new NsWindow(
			new CgRect(0, 0, w, h),
			NsWindow.StyleMask.Titled | NsWindow.StyleMask.Closable | NsWindow.StyleMask.Resizable,
			2
		);
		_window.Title = Title;

		_window.OnClose(() => {
			if (!NsApplication.Current.IsRunning) return;
			NsApplication.Current.Terminate();
		});

		foreach (var child in _children) {
			child.AttachTo(_window);
		}

		foreach (var group in _radioGroups) {
			ApplyRadioGroupInitialSelection(group);
		}

		RebuildFocusChain();
	}

	public void Append (INativeAttachable child) {
		_children.Add(child);
		if (child is INativeFocusable focusable) _focusOrder.Add(focusable);
		if (_window != null) {
			child.AttachTo(_window);
			RebuildFocusChain();
		}
	}

	public void Append (NativeRadioGroup group) {
		_radioGroups.Add(group);
		foreach (var item in group.Items) {
			var rb = (NativeRadioButton) item;
			_children.Add(rb);
			_focusOrder.Add(rb);
		}

		if (_window != null) {
			foreach (var item in group.Items) {
				((NativeRadioButton) item).AttachTo(_window);
			}

			ApplyRadioGroupInitialSelection(group);
			RebuildFocusChain();
		}
	}

	private void ApplyRadioGroupInitialSelection (NativeRadioGroup group) {
		var idx = group.SelectedIndex;
		if (idx < 0 || idx >= group.Items.Length) return;
		((NativeRadioButton) group.Items[idx]).nsRadioButton!.State = true;
	}

	private void RebuildFocusChain () {
		for (var i = 0; i < _focusOrder.Count - 1; i++) {
			var curr = _focusOrder[i].FocusView;
			var next = _focusOrder[i + 1].FocusView;
			if (curr != null && next != null) curr.SetNextKeyView(next);
		}

		var first = _initialFocusable?.FocusView ?? _focusOrder.FirstOrDefault()?.FocusView;
		if (first != null) _window!.SetInitialFirstResponder(first);
	}
}
