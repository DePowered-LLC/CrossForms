using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeProgressBar: IProgressBar, INativeAttachable {
	internal NsProgressIndicator? nsProgressIndicator;

	private double _min = 0;
	private double _max = 100;
	private double _value = 0;
	private bool _indeterminate;

	public double Min {
		get => _min;
		set {
			_min = value;
			nsProgressIndicator?.MinValue = value;
		}
	}

	public double Max {
		get => _max;
		set {
			_max = value;
			nsProgressIndicator?.MaxValue = value;
		}
	}

	public double Value {
		get => nsProgressIndicator?.Value ?? _value;
		set {
			_value = value;
			nsProgressIndicator?.Value = value;
		}
	}

	public bool Indeterminate {
		get => _indeterminate;
		set {
			_indeterminate = value;
			nsProgressIndicator?.Indeterminate = value;
		}
	}

	public int X { get; set; }
	public int Y { get; set; }
	public ushort Width { get; set; } = 200;
	public ushort Height { get; set; } = 20;

	public void AttachTo (NsWindow window) {
		var pi = new NsProgressIndicator {
			MinValue = _min,
			MaxValue = _max,
			Value = _value,
			Indeterminate = _indeterminate
		};
		nsProgressIndicator = pi;
		window.ContentView.AddSubview(pi);
		pi.ApplyConstraints(window, X, Y, Width, Height);
	}
}
