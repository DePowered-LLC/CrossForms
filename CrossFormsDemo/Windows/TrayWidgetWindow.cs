using CrossForms.Components;

namespace CrossFormsDemo.Windows;


public class TrayWidgetWindow: TrayPopup {
	public TrayWidgetWindow (): base(200, 102) {
		var label = new Label("Test tray popup") {
			X = 10, Y = 10, Width = 180
		};
		Append(label);
		
		var openAppBtn = new Button("Open app") {
			X = 10, Y = 40, Width = 180
		};
		openAppBtn.OnClick += (_, _) => {
			Application.MainWindow?.Show(); 
			Hide();
		};
		Append(openAppBtn);
		
		var quitBtn = new Button("Quit") {
			X = 10, Y = 70, Width = 180
		};
		quitBtn.OnClick += (_, _) => {
			Application.Quit();
		};
		Append(quitBtn);
	}
}
