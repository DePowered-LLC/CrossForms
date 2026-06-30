using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeTrayItem: NsEventDispatcher, ITrayItem {
	// todo: enum NSEventType
	// NSEventType: LeftMouseUp=2, RightMouseUp=4
	private const long RightMouseUp = 4L;

	public string IconPath { get; set; } = "";
	public byte[]? IconData { get; set; }
	public string? Tooltip { get; set; }
	public Func<ITrayPopup?>? OnClick { get; set; }
	public ITrayMenu? Menu { get; set; }

	private NsStatusItem? _statusItem;
	private NativeTrayPopup? _currentPopup;

	// todo: make NsEventDispatcher not nested
	public NativeTrayItem (): base(IntPtr.Zero) {
	}

	public void Show () {
		ApplicationBase.HasActiveTray = true;
		NsApplication.Current.SetActivationPolicy(NsApplication.ActivationPolicy.Accessory);
		_statusItem = NsStatusItem.CreateAuto();
		_statusItem.Retain();

		var button = _statusItem.BorrowButton();
		var clickSel = AttachEvent(button, "trayClick:", HandleClick);

		_statusItem.SetupButton(IconPath, IconData, Tooltip, dispatcherInstance, clickSel);
		
		// Menu directly attached only when it's set, but it must be built on the dispatcher
		// before any potential usage 
		((NativeTrayMenu?) Menu)?.Build(this);
		if (OnClick == null && Menu != null) {
			_statusItem.SetMenu(((NativeTrayMenu) Menu).BuiltMenu);
		}
	}

	public void Hide () {
		ApplicationBase.HasActiveTray = false;
		_statusItem?.Remove();
		_statusItem?.Release();
		_statusItem = null;
	}

	private void HandleClick () {
		var ev = NsApplication.Current.BorrowCurrentEvent();
		var isRightClick = ev is { Type: RightMouseUp };

		if (!isRightClick && OnClick != null) {
			// Toggle popup on repeated clicks
			if (_currentPopup != null) {
				if (_currentPopup.IsVisible) {
					_currentPopup.Hide();
					_currentPopup = null;
					return;
				}
				
				_currentPopup = null;
			}

			var popup = OnClick.Invoke();
			if (popup is NativeTrayPopup nativePopup) {
				_currentPopup = nativePopup;
				var loc = NsEvent.MouseLocation;
				nativePopup.ShowNear(loc.x - nativePopup.Width / 2.0, loc.y);
			} else if (popup != null) {
				var loc = NsEvent.MouseLocation;
				popup.ShowNear(loc.x - popup.Width / 2.0, loc.y);
			} else {
				ShowContextMenu(ev);
			}
		} else {
			ShowContextMenu(ev);
		}
	}

	private void ShowContextMenu (NsEvent? ev) {
		var nsMenu = ((NativeTrayMenu?) Menu)?.BuiltMenu;
		if (nsMenu == null || _statusItem == null) return;

		var button = _statusItem.BorrowButton();
		NsMenu.Open(nsMenu, ev, button.inner);
	}
}
