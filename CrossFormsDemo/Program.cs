using CrossForms.Components;
using CrossForms.Native;
using CrossForms.Platform;

using CrossFormsDemo.Windows;


try {
	Application.Start();
	Application.MainWindow = new MainWindow();
	
	var tray = new TrayItem(Resource.Get("CrossFormsDemo.Assets.TrayIcon.png")) {
		Tooltip = "CrossForms Tray",
		OnClick = () => new TrayWidgetWindow(),  
		Menu = new TrayMenu(
			new TrayMenuItem("Show app") {
				OnClick = () => Application.MainWindow.Show()
			},
			new TrayMenuItem("Quit") {
				OnClick = Application.Quit 
			}
		)   
	};  
	tray.Show();
	
	Application.MainWindow.Show();
	Application.Run();
} catch (NativeException err) {
	Console.Error.WriteLine(err.ToString());
}
