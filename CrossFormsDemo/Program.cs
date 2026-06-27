using CrossForms;
using CrossForms.Components;
using CrossForms.Native;

try {
	Application.Start();
	Application.mainWindow = new Form("MainWindow", "Тестовое окно");

	var testBtn1 = new Button("Hello!") {
		x = 10,
		y = 10,
		width = 200,
		height = 22
	};
	testBtn1.OnClick += (sender, e) => {
		Console.WriteLine($"Button 1 click at ({e.x}; {e.y})");
	};
	Application.mainWindow.Append(testBtn1);

	var testBtn2 = new Button("Ещё кнопка") {
		x = 10,
		y = 35,
		width = 100,
		height = 22
	};
	testBtn2.OnClick += (sender, e) => {
		Console.WriteLine($"Button 2 click at ({e.x}; {e.y})");
	};
	testBtn2.SetNextControl(testBtn1);
	Application.mainWindow.Append(testBtn2);
	Application.mainWindow.SetInitialControl(testBtn2);

	Application.mainWindow.Show();
	Application.Run();
} catch (NativeException err) {
	Console.Error.WriteLine(err.ToString());
}
