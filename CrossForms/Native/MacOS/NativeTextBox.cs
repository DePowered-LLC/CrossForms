using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeTextBox: ITextBox {
	internal NsTextInput? nsTextInput;

	private string _text = "";
	private bool _enabled = true;
	private EventHandler<ChangeEvent>? _onChange;

	public string Text {
		get => nsTextInput?.StringValue ?? _text;
		set {
			_text = value;
			nsTextInput?.StringValue = value;
		}
	}

	public bool Enabled {
		get => nsTextInput?.Enabled ?? _enabled;
		set {
			_enabled = value;
			nsTextInput?.Enabled = value;
		}
	}

	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 200;
	public ushort Height { get; set; } = 22;

	public EventHandler<ChangeEvent> OnChange {
		get => _onChange!;
		set => _onChange = value;
	}

	internal NsTextInput CreateNsTextInput () {
		var tf = new NsTextInput(_text);
		if (!_enabled) tf.Enabled = false;
		tf.OnChange(() => _onChange?.Invoke(this, new ChangeEvent { Text = tf.StringValue }));
		return tf;
	}
}
