using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeProgressBar: Control, IProgressBar {
	private double _min;
	private double _max = 100;
	private double _value;
	private bool _indeterminate;

	public double Min {
		get => _min;
		set {
			_min = value;
			if (IsLoaded) ApplyRange();
		}
	}

	public double Max {
		get => _max;
		set {
			_max = value;
			if (IsLoaded) ApplyRange();
		}
	}

	public double Value {
		get => IsLoaded
			? (int) SendMessage(handle, (uint) ProgressMessage.GetPos, 0, 0)
			: _value;
		set {
			_value = value;
			if (IsLoaded) SendMessage(handle, (uint) ProgressMessage.SetPos, (int) value, 0);
		}
	}

	public bool Indeterminate {
		get => _indeterminate;
		set {
			_indeterminate = value;
			if (!IsLoaded) return;
			var style = (uint) GetWindowLongPtr(handle, Gwl.Style);
			if (value) {
				SetWindowLongPtr(handle, Gwl.Style, (IntPtr) (style | (uint) ProgressStyle.Marquee));
				SendMessage(handle, (uint) ProgressMessage.SetMarquee, 1, 30);
			} else {
				SendMessage(handle, (uint) ProgressMessage.SetMarquee, 0, 0);
				SetWindowLongPtr(handle, Gwl.Style, (IntPtr) (style & ~(uint) ProgressStyle.Marquee));
				SendMessage(handle, (uint) ProgressMessage.SetPos, (int) _value, 0);
				InvalidateRect(handle, IntPtr.Zero, true);
			}
		}
	}

	public int X { get; set; } = 0;
	public int Y { get; set; } = 0;
	public ushort Width { get; set; } = 200;
	public ushort Height { get; set; } = 20;

	protected override void Load () {
		base.Load();
		ApplyRange();
		SendMessage(handle, (uint) ProgressMessage.SetPos, (int) _value, 0);
		
		if (_indeterminate) {
			SendMessage(handle, (uint) ProgressMessage.SetMarquee, 1, 30);
		}
	}

	private void ApplyRange () =>
		SendMessage(handle, (uint) ProgressMessage.SetRange32, (int) _min, (int) _max);

	protected override ControlCreationOptions GetCreationOptions () => new() {
		className = "msctls_progress32",
		style = WindowStyle.Visible
		        | WindowStyle.Child
		        | (_indeterminate ? (WindowStyle) (uint) ProgressStyle.Marquee : 0),
		label = "",
		width = Width,
		height = Height,
		x = X,
		y = Y
	};
}
