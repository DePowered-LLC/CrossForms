using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeForm: IForm {
	private readonly List<NativeButton> _children = [];
	private readonly List<NativeLabel> _labelChildren = [];
	private readonly List<NativeTextBox> _textBoxChildren = [];
	private readonly List<NativeCheckBox> _checkBoxChildren = [];
	private readonly List<NativeRadioButton> _radioButtonChildren = [];
	private readonly HashSet<NativeRadioGroup> _radioGroups = [];
	private NativeButton? _initialControl;

	private NsWindow? _window;
	public string Id { get; set; } = "";
	public string Title { get; set; } = "";
	public ushort Width { get; set; }
	public ushort Height { get; set; }

	public void SetInitialControl (IButton button) {
		_initialControl = (NativeButton) button;
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
			AttachButton(child);
		}

		foreach (var label in _labelChildren) {
			AttachLabel(label);
		}

		foreach (var textBox in _textBoxChildren) {
			AttachTextBox(textBox);
		}

		foreach (var checkBox in _checkBoxChildren) {
			AttachCheckBox(checkBox);
		}

		foreach (var radioButton in _radioButtonChildren) {
			AttachRadioButton(radioButton);
		}

		foreach (var group in _radioGroups) {
			ApplyRadioGroupInitialSelection(group);
		}

		foreach (var btn in _children) {
			if (btn.nextControl?.nsButton != null) {
				btn.nsButton!.SetNextKeyView(btn.nextControl.nsButton);
			}
		}

		var first = _initialControl ?? (_children.Count > 0 ? _children[0] : null);
		if (first?.nsButton != null) {
			_window.SetInitialFirstResponder(first.nsButton);
		}
	}

	public void Append (NativeRadioGroup group) {
		_radioGroups.Add(group);
		foreach (var item in group.Items)
			Append((NativeRadioButton) item);
		if (_window != null) ApplyRadioGroupInitialSelection(group);
	}

	public void Append (NativeRadioButton radioButton) {
		if (radioButton.group != null) _radioGroups.Add(radioButton.group);
		_radioButtonChildren.Add(radioButton);
		if (_window != null) AttachRadioButton(radioButton);
	}

	public void Append (NativeCheckBox checkBox) {
		_checkBoxChildren.Add(checkBox);
		if (_window != null) AttachCheckBox(checkBox);
	}

	public void Append (NativeTextBox textBox) {
		_textBoxChildren.Add(textBox);
		if (_window != null) AttachTextBox(textBox);
	}

	public void Append (NativeLabel label) {
		_labelChildren.Add(label);
		if (_window != null) AttachLabel(label);
	}

	public void Append (NativeButton button) {
		if (_children.Count > 0 && _children[^1].nextControl == null) {
			_children[^1].nextControl = button;
		}

		_children.Add(button);
		if (_window != null) {
			AttachButton(button);
		}
	}

	private void AttachRadioButton (NativeRadioButton radioButton) {
		var nsRb = radioButton.CreateNsRadioButton();
		radioButton.nsRadioButton = nsRb;

		var group = radioButton.group;
		if (group != null) {
			var index = Array.IndexOf(group.Items, radioButton);
			nsRb.OnClick(() => {
				foreach (var item in group.Items) {
					var sibling = (NativeRadioButton) item;
					sibling.nsRadioButton?.State = false;
				}
				
				nsRb.State = true;
				group.NotifyChange(index);
			});
		}

		_window!.Append(nsRb);

		var contentView = _window.ContentView;
		var rw = radioButton.Width > 0 ? (double) radioButton.Width : 120;
		var rh = radioButton.Height > 0 ? (double) radioButton.Height : 22;

		nsRb.LeadingAnchor.ConstraintToAnchor(contentView.LeadingAnchor, radioButton.X).Active = true;
		nsRb.TopAnchor.ConstraintToAnchor(contentView.TopAnchor, radioButton.Y).Active = true;
		nsRb.WidthAnchor.ConstraintToConstant(rw).Active = true;
		nsRb.HeightAnchor.ConstraintToConstant(rh).Active = true;
	}

	private void ApplyRadioGroupInitialSelection (NativeRadioGroup group) {
		var idx = group.SelectedIndex;
		if (idx < 0 || idx >= group.Items.Length) return;
		
		var rb = (NativeRadioButton) group.Items[idx];
		rb.nsRadioButton?.State = true;
	}

	private void AttachCheckBox (NativeCheckBox checkBox) {
		var nsCb = checkBox.CreateNsCheckBox();
		checkBox.nsCheckBox = nsCb;
		_window!.Append(nsCb);

		var contentView = _window.ContentView;
		var cw = checkBox.Width > 0 ? (double) checkBox.Width : 120;
		var ch = checkBox.Height > 0 ? (double) checkBox.Height : 22;

		nsCb.LeadingAnchor.ConstraintToAnchor(contentView.LeadingAnchor, checkBox.X).Active = true;
		nsCb.TopAnchor.ConstraintToAnchor(contentView.TopAnchor, checkBox.Y).Active = true;
		nsCb.WidthAnchor.ConstraintToConstant(cw).Active = true;
		nsCb.HeightAnchor.ConstraintToConstant(ch).Active = true;
	}

	private void AttachTextBox (NativeTextBox textBox) {
		var nsTi = textBox.CreateNsTextInput();
		textBox.nsTextInput = nsTi;
		_window!.Append(nsTi);

		var contentView = _window.ContentView;
		var tw = textBox.Width > 0 ? (double) textBox.Width : 200;
		var th = textBox.Height > 0 ? (double) textBox.Height : 22;

		nsTi.LeadingAnchor.ConstraintToAnchor(contentView.LeadingAnchor, textBox.X).Active = true;
		nsTi.TopAnchor.ConstraintToAnchor(contentView.TopAnchor, textBox.Y).Active = true;
		nsTi.WidthAnchor.ConstraintToConstant(tw).Active = true;
		nsTi.HeightAnchor.ConstraintToConstant(th).Active = true;
	}

	private void AttachLabel (NativeLabel label) {
		var nsTf = label.CreateNsTextField();
		label.nsTextField = nsTf;
		_window!.ContentView.AddSubview(nsTf);

		var contentView = _window.ContentView;
		var lw = label.Width > 0 ? (double) label.Width : 120;
		var lh = label.Height > 0 ? (double) label.Height : 17;

		nsTf.LeadingAnchor.ConstraintToAnchor(contentView.LeadingAnchor, label.X).Active = true;
		nsTf.TopAnchor.ConstraintToAnchor(contentView.TopAnchor, label.Y).Active = true;
		nsTf.WidthAnchor.ConstraintToConstant(lw).Active = true;
		nsTf.HeightAnchor.ConstraintToConstant(lh).Active = true;
	}

	private void AttachButton (NativeButton button) {
		var nsBtn = button.CreateNsButton();
		button.nsButton = nsBtn;
		_window!.Append(nsBtn);
		if (button.nextControl?.nsButton != null) {
			nsBtn.SetNextKeyView(button.nextControl.nsButton);
		}

		var contentView = _window.ContentView;
		var bw = button.Width > 0 ? (double) button.Width : 120;
		var bh = button.Height > 0 ? (double) button.Height : 22;

		nsBtn.LeadingAnchor.ConstraintToAnchor(contentView.LeadingAnchor, button.X).Active = true;
		nsBtn.TopAnchor.ConstraintToAnchor(contentView.TopAnchor, button.Y).Active = true;
		nsBtn.WidthAnchor.ConstraintToConstant(bw).Active = true;
		nsBtn.HeightAnchor.ConstraintToConstant(bh).Active = true;
	}
}
