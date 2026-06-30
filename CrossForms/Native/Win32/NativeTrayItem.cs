using CrossForms.Native.Common;
using CrossForms.Native.Win32.Internals;

using static CrossForms.Native.Win32.Internals.Internals;

namespace CrossForms.Native.Win32;


public class NativeTrayItem: ITrayItem, IDisposable {
	private const uint TrayId = 1;

	public string IconPath { get; set; } = "";
	public byte[]? IconData { get; set; }
	public string? Tooltip { get; set; }
	public Func<ITrayPopup?>? OnClick { get; set; }
	public ITrayMenu? Menu { get; set; }

	private IntPtr _hostHwnd;
	private IntPtr _hIcon;
	private WindowClassEx _windowClass;
	private WndProc? _wndProc;
	private bool _leftClickPending;

	public void Show () {
		ApplicationBase.HasActiveTray = true;
		_wndProc = OnMessage;
		_windowClass = WindowClassEx.CreateDefault("CFTrayHost_" + GetHashCode(), _wndProc);
		_windowClass.Register();

		_hostHwnd = CreateWindowEx(
			WindowStyleEx.Left,
			_windowClass.lpszClassName,
			"",
			WindowStyle.Overlapped,
			0, 0, 0, 0,
			IntPtr.Zero, IntPtr.Zero, GetModuleHandle(), IntPtr.Zero
		);

		if (IconData != null) {
			_hIcon = HIconFromImageData(IconData);
		} else if (IconPath is { Length: > 0 }) {
			_hIcon = LoadImage(IntPtr.Zero, IconPath, ImageIcon, 16, 16, LrLoadFromFile);
		}

		RegisterTrayIcon();
	}

	public void Hide () {
		RemoveTrayIcon();
	}

	private unsafe void RegisterTrayIcon () {
		var tip = Tooltip ?? "";
		var data = new NotifyIconData {
			cbSize = (uint) sizeof(NotifyIconData),
			hWnd = _hostHwnd,
			uID = TrayId,
			uFlags = NifMessage | NifIcon | NifTip,
			uCallbackMessage = WmTray,
			hIcon = _hIcon
		};

		var len = Math.Min(tip.Length, 127);
		for (var i = 0; i < len; i++) data.szTip[i] = tip[i];

		ShellNotifyIcon(NimAdd, ref data);
	}

	private unsafe void RemoveTrayIcon () {
		var data = new NotifyIconData {
			cbSize = (uint) sizeof(NotifyIconData),
			hWnd = _hostHwnd,
			uID = TrayId
		};

		ShellNotifyIcon(NimDelete, ref data);
	}

	private IntPtr OnMessage (IntPtr window, uint msg, IntPtr wParam, IntPtr lParam) {
		if (msg != WmTray) {
			return DefWindowProc(window, msg, wParam, lParam);
		}
		
		var notification = (uint) lParam & 0xFFFF;
		switch (notification) {
			case WmLButtonDown:
				_leftClickPending = true;
				break;
			case WmLButtonUp when _leftClickPending: {
				_leftClickPending = false;
				var popup = OnClick?.Invoke();
				if (popup != null) {
					var pw = (NativeTrayPopup) popup;
					var dpi = NativeApplicationBase.DpiScale;
					SetForegroundWindow(_hostHwnd);
					var (px, py) = GetPopupPosition(pw, dpi);
					popup.ShowNear(px, py);
				} else if (Menu != null) {
					ShowContextMenu();
				}
				break;
			}
			case WmRButtonUp:
				ShowContextMenu();
				break;
		}

		return DefWindowProc(window, msg, wParam, lParam);
	}

	private unsafe (double x, double y) GetPopupPosition (NativeTrayPopup popup, double dpi) {
		var identifier = new NotifyIconIdentifier {
			cbSize = (uint) sizeof(NotifyIconIdentifier),
			hWnd = _hostHwnd,
			uID = TrayId
		};

		if (Shell_NotifyIconGetRect(ref identifier, out var iconRect) != 0) {
			GetCursorPos(out var cur);
			return (cur.x / dpi - popup.Width / 2.0, cur.y / dpi - popup.Height);
		}

		var screenW = GetSystemMetrics(SmCxScreen);
		var screenH = GetSystemMetrics(SmCyScreen);
		var iconCenterX = (iconRect.left + iconRect.right) / 2;
		var iconCenterY = (iconRect.top + iconRect.bottom) / 2;

		var distToHEdge = Math.Min(iconCenterY, screenH - iconCenterY);
		var distToVEdge = Math.Min(iconCenterX, screenW - iconCenterX);

		double x, y;
		if (distToHEdge <= distToVEdge) {
			// Horizontal taskbar: center popup over icon
			x = (iconRect.left + iconRect.right) / 2.0 / dpi - popup.Width / 2.0;
			y = iconCenterY > screenH / 2
				? iconRect.top / dpi - popup.Height
				: iconRect.bottom / dpi;
		} else {
			// Vertical taskbar: center popup vertically on icon
			y = (iconRect.top + iconRect.bottom) / 2.0 / dpi - popup.Height / 2.0;
			x = iconCenterX > screenW / 2
				? iconRect.left / dpi - popup.Width
				: iconRect.right / dpi;               
		}

		return (x, y);
	}

	private void ShowContextMenu () {
		if (Menu == null) return;

		var menu = (NativeTrayMenu) Menu;
		var hMenu = menu.BuildHMenu();

		GetCursorPos(out var pos);
		SetForegroundWindow(_hostHwnd);
		var result = TrackPopupMenu(hMenu, TpmBottomAlign | TpmRightAlign | TpmReturnCmd, pos.x, pos.y, 0, _hostHwnd, IntPtr.Zero);
		PostMessage(_hostHwnd, WmNull2, IntPtr.Zero, IntPtr.Zero);
		DestroyMenu(hMenu);

		if (result > 0) menu.InvokeItem(result - 1);
	}

	public void Dispose () {
		ApplicationBase.HasActiveTray = false;
		RemoveTrayIcon();
		
		if (_hIcon != IntPtr.Zero) {
			DeleteObject(_hIcon);
		}
		
		if (_hostHwnd != IntPtr.Zero) {
			DestroyWindow(_hostHwnd);
		}
		
		_windowClass.UnRegister();
		_wndProc = null;
	}
}
