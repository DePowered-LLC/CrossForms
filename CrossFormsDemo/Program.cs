using CrossForms.Components;
using CrossForms.Native;

try {
	Application.Start();
	Application.MainWindow = new Form("MainWindow", "Тестовое окно");

	var testBtn1 = new Button("Hello!") {
		X = 10,
		Y = 10,
		Width = 200,
		Height = 22
	};
	testBtn1.OnClick += (_, e) => {
		Console.WriteLine($"Button 1 click at ({e.x}; {e.y})");
	};
	Application.MainWindow.Append(testBtn1);

	var testBtn2 = new Button("Ещё кнопка") {
		X = 10,
		Y = 35,
		Width = 100,
		Height = 22
	};
	testBtn2.OnClick += (_, e) => {
		Console.WriteLine($"Button 2 click at ({e.x}; {e.y})");
	};
	testBtn2.SetNextControl(testBtn1);
	Application.MainWindow.Append(testBtn2);
	Application.MainWindow.SetInitialControl(testBtn2);

	Application.MainWindow.Show();
	Application.Run();
} catch (NativeException err) {
	Console.Error.WriteLine(err.ToString());
}
