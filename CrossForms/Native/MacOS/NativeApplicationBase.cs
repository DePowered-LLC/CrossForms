using CrossForms.Native.Common;
using CrossForms.Native.MacOS.Internals;

namespace CrossForms.Native.MacOS;


public class NativeApplicationBase: ApplicationBase {
	private static NsApplication? _app;

	public new static void Start () {
		_app = NsApplication.BorrowShared();
		_app.SetActivationPolicy(NsApplication.ActivationPolicy.Regular);
		

		var appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
		var terminateSel = ObjSelector.Get("terminate:");

		var quitItemStr = NsString.CloneOwned("Quit " + appName);
		var quitItemKey = NsString.CloneOwned("q");
		var quitItem = NsMenuItem.CreateOwned(quitItemStr, terminateSel, quitItemKey);
		quitItem.SetTarget(_app.inner);
		quitItemKey.Release();
		quitItemStr.Release();

		var appMenu = NsMenu.CreateOwned();
		appMenu.AddItem(quitItem);

		var appMenuItemStr = NsString.CloneOwned(appName);
		var appMenuItem = NsMenuItem.CreateOwned(appMenuItemStr, IntPtr.Zero, NsString.Empty);
		appMenuItem.SetSubmenu(appMenu);
		appMenuItemStr.Release();

		var mainMenu = NsMenu.CreateOwned();
		mainMenu.AddItem(appMenuItem);

		_app.SetMainMenu(mainMenu);
		// todo: should I keep menu and its entries or it can be released?
	}

	public new static bool EventLoop () {
		_app!.Run();
		return false;
	}

	public new static void Dispose () {}

	public new static void Quit () => NsApplication.Current.Terminate();
}
