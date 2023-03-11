#if IS_WIN_X64 || IS_WIN_X86 || IS_WIN_ARM
#define WINDOWS
global using CrossForms.Native.Win32;

#elif IS_LINUX_X64 || IS_LINUX_ARM
#define LINUX
global using CrossForms.Native.Stub;

#elif IS_OSX_X64
#define MACOS
global using CrossForms.Native.MacOS;
using System.Runtime.InteropServices;

#else
global using CrossForms.Native.Stub;
#endif

using CrossForms;
using CrossForms.Native;
using CrossForms.Native.Common;

try {
	var app = new NSApplication();
    app.SetActivationPolicy(NSApplication.ActivationPolicy.Regular);

	var mainWindow = new NSWindow(
		new CGRect(0, 0, 400, 300),
		NSWindow.StyleMask.Titled | NSWindow.StyleMask.Closable | NSWindow.StyleMask.Resizable,
		2
	);

	mainWindow.title = "CrossForms App";

	var btn = new NSButton("Test button");
	btn.OnClick(() => {
		Console.WriteLine("CLICK!!!");
	});
	btn.SetFrameOrigin(0, 300 - 0 - btn.Frame.size.height);
	mainWindow.Append(btn);


	var btn2 = new NSButton("Test button 2");
	btn2.OnClick(() => {
		var origin = btn.Frame.origin;
		origin.y += 1;
		btn.SetFrameOrigin(origin);
		Console.WriteLine("CLICK 2!!!");
	});
	btn2.SetFrameOrigin(0, 300 - 22 - btn2.Frame.size.height);
	mainWindow.Append(btn2);

	mainWindow.OnClose(() => Console.WriteLine("main window closed"));

	app.Run();

	// var NSApplication = new ObjClass();
	// var result = ObjC.InitApp();
	// if (result.success) Console.WriteLine($"Ok({result.value})");
	// else Console.WriteLine($"Err({result.error})");
	// result.Dispose();

	// var NSApplication = ObjC.GetClass("NSApplication");
	// Console.WriteLine(NSApplication);
	// NativeApplication.Start();
	// Application.mainWindow = new Form("MainWindow", "Тестовое окно");

	// var testBtn1 = new Button("Hello!") {
	// 	x = 10,
	// 	y = 10,
	// 	width = 200
	// };
	// testBtn1.OnClick += (sender, e) => {
	// 	Console.WriteLine($"Button 1 click at ({e.x}; {e.y})");
	// };
	// Application.mainWindow.Append(testBtn1);

	// var testBtn2 = new Button("Ещё кнопка") {
	// 	x = 10,
	// 	y = 35,
	// 	width = 100
	// };
	// testBtn2.OnClick += (sender, e) => {
	// 	Console.WriteLine($"Button 2 click at ({e.x}; {e.y})");
	// };
	// Application.mainWindow.Append(testBtn2);

	// Application.mainWindow.Show();

	// Application.Run();
} catch (NativeException err) {
	Console.Error.WriteLine(err.ToString());
}
