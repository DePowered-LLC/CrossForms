using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeTrayPopup: ITrayPopup {
	private readonly List<INativeAttachable> _children = [];
	private readonly List<INativeFocusable> _focusOrder = [];
	private readonly HashSet<NativeRadioGroup> _radioGroups = [];
	private INativeFocusable? _initialFocusable;

	private NsPanel? _panel;

	public ushort Width { get; set; } = 300;
	public ushort Height { get; set; } = 200;

	private NsPanel EnsurePanel () {
		if (_panel != null) {
			return _panel;
		}

		_panel = NsPanel.CreateOwned(Width, Height);
		foreach (var child in _children) {
			child.AttachTo(_panel);
		}

		foreach (var group in _radioGroups) {
			ApplyRadioGroupSelection(group);
		}

		RebuildFocusChain();
		_children.Clear();
		_focusOrder.Clear();
		return _panel;
	}

	public bool IsVisible => _panel?.IsVisible == true;

	public void ShowNear (double x, double y) {
		EnsurePanel().ShowNear(x, y, Height);
	}

	public void Hide () {
		_panel?.Hide();
	}
	
	// todo: commonize append
	public void Append (INativeAttachable child) {
		if (_panel != null) {
			child.AttachTo(_panel);
			if (child is INativeFocusable focusable) {
				_focusOrder.Add(focusable);
				RebuildFocusChain();
			}
		} else {
			_children.Add(child);
			if (child is INativeFocusable focusable) _focusOrder.Add(focusable);
		}
	}

	public void Append (NativeRadioGroup group) {
		_radioGroups.Add(group);
		foreach (var item in group.Items) {
			var rb = (NativeRadioButton) item;
			if (_panel != null) {
				rb.AttachTo(_panel);
				_focusOrder.Add(rb);
				RebuildFocusChain();
			} else {
				_children.Add(rb);
				_focusOrder.Add(rb);
			}
		}

		if (_panel != null) ApplyRadioGroupSelection(group);
	}

	// todo: commonize tab flow
	public void SetInitialControl (IButton button) {
		_initialFocusable = (NativeButton) button;
	}

	private void ApplyRadioGroupSelection (NativeRadioGroup group) {
		var idx = group.SelectedIndex;
		if (idx < 0 || idx >= group.Items.Length) return;
		((NativeRadioButton) group.Items[idx]).nsRadioButton!.State = true;
	}

	private void RebuildFocusChain () {
		if (_panel == null) return;
		for (var i = 0; i < _focusOrder.Count - 1; i++) {
			var curr = _focusOrder[i].FocusView;
			var next = _focusOrder[i + 1].FocusView;
			if (curr != null && next != null) curr.SetNextKeyView(next);
		}

		var first = _initialFocusable?.FocusView ?? _focusOrder.FirstOrDefault()?.FocusView;
		if (first != null) _panel.SetInitialFirstResponder(first);
	}
}
